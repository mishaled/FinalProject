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
            if (args.Length < 2)
            {
                throw new Exception("Not enough arguments");
            }

            string graphDbFilename = args[0];
            string frequentFeaturesFilename = args[1];

            RegisterLogger();
            ILogger logger = DIFactory.Resolve<ILogger>();

            Registration.RegisterDal();

            List<Graph> graphsDb = LoadGraphsFromSynthDb(graphDbFilename);
            Dictionary<Graph, List<int>> ff = LoadFrequentFeaturesFromFile(frequentFeaturesFilename);
            //int minQuerySize = graphsDb.Min(x => x.Size);
            //Graph query = graphsDb.First(x => x.edges.Count <= minQuerySize);
            Graph query = graphsDb.FirstOrDefault(x => x.id == 797);

            logger.WriteInfo("Start building index");

            GIndex gIndex = new GIndex();
            gIndex.Fill(ff);

            logger.WriteInfo(gIndex.ToString());

            logger.WriteInfo("Finish building index");

            Console.ReadLine();

            logger.WriteInfo("Start querying gIndex");

            List<Graph> gIndexResults = gIndex.Search(query);

            logger.WriteInfo("Finish querying gIndex");

            Console.ReadLine();

            logger.WriteInfo("Start querying by brute force isomorphism");

            SubgraphIsomorphismGenerator checker = new SubgraphIsomorphismGenerator();
            List<Graph> isomorphismRsults = checker.FindIsomorphicGraphs(query, graphsDb);

            logger.WriteInfo("Finish querying by brute force isomorphism");

            Console.ReadLine();

            logger.WriteInfo("Start querying neo4j naively");

            PatternMatcher matcher = new PatternMatcher();
            List<Graph> patterMatcherResults = matcher.Match(query);

            logger.WriteInfo("Finish querying neo4j naively");

            Console.ReadLine();

            DIFactory
                .Resolve<ILogger>()
                .WriteInfo(string.Format(
                    "Neo4j: {0}; gIndex: {1}; isomorphism: {2}; query size: {3}",
                    patterMatcherResults.Count,
                    gIndexResults.Count,
                    isomorphismRsults.Count,
                    query.Size
                    ));

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
