using BL;
using Common;
using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFNeo4jLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                throw new Exception("Not enough arguments");
            }

            string frequentFeaturesFilename = args[0];

            RegisterLogger();
            ILogger logger = DIFactory.Resolve<ILogger>();

            Registration.RegisterDal();

            Dictionary<Graph, List<int>> ff = LoadFrequentFeaturesFromFile(frequentFeaturesFilename);

            logger.WriteInfo("Start cleaning DB");

            DIFactory
                .Resolve<INeo4jDAL>()
                .CleanDb();

            logger.WriteInfo("Finish cleaning DB");

            logger.WriteInfo("Start loading Neo4j");

            Stopwatch sw = Stopwatch.StartNew();
            LoadNeo4j(ff.Keys.ToList());
            sw.Stop();

            logger.WriteInfo("Loading Neo4j took: " + sw.Elapsed);

            Console.ReadLine();
        }

        private static void LoadNeo4j(List<Graph> graphsDb)
        {
            SyntheticGraphDatabaseLoader reader = new SyntheticGraphDatabaseLoader();
            reader.Load(graphsDb);
        }

        private static Dictionary<Graph, List<int>> LoadFrequentFeaturesFromFile(string graphDbFilename)
        {
            Dictionary<Graph, List<int>> ff = FrequentFeaturesFileDal.Read(graphDbFilename);

            DIFactory.Resolve<ILogger>().WriteInfo("Fetched: #" + ff.Count + " FF from file");

            for (int i = 0; i < ff.Count; i++)
            {
                Graph graph = ff.Keys.ElementAt(i);
                FillGraphIdAndFixReferencesHellByCreatingNewNodesAndEdges(i, graph);
            }

            return ff;
        }

        private static void FillGraphIdAndFixReferencesHellByCreatingNewNodesAndEdges(int graphId, Graph graph)
        {
            graph.id = graphId;

            List<DFS_Code> newEdges = new List<DFS_Code>();
            List<Node> newNodes = new List<Node>();

            for (int j = 0; j < graph.nodes.Count; j++)
            {
                Node node = graph.nodes[j];
                node.graphId = graphId;
                node.id = j;

                Node newNode = new Node(node);
                newNodes.Add(newNode);
            }

            graph.nodes = newNodes;

            foreach (DFS_Code edge in graph.edges)
            {
                DFS_Code newEdge = new DFS_Code(edge);
                newEdges.Add(newEdge);

                newEdge.GraphID = graphId;
            }

            graph.edges = newEdges;
        }

        private static void RegisterLogger()
        {
            ILogger logger = new Logger();
            DIFactory.Register(logger);
        }
    }
}
