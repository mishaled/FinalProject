using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
using Common;
using DAL;
using Model;

namespace GIndexVsNeo4jRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 4)
            {
                throw new Exception("Not enough arguments");
            }

            string graphDbFilename = args[0];
            string frequentFeaturesFilename = args[1];
            string queriesFilename = args[2];
            int minSupPercent = int.Parse(args[3]);

            RegisterLogger();
            var logger = DIFactory.Resolve<ILogger>();

            List<Graph> graphsDb = LoadGraphsFromSynthDb(graphDbFilename);
            Dictionary<Graph, List<int>> ff = LoadFrequentFeaturesFromFile(frequentFeaturesFilename);
            List<Graph> queries = LoadFrequentFeaturesFromFile(queriesFilename).Keys.ToList();

            double numberOfGraphsForMinSup = (double)(minSupPercent * graphsDb.Count) / 100;
            int minSup = (int)Math.Round(numberOfGraphsForMinSup);

            RegisterDal();
            DIFactory
                .Resolve<INeo4jDAL>()
                .CleanDb();
            Stopwatch sw = Stopwatch.StartNew();
            LoadNeo4j(graphsDb);
            sw.Stop();
            logger.WriteInfo("Loading Neo4j took: " + sw.Elapsed);

            sw.Restart();
            GIndex gIndex = BuildGIndex(ff, minSup);
            sw.Stop();
            logger.WriteInfo("Building gIndex took: " + sw.Elapsed);
            RunQueries(queries, gIndex, graphsDb);

            Console.Read();
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
            return ff;
        }


        private static void RunQueries(List<Graph> queries, GIndex gIndex, List<Graph> graphDb)
        {
            ResultsCsvWriter writer = new ResultsCsvWriter();
            PatternMatcher patternMatcher = new PatternMatcher();

            foreach (Graph query in queries)
            {
                try
                {
                    Stopwatch neo4jStopWatch = Stopwatch.StartNew();
                    List<Graph> neo4jResult = patternMatcher.Match(query);
                    neo4jStopWatch.Stop();

                    Stopwatch gIndexStopWatch = Stopwatch.StartNew();
                    List<Graph> gIndexResult = gIndex.Search(query, graphDb, true);
                    gIndexStopWatch.Stop();

                    writer.WriteResult(neo4jStopWatch, gIndexStopWatch, neo4jResult.Count, gIndexResult.Count);
                }
                catch (Exception e)
                {
                    DIFactory.Resolve<ILogger>().WriteError(e);
                }

            }
        }

        private static void LoadNeo4j(List<Graph> graphsDb)
        {
            SyntheticGraphDatabaseLoader reader = new SyntheticGraphDatabaseLoader();
            reader.Load(graphsDb);
        }

        private static GIndex BuildGIndex(Dictionary<Graph, List<int>> ff, int minSup)
        {
            GIndex gIndex = new GIndex(minSup);
            gIndex.Fill(ff);

            return gIndex;
        }

        private static void RegisterDal()
        {
            string neo4jUrl = ConfigurationManager.AppSettings.Get("Neo4jUrl");
            string neo4jUsername = ConfigurationManager.AppSettings.Get("Neo4jUsername");
            string neo4jPassword = ConfigurationManager.AppSettings.Get("Neo4jPassword");
            Neo4jDAL dal = new Neo4jDAL(neo4jUrl, neo4jUsername, neo4jPassword);
            DIFactory.Register<INeo4jDAL>(dal);
        }

        private static void RegisterLogger()
        {
            ILogger logger = new Logger();
            DIFactory.Register(logger);
        }
    }
}
