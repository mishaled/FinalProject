using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace BL.UnitTests
{
    [TestClass]
    public class SubgraphIsomorphismGeneratorTests
    {
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestMethod]
        public void FindIsomorphicGraphs_GraphIsomorphic_ShouldReturnGraph()
        {
            List<Graph> graphDb = new List<Graph>();
            graphDb.Add(MockGraphFactory.GenerateEulerianGraph());
            graphDb.Add(MockGraphFactory.GenerateSquareGraph());

            Graph subgraph = MockGraphFactory.GenerateSquareGraph();

            SubgraphIsomorphismGenerator gen = new SubgraphIsomorphismGenerator();

            List<Graph> result = gen.FindIsomorphicGraphs(subgraph, graphDb);

            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result.Single().Equals(graphDb.ElementAt(1)));
        }

        [TestMethod]
        public void FindIsomorphicGraphs_GraphNonIsomorphicToAll_ShouldReturnEmpty()
        {
            List<Graph> graphDb = new List<Graph>();
            graphDb.Add(MockGraphFactory.GenerateSquareGraphWithTwoLines());
            graphDb.Add(MockGraphFactory.GenerateSquareGraph());

            Graph subgraph = MockGraphFactory.GenerateEulerianGraph();

            SubgraphIsomorphismGenerator gen = new SubgraphIsomorphismGenerator();

            List<Graph> result = gen.FindIsomorphicGraphs(subgraph, graphDb);

            Assert.IsFalse(result.Any());
        }
    }
}
