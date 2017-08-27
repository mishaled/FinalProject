using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class SyntheticGraphDatabaseReader
    {
        public SyntheticGraphDatabaseReader()
        {

        }

        public List<Graph> Read(string filename)
        {
            List<Graph> D = new List<Graph>();
            string line = "";
            using (StreamReader sr = File.OpenText(filename))
            {
                Graph G = null;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains("t"))
                    {
                        G = createGraph(D, line);
                    }
                    else if (line.Contains("v")) // vertex
                    {
                        if (G != null)
                        {
                            addNodeToGraph(line, G);
                        }
                    }
                    else if (line.Contains("e")) // edge
                    {
                        if (G != null)
                        {
                            addEdgeToGraph(line, G);
                        }
                    }
                }
            }

            return D;
        }

        private static void addEdgeToGraph(string line, Graph G)
        {
            int u = int.Parse(line.Split()[1]);
            int v = int.Parse(line.Split()[2]);
            int label = int.Parse(line.Split()[3]);

            G.AddEdge(u, v, label);
        }

        private static void addNodeToGraph(string line, Graph G)
        {
            int id = int.Parse(line.Split()[1]);
            int label = int.Parse(line.Split()[2]);

            G.AddNode(id, label);
        }

        private static Graph createGraph(List<Graph> D, string line)
        {
            Graph G = new Graph();
            G.nodes = new List<Node>();
            G.edges = new List<DFS_Code>();
            D.Add(G);
            G.id = int.Parse(line.Split()[2]);
            return G;
        }
    }
}