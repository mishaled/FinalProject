using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model;
using System.Configuration;
using System.Diagnostics;
using Common;

namespace Neo4jSyntheticDatabaseFiller
{
    class Program
    {
        static void Main(string[] args)
        {
            RegisterDal();

            Console.WriteLine("Start");
            if (args.Length == 0)
            {
                return;
            }
            Console.WriteLine("Start reading synth graphs");

            SyntheticGraphDatabaseReader reader = new SyntheticGraphDatabaseReader();
            List<Graph> graphs = reader.Read(args[0]);

            Console.WriteLine("Finish reading synth graphs");


            foreach (var graph in graphs)
            {
                Stopwatch sw = Stopwatch.StartNew();
                DIFactory.Resolve<INeo4jDAL>().WriteWholeGraph(graph);
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

        private static Neo4jDAL RegisterDal()
        {
            string neo4jUrl = ConfigurationManager.AppSettings.Get("Neo4jUrl");
            string neo4jUsername = ConfigurationManager.AppSettings.Get("Neo4jUsername");
            string neo4jPassword = ConfigurationManager.AppSettings.Get("Neo4jPassword");
            Neo4jDAL dal = new Neo4jDAL(neo4jUrl, neo4jUsername, neo4jPassword);
            DIFactory.Register<INeo4jDAL>(dal);
            return dal;
        }
    }
}
