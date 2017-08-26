using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Model;
using System.Linq;

namespace DAL.IntegrationTests
{
    [TestClass]
    public class SyntheticGraphDatabaseReaderTests
    {
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestMethod]
        public void ReadDatabase()
        {
            string currentDir = System.AppDomain.CurrentDomain.BaseDirectory;
            string filename = string.Format("{0}Resources\\TestDataSet.data", Path.GetFullPath(Path.Combine(currentDir, @"..\..\")));

            SyntheticGraphDatabaseReader reader = new SyntheticGraphDatabaseReader();
            List<Graph> graphs = reader.Read(filename);

            AssertCorrectnessOfReadDatabase(filename, graphs);
        }

        private void AssertCorrectnessOfReadDatabase(string filename, List<Graph> graphs)
        {
            Graph currentGraph = null;
            string line = string.Empty;

            using (StreamReader sr = File.OpenText(filename))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains("t"))
                    {
                        currentGraph = assertGraph(graphs, line);
                    }
                    else if (line.Contains("v"))
                    {
                        assertNode(currentGraph, line);
                    }
                    else if (line.Contains("e")) // edge
                    {
                        assertEdge(currentGraph, line);
                    }
                }
            }
        }

        private static void assertEdge(Graph currentGraph, string line)
        {
            DFS_Code code = new DFS_Code();
            code.u = int.Parse(line.Split()[1]);
            code.v = int.Parse(line.Split()[2]);
            code.l_u = currentGraph.nodes[code.u].label;
            code.l_v = currentGraph.nodes[code.v].label;
            code.l_w = int.Parse(line.Split()[3]);
            code.support = 1;
            code.GraphID = currentGraph.id;

            CollectionAssert.Contains(currentGraph.edges, code);
        }

        private static void assertNode(Graph currentGraph, string line)
        {
            Node node = new Node();
            node.id = int.Parse(line.Split()[1]);
            node.label = int.Parse(line.Split()[2]);
            node.graphId = currentGraph.id;

            CollectionAssert.Contains(currentGraph.nodes, node);
        }

        private static Graph assertGraph(List<Graph> graphs, string line)
        {
            Graph currentGraph;
            int graphId = int.Parse(line.Split()[2]);
            currentGraph = graphs.FirstOrDefault(x => x.id == graphId);

            Assert.IsNotNull(currentGraph);
            return currentGraph;
        }
    }
}
