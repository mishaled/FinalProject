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
            Registration.RegisterDal();

            RegisterLogger();
            var logger = DIFactory.Resolve<ILogger>();

            logger.WriteInfo("Start");

            if (args.Length == 0)
            {
                return;
            }

            logger.WriteInfo("Start reading synth graphs");

            DIFactory.Resolve<INeo4jDAL>().CleanDb();
            SyntheticGraphDatabaseLoader reader = new SyntheticGraphDatabaseLoader(args[0]);

            var sw = Stopwatch.StartNew();
            List<Graph> graphs = reader.Load();
            sw.Stop();

            logger.WriteInfo("Took: " + sw.Elapsed + " to write #" + graphs.Count + " graphs to the db");

            logger.WriteInfo("Finished");

            Console.Read();
        }

        private static void RegisterLogger()
        {
            ILogger logger = new Logger();
            DIFactory.Register(logger);
        }
    }
}