﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4j.Driver.V1;
using Model;

namespace DAL
{
    public class Neo4jDAL
    {
        //private const string WRITE_WHOLE_GRAPHS_NODES_AND_RETURN_IDS_COMMAND =
        //    @"WITH {graph} as graph
        //    unwind graph.nodes as node
        //    create (n:Node {id: node.id, label: node.label})
        //    return n.id as id, n.label as label, ID(n) as internal_id";

        //private const string WRITE_WHOLE_GRAPHS_EDGES =
        //    @"WITH {Graph} as graph
        //    unwind graph.edges as edge
        //    MATCH (n1)
        //    WHERE ID(n1) = edge.u_dbId
        //    MATCH (n2)
        //    WHERE ID(n2) = edge.v_dbId
        //    CREATE (n1)-[r:CONNECTED_TO { label: edge.l_w }]->(n2)
        //    return r.Type as type, r.label as label;";


        private const string WRITE_WHOLE_GRAPH =
            @"WITH {graph} as graph
            unwind graph.nodes as node
            create (n:Node {id: node.id, label: node.label, graphId : node.graphId})
            WITH {graph} as graph
            unwind graph.edges as edge
            MATCH (n1:Node {graphId : edge.GraphID, id : edge.u}), (n2:Node {graphId : edge.GraphID, id : edge.v})
            CREATE UNIQUE (n1)-[r:CONNECTED_TO { label: edge.l_w, graphId : edge.GraphID }]->(n2) return n1.id as u, n2.id as v";

        //private const string GET_SUBGRAPH_BY_ID_STATEMENT = @"MATCH (u {graphId:{graphId}})-[edge {graphId:{graphId}}]->(v {graphId:{graphId}}) RETURN u,edge,v;";
        private const string GET_SUBGRAPH_BY_ID_STATEMENT = @"MATCH (u {graphId:{graphId}})-[edge {graphId:{graphId}}]->(v {graphId:{graphId}}) RETURN u.id as u_id, u.label as u_label, edge.label as l_w, v.id as v_id, v.label as v_label";


        public void WriteWholeGraph(Graph graph)
        {
            using (ISession session = Neo4jConnectionManager.GetSession())
            {
                session.Run(WRITE_WHOLE_GRAPH, new { graph = graph });
            }
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
                //object uNeo4jNode = record["u"];
                Node uNode = new Node()
                {
                    graphId = id,
                    id = int.Parse(record["u_id"].ToString()),
                    label = int.Parse(record["u_label"].ToString())
                };
                //convertNeo4jNodeIntoNode(uNeo4jNode);
                //object vNeo4jNode = record["v"];
                //Node vNode = convertNeo4jNodeIntoNode(vNeo4jNode);
                Node vNode = new Node()
                {
                    graphId = id,
                    id = int.Parse(record["v_id"].ToString()),
                    label = int.Parse(record["v_label"].ToString())
                };
                //object neo4jRelationship = record["edge"];
                DFS_Code dfsCode = new DFS_Code()
                {
                    GraphID = id,
                    l_u = uNode.label,
                    l_v = vNode.label,
                    l_w = int.Parse(record["l_w"].ToString()),
                    u = uNode.id,
                    v = vNode.id
                };
                    //convertNeo4jRelationshipIntoDFSCode(neo4jRelationship);

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


        //public List<Graph> ReadAllGraphs()
        //{
        //    var graphs = new List<Graph>();
        //    var nodes = new List<Node>();
        //    var graph = new Graph();
        //    graphs.Add(graph);

        //    using (ISession session = Neo4jConnectionManager.GetSession())
        //    {
        //        var results = session.Run("Match (n:Node) return n.id as id, n.label as label");

        //        foreach (var record in results)
        //        {
        //            graph.nodes.Add(new Node()
        //            {
        //                id = record["id"].As<int>(),
        //                label = record["label"].As<int>()
        //            });
        //        }
        //    }

        //    return graphs;
        //}
    }
}
