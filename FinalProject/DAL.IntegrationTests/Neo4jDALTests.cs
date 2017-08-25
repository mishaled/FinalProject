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
        [TestMethod]
        public void WriteWhileGraph_ShoulWriteSomething()
        {
            Neo4jDAL dal = new Neo4jDAL();
            var graph = prepareMockGraph();

            dal.WriteWholeGraph(graph);
            var graphAfterReload = dal.GetGraphById(graph.id);

            Assert.AreEqual(graph, graphAfterReload);
        }

        private Graph prepareMockGraph()
        {
            var rand = new Random();

            Graph graph = new Graph()
            {
                id = rand.Next(1000)
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
