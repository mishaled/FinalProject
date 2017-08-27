using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace BL.UnitTests
{
    [TestClass]
    public class PatternMatcherTests
    {
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestMethod]
        public void Verify_GraphAgainstItself_GraphWithOneEdge_ShouldReturnTrue()
        {
            Graph graph = MockGraphFactory.GenerateGraphWithOneEdge();
            VerifyGraphAgainstItself(graph);
        }

        [TestMethod]
        public void Verify_GraphAgainstItself_GenerateSquareGraph_ShouldReturnTrue()
        {
            Graph graph = MockGraphFactory.GenerateSquareGraph();
            VerifyGraphAgainstItself(graph);
        }

        [TestMethod]
        public void Verify_GraphAgainstItself_GenerateSquareGraphWithTwoLines_ShouldReturnTrue()
        {
            Graph graph = MockGraphFactory.GenerateSquareGraphWithTwoLines();
            VerifyGraphAgainstItself(graph);
        }

        [TestMethod]
        public void Verify_GraphAgainstSubset_ShouldReturnTrue()
        {
            Graph graph1 = MockGraphFactory.GenerateSquareGraph();
            Graph graph2 = MockGraphFactory.GenerateSquareGraph();

            graph2.nodes.RemoveAt(graph2.nodes.Count - 1);
            graph2.edges.RemoveAt(graph2.edges.Count - 2);
            graph2.edges.RemoveAt(graph2.edges.Count - 1);


            VerifyTwoGraphs(graph1, graph2);
        }

        public void VerifyGraphAgainstItself(Graph graph)
        {
            VerifyTwoGraphs(graph, graph);
        }

        public void VerifyTwoGraphs(Graph graph1, Graph graph2)
        {
            PatternMatcher matcher = new PatternMatcher();
            bool result = matcher.Verify(graph1, graph2);

            Assert.IsTrue(result);
        }
    }
}
