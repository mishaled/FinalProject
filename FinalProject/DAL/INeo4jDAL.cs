using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4j.Driver.V1;
using Model;

namespace DAL
{
    public interface INeo4jDAL
    {
        void DeleteGraphById(int graphId);
        List<int> GetMatchingGraphsIds(List<DFS_Code> path);
        void WriteWholeGraph(Graph graph);
        void WriteWholeGraphs(List<Graph> graphs);
        Graph GetGraphById(int id);
        void BatchWriteWholeGraphs(List<Graph> graphs);
        TimeSpan LoadGraphsFromCsvs(string nodesFilename, string relationshipsFilename);
    }
}
