using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DAL;
using Model;
using System.Collections.Generic;
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
            var graph = prepareMockGraph(rand.Next(1000));

            dal.WriteWholeGraph(graph);
            var graphAfterReload = dal.GetGraphById(graph.id);

            Assert.AreEqual(graph, graphAfterReload);
        }

        [TestMethod]
        public void WriteWholeGraphs_ShouldWriteCorrectGraphsAndReadThemBack()
        {
            var rand = new Random();
            var graph1 = prepareMockGraph(rand.Next(1000));
            var graph2 = prepareMockGraph(rand.Next(1000));

            dal.WriteWholeGraphs(new List<Graph>() { graph1, graph2 });
            var graph1AfterReload = dal.GetGraphById(graph1.id);
            var graph2AfterReload = dal.GetGraphById(graph2.id);

            Assert.AreEqual(graph1, graph1AfterReload);
            Assert.AreEqual(graph2, graph2AfterReload);
        }

        private Graph prepareMockGraph(int graphId)
        {
            Graph graph = new Graph()
            {
                id = graphId
            };

            graph.nodes.Add(new Node()
            {
                id = 1,
                label = 2,
                graphId = graph.id
            });

            graph.nodes.Add(new Node()
            {
                id = 2,
                label = 3,
                graphId = graph.id
            });

            graph.edges.Add(new DFS_Code()
            {
                u = 1,
                v = 2,
                l_u = 2,
                l_v = 3,
                l_w = 4,
                GraphID = graph.id
            });

            return graph;
        }
    }
}
