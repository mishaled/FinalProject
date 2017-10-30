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

namespace GIndexTriePrinter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                throw new Exception("Not enough arguments");
            }

            string ffFilename = args[0];
            int minSup = int.Parse(args[1]);

            RegisterLogger();
            ILogger logger = DIFactory.Resolve<ILogger>();

            var frequentFeatures = FrequentFeaturesFileDal.Read(ffFilename);

            GIndex gIndex = new GIndex(minSup);
            gIndex.Fill(frequentFeatures);

            Console.WriteLine(gIndex.ToString());

            Console.Read();
        }

        private static void RegisterLogger()
        {
            ILogger logger = new Logger();
            DIFactory.Register(logger);
        }
    }
}
