using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4j.Driver.V1;
using Model;

namespace DAL
{
    public class Neo4jDAL : INeo4jDAL
    {
        private const string WRITE_WHOLE_GRAPH =
            @"WITH {graph} as graph
            unwind graph.nodes as node
            create (n:Node {id: node.id, label: node.label, graphId : node.graphId})
            WITH {graph} as graph
            unwind graph.edges as edge
            MATCH (n1:Node {graphId : edge.GraphID, id : edge.u}), (n2:Node {graphId : edge.GraphID, id : edge.v})
            CREATE UNIQUE (n1)-[r:CONNECTED_TO { label: edge.l_w, graphId : edge.GraphID }]->(n2) return n1.id as u, n2.id as v";

        private const string GET_SUBGRAPH_BY_ID_STATEMENT = @"MATCH (u {graphId:{graphId}})-[edge {graphId:{graphId}}]->(v {graphId:{graphId}}) RETURN u.id as u_id, u.label as u_label, edge.label as l_w, v.id as v_id, v.label as v_label";

        public Neo4jDAL(string neo4jUrl, string username, string password)
        {
            //_neo4jDriver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "Aa123456"));
            Neo4jConnectionManager.Initialize(neo4jUrl, username, password);
        }

        public List<int> GetMatchingGraphsIds(List<DFS_Code> path)
        {
            EdgePathToCypherQueryConverter converter = new EdgePathToCypherQueryConverter();
            var neo4jQuery = converter.Convert(path);
            neo4jQuery = "match (n1:Node {id:3})-[:CONNECTED_TO]-(n2:Node {id:0}) return DISTINCT n1.graphId";

            using (ISession session = Neo4jConnectionManager.GetSession())
            {
                var t = session.Run(neo4jQuery);
            }

            return null;
        }

        public void WriteWholeGraph(Graph graph)
        {
            using (ISession session = Neo4jConnectionManager.GetSession())
            {
                session.Run(WRITE_WHOLE_GRAPH, new { graph = graph });
            }
        }

        public void WriteWholeGraphs(List<Graph> graphs)
        {
            graphs.ForEach(graph =>
            {
                WriteWholeGraph(graph);
            });
        }

        public Graph GetGraphById(int id)
        {
            using (ISession session = Neo4jConnectionManager.GetSession())
            {
                var results = session.Run(GET_SUBGRAPH_BY_ID_STATEMENT, new { graphId = id });
                return convertNeo4jResultIntoGraph(results, id);
            }
        }

        private Graph convertNeo4jResultIntoGraph(IStatementResult results, int id)
        {
            Graph graph = new Graph(id);

            foreach (var record in results)
            {
                Node uNode = new Node()
                {
                    graphId = id,
                    id = int.Parse(record["u_id"].ToString()),
                    label = int.Parse(record["u_label"].ToString())
                };
                Node vNode = new Node()
                {
                    graphId = id,
                    id = int.Parse(record["v_id"].ToString()),
                    label = int.Parse(record["v_label"].ToString())
                };
                DFS_Code dfsCode = new DFS_Code()
                {
                    GraphID = id,
                    l_u = uNode.label,
                    l_v = vNode.label,
                    l_w = int.Parse(record["l_w"].ToString()),
                    u = uNode.id,
                    v = vNode.id
                };

                if (!graph.nodes.Contains(uNode))
                {
                    graph.nodes.Add(uNode);
                }

                if (!graph.nodes.Contains(vNode))
                {
                    graph.nodes.Add(vNode);
                }

                if (!graph.edges.Contains(dfsCode))
                {
                    graph.edges.Add(dfsCode);
                }
            }

            return graph;
        }

        private DFS_Code convertNeo4jRelationshipIntoDFSCode(object neo4jRelationship)
        {
            return null;
        }

        private Node convertNeo4jNodeIntoNode(object neo4jNode)
        {
            return null;
        }
    }
}
