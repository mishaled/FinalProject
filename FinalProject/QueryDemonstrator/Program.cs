using BL;
using Common;
using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryDemonstrator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                throw new Exception("Not enough arguments");
            }

            string graphDbFilename = args[0];
            string frequentFeaturesFilename = args[1];
            int minSup = int.Parse(args[2]);

            RegisterLogger();
            ILogger logger = DIFactory.Resolve<ILogger>();

            Registration.RegisterDal();

            List<Graph> graphsDb = LoadGraphsFromSynthDb(graphDbFilename);
            Dictionary<Graph, List<int>> ff = LoadFrequentFeaturesFromFile(frequentFeaturesFilename);
            Graph query = ff.Keys.First();

            logger.WriteInfo("Start building index");

            GIndex gIndex = new GIndex(minSup);
            gIndex.Fill(ff);

            logger.WriteInfo("Finish building index");

            logger.WriteInfo(gIndex.ToString());

            logger.WriteInfo("Start queriying");
            var results = gIndex.Search(query, graphsDb);
            var isomorphismRsults = gIndex.Search(query, graphsDb, false);
            logger.WriteInfo("Finish queriying : " + results.Count + ", " + isomorphismRsults.Count);

            Console.ReadLine();
        }

        private static void RegisterLogger()
        {
            ILogger logger = new Logger();
            DIFactory.Register(logger);
        }

        private static List<Graph> LoadGraphsFromSynthDb(string graphDbFilename)
        {
            SyntheticGraphDatabaseReader reader = new SyntheticGraphDatabaseReader();
            List<Graph> graphsDb = reader.Read(graphDbFilename);
            return graphsDb;
        }

        private static Dictionary<Graph, List<int>> LoadFrequentFeaturesFromFile(string graphDbFilename)
        {
            Dictionary<Graph, List<int>> ff = FrequentFeaturesFileDal.Read(graphDbFilename);

            DIFactory.Resolve<ILogger>().WriteInfo("Fetched: #" + ff.Count + " FF from file");

            return ff;
        }

        //private Graph GenerateQuery()
        //{
        //    Graph query = new Graph();
        //    query.AddNode(0,0)
        //}
    }
}
