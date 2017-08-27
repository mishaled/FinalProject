using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model;
using System.Configuration;
using System.Diagnostics;
using BL;
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

            SyntheticGraphDatabaseLoader reader = new SyntheticGraphDatabaseLoader(args[0]);

            var sw = Stopwatch.StartNew();
            List<Graph> graphs = reader.Load();
            sw.Stop();

            Console.WriteLine("Took: " + sw.Elapsed + " to write #" + graphs.Count + " graphs to the db");

            Console.WriteLine("Finished");

            Console.Read();
        }

        private static void RegisterDal()
        {
            string neo4jUrl = ConfigurationManager.AppSettings.Get("Neo4jUrl");
            string neo4jUsername = ConfigurationManager.AppSettings.Get("Neo4jUsername");
            string neo4jPassword = ConfigurationManager.AppSettings.Get("Neo4jPassword");
            Neo4jDAL dal = new Neo4jDAL(neo4jUrl, neo4jUsername, neo4jPassword);
            DIFactory.Register<INeo4jDAL>(dal);
        }
    }
}
