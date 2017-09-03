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
            Dictionary<Graph, List<int>> ff = selector.Select(graphs, 0.5);

            Assert.AreEqual(25, ff.Count);
        }
        [TestMethod]
        public void Select_PartOfSyntheticDb_ShouldGenerateCorrectGraphIdsForFeatures()
        {
            string currentDir = System.AppDomain.CurrentDomain.BaseDirectory;
            string filename = string.Format("{0}Resources\\TestDataSet.data", Path.GetFullPath(Path.Combine(currentDir, @"..\..\")));

            SyntheticGraphDatabaseReader reader = new SyntheticGraphDatabaseReader();
            List<Graph> graphs = reader.Read(filename, 100);

            FrequentFeatureSelector selector = new FrequentFeatureSelector();
            Dictionary<Graph, List<int>> ff = selector.Select(graphs, 0.5);

            foreach (var subGraph in ff.Keys)
            {
                if (subGraph.Size > 0)
                {
                 Assert.IsTrue(ff[subGraph].Any());
                }

                foreach (var key in ff[subGraph])
                {
                    var superGraph = graphs.First(x => x.id == key);
                    SubgraphIsomorphismGenerator gen = new SubgraphIsomorphismGenerator();
                    var isIsomorphic = gen.IsSubgraphIsomorphic(subGraph, superGraph);
                    Assert.IsTrue(isIsomorphic);
                }
            }
        }
    }
}
