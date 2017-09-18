using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
using Common;
using DAL;
using Model;

namespace FrequentFeaturesFileWriter
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
            //int minSupMin = int.Parse(args[1]);
            //int minSupMax = int.Parse(args[2]);
            int minSupPercent = int.Parse(args[1]);
            int minSup = int.Parse(args[1]);


            RegisterLogger();
            ILogger logger = DIFactory.Resolve<ILogger>();

            logger.WriteInfo("Start loading from synth DB");

            List<Graph> graphsDb = LoadGraphsFromSynthDb(graphDbFilename);

            //double numberOfGraphsForMinSup = (double)(minSupPercent * graphsDb.Count) / 100;
            //int minSup = (int)Math.Round(numberOfGraphsForMinSup);

            logger.WriteInfo("Finish loading from synth DB");

            //Parallel.For(minSupMin, minSupMax, (i) =>
            //{
            //    double minSup = (double)i / 10;

            //    logger.WriteInfo("Start selecting FF for minSup: " + minSup);

            //    FrequentFeatureSelector selector = new FrequentFeatureSelector();
            //    Dictionary<Graph, List<int>> features = selector.Select(graphsDb, minSup);

            //    logger.WriteInfo("Finish selecting FF for minSup: " + minSup);

            //    logger.WriteInfo("Start writing FF for minSup: " + minSup + " to file");

            //    string filename = FrequentFeaturesFileDal.Write(features, graphDbFilename, minSup);

            //    logger.WriteInfo("Finish writing FF to file: " + filename);

            //});
            logger.WriteInfo("Start selecting FF");
            Stopwatch sw = Stopwatch.StartNew();
            FrequentFeatureSelector selector = new FrequentFeatureSelector();
            Dictionary<Graph, List<int>> features = selector.Select(graphsDb, minSup);
            sw.Stop();
            logger.WriteInfo("Finish selecting FF in: " + sw.Elapsed);

            //string queriesFileName = string.Format("{0}__{1}__{2}.data", graphsDb.Count, minSup.ToString().Replace(".","_"), Guid.NewGuid());

            logger.WriteInfo("Start writing FF to file");

            var filename = FrequentFeaturesFileDal.Write(features, graphDbFilename, minSup);

            logger.WriteInfo("Finish writing FF to file: " + filename);

            Console.Read();
        }

        private static List<Graph> LoadGraphsFromSynthDb(string graphDbFilename)
        {
            SyntheticGraphDatabaseReader reader = new SyntheticGraphDatabaseReader();
            List<Graph> graphsDb = reader.Read(graphDbFilename);
            return graphsDb;
        }

        private static void RegisterLogger()
        {
            ILogger logger = new Logger();
            DIFactory.Register(logger);
        }
    }
}
