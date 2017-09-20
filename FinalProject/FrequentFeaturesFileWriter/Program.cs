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
            if (args.Length < 1)
            {
                throw new Exception("Not enough arguments");
            }

            string graphDbFilename = args[0];
            //int minSup = int.Parse(args[1]);

            RegisterLogger();
            ILogger logger = DIFactory.Resolve<ILogger>();

            logger.WriteInfo("Start loading from synth DB");

            List<Graph> graphsDb = LoadGraphsFromSynthDb(graphDbFilename);

            logger.WriteInfo("Finish loading from synth DB");

            for (int i = 2000; i <= graphsDb.Count; i += 1000)
            {
                SelectFfAndWriteToFile(logger, graphsDb, i, graphDbFilename);
            }

            Console.Read();
        }

        private static void SelectFfAndWriteToFile(ILogger logger, List<Graph> graphsDb, int minSup, string graphDbFilename)
        {
            logger.WriteInfo("Start selecting FF for: " + minSup);
            Stopwatch sw = Stopwatch.StartNew();
            FrequentFeatureSelector selector = new FrequentFeatureSelector();
            Dictionary<Graph, List<int>> features = selector.Select(graphsDb, minSup);
            sw.Stop();
            logger.WriteInfo("Finish selecting FF for: " + minSup + " in: " + sw.Elapsed);

            logger.WriteInfo("Start writing FF to file");

            var filename = FrequentFeaturesFileDal.Write(features, graphDbFilename, minSup);

            logger.WriteInfo("Finish writing FF to file: " + filename);
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
