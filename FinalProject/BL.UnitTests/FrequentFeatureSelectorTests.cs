using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace BL.UnitTests
{
    [TestClass]
    public class FrequentFeatureSelectorTests
    {
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestMethod]
        [Ignore]
        public void Select_WholeSyntheticDb_ShouldGenerateCorrectNumberOfFeatures()
        {
            string currentDir = System.AppDomain.CurrentDomain.BaseDirectory;
            string filename = string.Format("{0}Resources\\TestDataSet.data", Path.GetFullPath(Path.Combine(currentDir, @"..\..\")));

            SyntheticGraphDatabaseReader reader = new SyntheticGraphDatabaseReader();
            var graphs = reader.Read(filename);

            FrequentFeatureSelector selector = new FrequentFeatureSelector();
            Dictionary<Graph, List<int>> ff = selector.Select(graphs, 0.5);

            Assert.AreEqual(31, ff.Count);
        }

        [TestMethod]
        public void Select_PartOfSyntheticDb_ShouldGenerateCorrectNumberOfFeatures()
        {
            string currentDir = System.AppDomain.CurrentDomain.BaseDirectory;
            string filename = string.Format("{0}Resources\\TestDataSet.data", Path.GetFullPath(Path.Combine(currentDir, @"..\..\")));

            SyntheticGraphDatabaseReader reader = new SyntheticGraphDatabaseReader();
            var graphs = reader.Read(filename, 100);

            FrequentFeatureSelector selector = new FrequentFeatureSelector();
            Dictionary<Graph, List<int>> ff = selector.Select(graphs, 10);

            Assert.AreEqual(89, ff.Count);
        }
        [TestMethod]
        public void Select_PartOfSyntheticDb_ShouldGenerateCorrectGraphIdsForFeatures()
        {
            string currentDir = System.AppDomain.CurrentDomain.BaseDirectory;
            string filename = string.Format("{0}Resources\\TestDataSet.data", Path.GetFullPath(Path.Combine(currentDir, @"..\..\")));

            SyntheticGraphDatabaseReader reader = new SyntheticGraphDatabaseReader();
            List<Graph> graphs = reader.Read(filename, 100);

            FrequentFeatureSelector selector = new FrequentFeatureSelector();
            Dictionary<Graph, List<int>> ff = selector.Select(graphs, 10);

            foreach (Graph subGraph in ff.Keys)
            {
                if (subGraph.Size > 0)
                {
                    Assert.IsTrue(ff[subGraph].Any());
                }

                foreach (var key in ff[subGraph])
                {
                    Graph superGraph = graphs.First(x => x.id == key);
                    SubgraphIsomorphismGenerator gen = new SubgraphIsomorphismGenerator();
                    bool isIsomorphic = gen.IsSubgraphIsomorphic(subGraph, superGraph);
                    Assert.IsTrue(isIsomorphic);
                }
            }
        }

        [TestMethod]
        public void CompoteCanonicalLabel_SquareGraph_ShouldSucceed()
        {
            Graph graph = MockGraphFactory.GenerateSquareGraph();

            FrequentFeatureSelector selector = new FrequentFeatureSelector();
            List<DFS_Code> canonicalLabel = selector.ComputeCanonicalLabel(graph);

            Assert.AreEqual("[0 1 0 0 1],[1 2 1 1 2],[2 3 2 2 3],[3 0 3 3 0]", String.Join(",", canonicalLabel));
        }

        [TestMethod]
        public void CompoteCanonicalLabel_OneEdge_ShouldSucceed()
        {
            Graph graph = MockGraphFactory.GenerateGraphWithOneEdge();

            FrequentFeatureSelector selector = new FrequentFeatureSelector();
            List<DFS_Code> canonicalLabel = selector.ComputeCanonicalLabel(graph);

            Assert.AreEqual("[0 1 1 4 2]", String.Join(",", canonicalLabel));
        }
    }
}
