using System;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using Common;
using DAL;
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

        private static INeo4jDAL dal;
        private List<Graph> graphsToCleanup;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            string neo4jUrl = ConfigurationManager.AppSettings.Get("Neo4jUrl");
            string neo4jUsername = ConfigurationManager.AppSettings.Get("Neo4jUsername");
            string neo4jPassword = ConfigurationManager.AppSettings.Get("Neo4jPassword");

            dal = new Neo4jDAL(neo4jUrl, neo4jUsername, neo4jPassword);
            DIFactory.Register<INeo4jDAL>(dal);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            graphsToCleanup = new List<Graph>();

        }

        [TestCleanup]
        public void TestCleanup()
        {
            INeo4jDAL dal = DIFactory.Resolve<INeo4jDAL>();
            graphsToCleanup.ForEach(x => dal.DeleteGraphById(x.id));
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

        [TestMethod]
        public void Match_GraphAgainstItself_GraphWithOneEdge_ShouldReturnTrue()
        {
            Graph graph = MockGraphFactory.GenerateGraphWithOneEdge();

            dal.WriteWholeGraph(graph);
            graphsToCleanup.Add(graph);

            PatternMatcher mathcher = new PatternMatcher();
            List<Graph> matches = mathcher.Match(graph);

            CollectionAssert.Contains(matches, graph);
        }

        [TestMethod]
        public void Match_GraphAgainstSubset_ShouldReturnTrue()
        {
            Graph graph1 = MockGraphFactory.GenerateSquareGraph();
            Graph graph2 = MockGraphFactory.GenerateSquareGraph();

            dal.WriteWholeGraph(graph1);

            graph2.nodes.RemoveAt(graph2.nodes.Count - 1);
            graph2.edges.RemoveAt(graph2.edges.Count - 2);
            graph2.edges.RemoveAt(graph2.edges.Count - 1);


            PatternMatcher mathcher = new PatternMatcher();
            List<Graph> matches = mathcher.Match(graph2);

            CollectionAssert.Contains(matches, graph2);
        }
    }
}
