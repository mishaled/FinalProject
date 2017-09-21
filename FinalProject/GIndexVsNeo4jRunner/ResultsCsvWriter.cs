using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Model;

namespace GIndexVsNeo4jRunner
{
    public class ResultsCsvWriter
    {
        private StreamWriter _sw;

        public ResultsCsvWriter()
        {
            _sw = new StreamWriter("Results_" + Guid.NewGuid().ToString());
            _sw.WriteLine("Neo4jTime,Neo4jResults,gIndexTime,gIndexResults,isomorphismTime,isomorphismResult,querySize");
        }

        ~ResultsCsvWriter()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            if (_sw.BaseStream != null)
            {
                _sw.Close();
            }
        }

        public void WriteResult(Stopwatch neo4jWatch, Stopwatch gIndexWatch, Stopwatch isomorphismWatch, int neo4jResultsNum, int gIndexResultsNum, int isomorphismResult, Graph query)
        {
            DIFactory
                .Resolve<ILogger>()
                .WriteInfo(string.Format(
                    "Neo4j: {0}, {1}; gIndex: {2}, {3}; isomorphism: {4}, {5}; query size: {6}",
                    neo4jResultsNum,
                    neo4jWatch.Elapsed,
                    gIndexResultsNum,
                    gIndexWatch.Elapsed,
                    isomorphismResult,
                    isomorphismWatch.Elapsed,
                    query.Size
                    ));

            _sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6}", neo4jWatch.Elapsed, neo4jResultsNum, gIndexWatch.Elapsed, gIndexResultsNum, isomorphismWatch.Elapsed, isomorphismResult, query.Size));
            _sw.Flush();
        }
    }
}
