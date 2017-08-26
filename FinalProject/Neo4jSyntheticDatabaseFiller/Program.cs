using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model;
using System.Configuration;
using System.Diagnostics;

namespace Neo4jSyntheticDatabaseFiller
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start");
            if (args.Length == 0)
            {
                return;
            }
            Console.WriteLine("Start reading synth graphs");

            SyntheticGraphDatabaseReader reader = new SyntheticGraphDatabaseReader();
            List<Graph> graphs = reader.Read(args[0]);

            Console.WriteLine("Finish reading synth graphs");

            string neo4jUrl = ConfigurationManager.AppSettings.Get("Neo4jUrl");
            string neo4jUsername = ConfigurationManager.AppSettings.Get("Neo4jUsername");
            string neo4jPassword = ConfigurationManager.AppSettings.Get("Neo4jPassword");

            Neo4jDAL dal = new Neo4jDAL(neo4jUrl, neo4jUsername, neo4jPassword);

            foreach (var graph in graphs)
            {
                Stopwatch sw = Stopwatch.StartNew();
                dal.WriteWholeGraph(graph);
                sw.Stop();
                Console.WriteLine("Took: " + sw.Elapsed + " to write graph #" + graphs.IndexOf(graph) + " to the db");

                if (graphs.IndexOf(graph) > 100)
                {
                    break;
                }
            }

            Console.WriteLine("Finished");

            Console.ReadLine();
        }
    }
}
