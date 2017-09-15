using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using DAL;
using Model;

namespace BL
{
    public class SyntheticGraphDatabaseLoader
    {
        private readonly string filename;

        public SyntheticGraphDatabaseLoader(string filename = null)
        {
            this.filename = filename;
        }

        public List<Graph> Load(int? maxNumOfGraphs = null)
        {
            SyntheticGraphDatabaseReader reader = new SyntheticGraphDatabaseReader();
            List<Graph> graphs = reader.Read(filename);

            GraphDatabaseCsvWriter csvWriter = new GraphDatabaseCsvWriter();

            List<Graph> graphsToWrite = new List<Graph>();
            if (maxNumOfGraphs != null)
            {
                graphsToWrite = graphs.Take(maxNumOfGraphs.Value).ToList();
            }
            else
            {
                graphsToWrite = graphs;
            }

            Tuple<string, string> files = csvWriter.Write(graphsToWrite);

            INeo4jDAL dal = DIFactory.Resolve<INeo4jDAL>();
            var elapsed = dal.LoadGraphsFromCsvs(files.Item1, files.Item2);

            return graphsToWrite;
        }

        public List<Graph> Load(List<Graph> graphsDb)
        {
            GraphDatabaseCsvWriter csvWriter = new GraphDatabaseCsvWriter();

            Tuple<string, string> files = csvWriter.Write(graphsDb);

            INeo4jDAL dal = DIFactory.Resolve<INeo4jDAL>();
            dal.LoadGraphsFromCsvs(files.Item1, files.Item2);

            return graphsDb;
        }
    }
}
