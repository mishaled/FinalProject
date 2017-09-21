using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Neo4j.Driver.V1;
using Model;
using ILogger = Common.ILogger;

namespace DAL
{
    public class Neo4jDAL : INeo4jDAL
    {
        private const string DELETE_GRAPH_COMMAND =
            @"match (n {graphId : { graphId }}) detach delete n";

        private const string CLEAN_DB_COMMAND =
            @"match (n) detach delete n";

        private const string WRITE_WHOLE_GRAPH =
            @"WITH {graph} as graph
            unwind graph.nodes as node
            create (n:Node {id: node.id, label: node.label, graphId : node.graphId})
            WITH {graph} as graph
            unwind graph.edges as edge
            MATCH (n1:Node {graphId : edge.GraphID, id : edge.u}), (n2:Node {graphId : edge.GraphID, id : edge.v})
            CREATE UNIQUE (n1)-[r:CONNECTED_TO { label: edge.l_w, graphId : edge.GraphID }]->(n2) return n1.id as u, n2.id as v";

        private const string GET_SUBGRAPH_BY_ID_STATEMENT = @"MATCH (u:Node {graphId:{graphId}})-[edge {graphId:{graphId}}]->(v:Node {graphId:{graphId}}) RETURN u.id as u_id, u.label as u_label, edge.label as l_w, v.id as v_id, v.label as v_label";

        private const string LOAD_NODES_FROM_CSVS_COMMAND =
            @"LOAD CSV WITH HEADERS FROM { nodesFilename }  AS nodeCsvLine
                create (n:Node {id: toInt(nodeCsvLine.id), label: toInt(nodeCsvLine.label), graphId : toInt(nodeCsvLine.graphId)})";
        //WITH nodeCsvLine";
        private const string LOAD_RELATIONSHIPS_FROM_CSVS_COMMAND =
            @"LOAD CSV WITH HEADERS FROM { relationshipsFilename } AS relationshipCsvLine
            MATCH(n1:Node { graphId: toInt(relationshipCsvLine.GraphID), id: toInt(relationshipCsvLine.u)}), (n2:Node {graphId : toInt(relationshipCsvLine.GraphID), id : toInt(relationshipCsvLine.v)})
            CREATE (n1)-[r: CONNECTED_TO { label: toInt(relationshipCsvLine.label), graphId : toInt(relationshipCsvLine.GraphID) }]->(n2)";

        public Neo4jDAL(string neo4jUrl, string username, string password)
        {
            //_neo4jDriver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "Aa123456"));
            Neo4jConnectionManager.Initialize(neo4jUrl, username, password);
        }

        public TimeSpan LoadGraphsFromCsvs(string nodesFilename, string relationshipsFilename)
        {
            string importFolderPath = @"C:\Users\misha\Documents\Neo4j\default.graphdb\import";

            FileInfo nodesFile = new FileInfo(nodesFilename);
            string newNodesFilepath = Path.Combine(importFolderPath, nodesFile.Name);
            File.Copy(nodesFile.FullName, newNodesFilepath, true);

            FileInfo relationshipsFile = new FileInfo(relationshipsFilename);
            string newRelationshipsFilepath = Path.Combine(importFolderPath, relationshipsFile.Name);
            File.Copy(relationshipsFile.FullName, newRelationshipsFilepath, true);

            var nodesFileNameLocal = "file:///" + nodesFile.Name;
            var relationshipsFilenameLocal = "file:///" + relationshipsFile.Name;

            Stopwatch sw = Stopwatch.StartNew();
            using (ISession session = Neo4jConnectionManager.GetSession())
            {
                session.Run(LOAD_NODES_FROM_CSVS_COMMAND, new { nodesFilename = nodesFileNameLocal });
            }

            using (ISession session = Neo4jConnectionManager.GetSession())
            {
                session.Run(LOAD_RELATIONSHIPS_FROM_CSVS_COMMAND, new { relationshipsFilename = relationshipsFilenameLocal });
            }
            sw.Stop();

            File.Delete(nodesFilename);
            File.Delete(relationshipsFilename);

            return sw.Elapsed;
        }

        public void DeleteGraphById(int graphId)
        {
            using (ISession session = Neo4jConnectionManager.GetSession())
            {
                session.Run(DELETE_GRAPH_COMMAND, new { graphId });

                DIFactory
                    .Resolve<ILogger>()
                    .WriteInfo("Finished deleting graph: " + graphId);
            }
        }

        public void CleanDb()
        {
            using (ISession session = Neo4jConnectionManager.GetSession())
            {
                session.Run(CLEAN_DB_COMMAND);

                DIFactory
                    .Resolve<ILogger>()
                    .WriteInfo("Finished cleaning DB");
            }
        }

        public List<int> GetMatchingGraphsIds(List<DFS_Code> path)
        {
            EdgePathToCypherQueryConverter converter = new EdgePathToCypherQueryConverter();
            string neo4jQuery = converter.Convert(path);

            List<int> ids = new List<int>();

            using (ISession session = Neo4jConnectionManager.GetSession())
            {
                IStatementResult results = session.Run(neo4jQuery);

                foreach (IRecord record in results)
                {
                    ids.Add(int.Parse(record["graphId"].ToString()));
                }
            }

            //DIFactory
            //    .Resolve<ILogger>()
            //    .WriteInfo("Finished getting matching graph ids: " + string.Join(",", ids));

            return ids
                .Distinct()
                .ToList();
        }

        public void WriteWholeGraph(Graph graph)
        {
            using (ISession session = Neo4jConnectionManager.GetSession())
            {
                session.Run(WRITE_WHOLE_GRAPH, new { graph = graph });

                DIFactory
                    .Resolve<ILogger>()
                    .WriteInfo("Finished writing whole graph to DB: " + graph.id);
            }
        }

        public void WriteWholeGraphs(List<Graph> graphs)
        {
            graphs.ForEach(WriteWholeGraph);
        }

        public void BatchWriteWholeGraphs(List<Graph> graphs)
        {
            using (ISession session = Neo4jConnectionManager.GetSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        graphs.ForEach(graph =>
                        {
                            Stopwatch sw = Stopwatch.StartNew();
                            transaction.Run(WRITE_WHOLE_GRAPH, new { graph });
                            sw.Stop();

                            DIFactory
                                .Resolve<ILogger>()
                                .WriteInfo("Took: " + sw.Elapsed + " to write graph #" + graphs.IndexOf(graph) + " to the db");
                        });
                    }
                    catch
                    {
                        transaction.Failure();
                    }

                    transaction.Success();
                }
            }
        }

        public Graph GetGraphById(int id)
        {
            using (ISession session = Neo4jConnectionManager.GetSession())
            {
                var results = session.Run(GET_SUBGRAPH_BY_ID_STATEMENT, new { graphId = id });

                //DIFactory
                //    .Resolve<ILogger>()
                //    .WriteInfo("Finished getting graph by id: " + id);

                return convertNeo4jResultIntoGraph(results, id);
            }
        }

        //public Graph GetAllGraphIds()
        //{
        //    using (ISession session = Neo4jConnectionManager.GetSession())
        //    {
        //        var results = session.Run(GET_SUBGRAPH_BY_ID_STATEMENT, new { graphId = id });

        //        //DIFactory
        //        //    .Resolve<ILogger>()
        //        //    .WriteInfo("Finished getting graph by id: " + id);

        //        return convertNeo4jResultIntoGraph(results, id);
        //    }
        //}

        private Graph convertNeo4jResultIntoGraph(IStatementResult results, int id)
        {
            Graph graph = new Graph(id);

            foreach (var record in results)
            {
                int u_id = int.Parse(record["u_id"].ToString());
                int u_label = int.Parse(record["u_label"].ToString());
                int v_id = int.Parse(record["v_id"].ToString());
                int v_label = int.Parse(record["v_label"].ToString());
                int edge_label = int.Parse(record["l_w"].ToString());

                if (!graph.DoesNodeExist(u_id, u_label))
                {
                    graph.AddNode(u_id, u_label);
                }

                if (!graph.DoesNodeExist(v_id, v_label))
                {
                    graph.AddNode(v_id, v_label);
                }

                if (!graph.DoesEdgeExist(u_id, v_id, edge_label))
                {
                    graph.AddEdge(u_id, v_id, edge_label);
                }
            }

            return graph;
        }
    }
}
