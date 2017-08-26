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
            DFS_Code code = new DFS_Code();
            code.u = int.Parse(line.Split()[1]);
            code.v = int.Parse(line.Split()[2]);
            code.l_u = G.nodes[code.u].label;
            code.l_v = G.nodes[code.v].label;
            code.l_w = int.Parse(line.Split()[3]);
            code.support = 1;
            code.GraphID = G.id;

            G.edges.Add(code);
        }

        private static void addNodeToGraph(string line, Graph G)
        {
            Node node = new Node();
            node.id = int.Parse(line.Split()[1]);
            node.label = int.Parse(line.Split()[2]);
            node.graphId = G.id;

            G.nodes.Add(node);
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