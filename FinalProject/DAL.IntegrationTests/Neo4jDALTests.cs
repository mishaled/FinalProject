using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DAL;
using Model;
using System.Collections.Generic;
using Common;
using Newtonsoft;

namespace DAL.IntegrationTests
{
    [TestClass]
    public class Neo4jDALTests
    {
        private const string NEO4J_URL = "bolt://localhost:7687";
        private const string USERNAME = "neo4j";
        private const string PASSROWD = "Aa123456";
        private Neo4jDAL dal;

        [TestInitialize]
        public void TestInitialize()
        {
            dal = new Neo4jDAL(NEO4J_URL, USERNAME, PASSROWD);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            dal = null;
        }


        [TestMethod]
        public void WriteWholeGraph_ShouldWriteCorrectGraphAndReadItBack()
        {
            var rand = new Random();
            Graph graph = MockGraphFactory.GenerateGraphWithOneEdge(10000 + rand.Next(1000));

            dal.WriteWholeGraph(graph);
            var graphAfterReload = dal.GetGraphById(graph.id);

            Assert.AreEqual(graph, graphAfterReload);
        }

        [TestMethod]
        public void WriteWholeGraphs_ShouldWriteCorrectGraphsAndReadThemBack()
        {
            var rand = new Random();
            Graph graph1 = MockGraphFactory.GenerateGraphWithOneEdge(10000 + rand.Next(1000));
            Graph graph2 = MockGraphFactory.GenerateGraphWithOneEdge(10000 + rand.Next(1000));

            dal.WriteWholeGraphs(new List<Graph>() { graph1, graph2 });
            var graph1AfterReload = dal.GetGraphById(graph1.id);
            var graph2AfterReload = dal.GetGraphById(graph2.id);

            Assert.AreEqual(graph1, graph1AfterReload);
            Assert.AreEqual(graph2, graph2AfterReload);
        }

        [TestMethod]
        public void LoadGraphsFromCsvs_ShouldWriteCorrectGraphAndReadItBack()
        {
            var rand = new Random();
            Graph graph = MockGraphFactory.GenerateGraphWithOneEdge(10000 + rand.Next(1000));

            var writer = new GraphDatabaseCsvWriter();
            var files = writer.Write(new List<Graph>() { graph });

            dal.LoadGraphsFromCsvs(files.Item1, files.Item2);
            var graphAfterReload = dal.GetGraphById(graph.id);

            Assert.AreEqual(graph, graphAfterReload);
        }
    }
}
