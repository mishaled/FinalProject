using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace DAL
{
    public class GraphDatabaseCsvWriter
    {
        public Tuple<string, string> Write(List<Graph> graphDatabase)
        {
            string nodesFileName = Path.Combine(Environment.CurrentDirectory, Guid.NewGuid().ToString());
            string relationshipsFileName = Path.Combine(Environment.CurrentDirectory, Guid.NewGuid().ToString());

            using (StreamWriter nodesSw = new StreamWriter(nodesFileName))
            using (StreamWriter relationshipsSw = new StreamWriter(relationshipsFileName))
            {
                nodesSw.WriteLine("id,label,graphId");
                relationshipsSw.WriteLine("u,v,label,GraphID");

                foreach (Graph graph in graphDatabase)
                {
                    foreach (Node node in graph.nodes)
                    {
                        nodesSw.WriteLine($"{node.id},{node.label},{node.graphId}");
                    }

                    foreach (DFS_Code edge in graph.edges)
                    {
                        relationshipsSw.WriteLine($"{edge.u},{edge.v},{edge.l_w},{edge.GraphID}");
                    }
                }
            }

            return new Tuple<string, string>(nodesFileName, relationshipsFileName);
        }
    }
}
