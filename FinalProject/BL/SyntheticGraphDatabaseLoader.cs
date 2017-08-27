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

        public SyntheticGraphDatabaseLoader(string filename)
        {
            this.filename = filename;
        }

        public List<Graph> Load()
        {
            SyntheticGraphDatabaseReader reader = new SyntheticGraphDatabaseReader();
            List<Graph> graphs = reader.Read(filename);

            GraphDatabaseCsvWriter csvWriter = new GraphDatabaseCsvWriter();
            var partList = graphs.Take(200).ToList();
            Tuple<string, string> files = csvWriter.Write(partList);

            INeo4jDAL dal = DIFactory.Resolve<INeo4jDAL>();
            var elapsed = dal.LoadGraphsFromCsvs(files.Item1, files.Item2);

            return partList;
        }
    }
}
