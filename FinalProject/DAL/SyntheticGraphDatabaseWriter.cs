using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace DAL
{
    public class SyntheticGraphDatabaseWriter
    {
        public void Write(string filename, List<Graph> graphDb)
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                foreach (Graph graph in graphDb)
                {
                    WriteGraph(sw, graph);
                }
            }
        }

        private void WriteGraph(StreamWriter sw, Graph graph)
        {
            sw.WriteLine(string.Format("t # {0}", graph.id));
            sw.Flush();

            WriteGraphVertices(sw, graph.nodes);
            WriteGraphEdges(sw, graph.edges);
        }

        private void WriteGraphEdges(StreamWriter sw, List<DFS_Code> graphEdges)
        {
            foreach (DFS_Code edge in graphEdges)
            {
                sw.WriteLine(string.Format("e {0} {1} {2}", edge.u, edge.v, edge.l_w));
                sw.Flush();
            }
        }

        private void WriteGraphVertices(StreamWriter sw, List<Node> graphNodes)
        {
            foreach (Node node in graphNodes)
            {
                sw.WriteLine(string.Format("v {0} {1}", node.id, node.label));
                sw.Flush();
            }
        }
    }
}