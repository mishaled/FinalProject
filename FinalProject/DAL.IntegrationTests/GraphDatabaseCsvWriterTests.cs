using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace DAL.IntegrationTests
{
    [TestClass]
    public class GraphDatabaseCsvWriterTests
    {
        private TestContext testContextInstance;
        public TestContext TestContext { get { return testContextInstance; } set { testContextInstance = value; } }

        [TestMethod]
        public void Write_OneGraph_ShouldWriteCorrectly()
        {
            Graph graph = MockGraphFactory.GenerateGraphWithOneEdge();
            GraphDatabaseCsvWriter writer = new GraphDatabaseCsvWriter();

            var files = writer.Write(new List<Graph>() { graph });

            AssertNodesInFile(files.Item1, graph);
            AssertRelationShipsInFile(files.Item2, graph);
        }

        [TestMethod]
        public void Write_TwoGraphs_ShouldWriteCorrectly()
        {
            Graph graph1 = MockGraphFactory.GenerateGraphWithOneEdge(1);
            Graph graph2 = MockGraphFactory.GenerateSquareGraph(2);
            GraphDatabaseCsvWriter writer = new GraphDatabaseCsvWriter();

            var files = writer.Write(new List<Graph>() { graph1,graph2 });

            AssertNodesInFile(files.Item1, graph1);
            AssertRelationShipsInFile(files.Item2, graph1);

            AssertNodesInFile(files.Item1, graph2);
            AssertRelationShipsInFile(files.Item2, graph2);
        }

        private void AssertNodesInFile(string filename, Graph graph)
        {
            using (StreamReader nodesSr = new StreamReader(filename))
            {
                List<Node> nodesInFile = new List<Node>();
                nodesSr.ReadLine();

                while (!nodesSr.EndOfStream)
                {
                    string line = nodesSr.ReadLine();

                    string[] lineNode = line.Split(',');

                    if (int.Parse(lineNode[2]) != graph.id)
                    {
                        continue;
                    }

                    Node node = graph.nodes.FirstOrDefault(x => x.id == int.Parse(lineNode[0]));

                    Assert.IsNotNull(node);
                    Assert.AreEqual(node.label, int.Parse(lineNode[1]));
                    Assert.AreEqual(node.graphId, int.Parse(lineNode[2]));

                    nodesInFile.Add(node);
                }

                CollectionAssert.AreEqual(nodesInFile, graph.nodes);
            }
        }

        private void AssertRelationShipsInFile(string filename, Graph graph)
        {
            using (StreamReader edgesSr = new StreamReader(filename))
            {
                List<DFS_Code> relationshipdInFile = new List<DFS_Code>();
                edgesSr.ReadLine();

                while (!edgesSr.EndOfStream)
                {
                    string line = edgesSr.ReadLine();

                    string[] lineRelationship = line.Split(',');

                    if (int.Parse(lineRelationship[3]) != graph.id)
                    {
                        continue;
                    }

                    DFS_Code edge = graph.edges.FirstOrDefault(x => x.u == int.Parse(lineRelationship[0]) && x.v == int.Parse(lineRelationship[1]));

                    Assert.IsNotNull(edge);
                    Assert.AreEqual(edge.l_w, int.Parse(lineRelationship[2]));
                    Assert.AreEqual(edge.GraphID, int.Parse(lineRelationship[3]));

                    relationshipdInFile.Add(edge);
                }

                CollectionAssert.AreEqual(relationshipdInFile, graph.edges);
            }
        }
    }
}
