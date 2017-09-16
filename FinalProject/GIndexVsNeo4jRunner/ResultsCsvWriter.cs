using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace GIndexVsNeo4jRunner
{
    public class ResultsCsvWriter
    {
        private StreamWriter _sw;

        public ResultsCsvWriter()
        {
            _sw = new StreamWriter("Results_" + Guid.NewGuid().ToString());
        }

        ~ResultsCsvWriter()
        {
            _sw.Close();
        }

        public void WriteResult(Stopwatch neo4jWatch, Stopwatch gIndexWatch, int neo4jResultsNum, int gIndexResultsNum)
        {
            DIFactory
                .Resolve<ILogger>()
                .WriteInfo("Neo4j found: " + neo4jResultsNum + " in :" + neo4jWatch.Elapsed + ", and gIndex found: " + gIndexResultsNum + " in: " + gIndexWatch.Elapsed);
            _sw.WriteLine(string.Format("{0},{1},{2},{3};", neo4jWatch.Elapsed, neo4jResultsNum, gIndexWatch.Elapsed, gIndexResultsNum));
            _sw.Flush();
        }
    }
}
