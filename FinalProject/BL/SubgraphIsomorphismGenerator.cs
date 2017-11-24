using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace BL
{
    public class SubgraphIsomorphismGenerator
    {
        public bool IsSubgraphIsomorphic(Graph subgraph, Graph supergraph)
        {
            return GenerateIsomorphismMappings(subgraph.edges, supergraph)
                .Any();
        }

        public List<Graph> FindIsomorphicGraphs(Graph subgraph, List<Graph> supergraphCandidates)
        {
            return supergraphCandidates
               .Where(x => IsSubgraphIsomorphic(subgraph, x))
               .ToList();
        }

        public List<Isomorphism> GenerateIsomorphismMappings(List<DFS_Code> C, Graph G)
        {
            List<Isomorphism> iso = new List<Isomorphism>();
            // map vertex 0 in C to vertex x in G if label(0)=label(x)
            foreach (Node node in G.nodes)
            {
                if (node.label != C[0].l_u)
                {
                    continue;
                }

                Isomorphism o = new Isomorphism() { map = new Dictionary<int, int>() };
                o.map.Add(0, node.id);
                iso.Add(o);
            }

            foreach (DFS_Code t in C)
            {
                List<Isomorphism> iso1 = new List<Isomorphism>();
                foreach (Isomorphism o in iso)
                {
                    Node node_u = new Node() { id = o.map[t.u], label = t.l_u }; // node u in G    
                    List<Neighbor> neighbors_node_u = getNeighbors(node_u, G);

                    if (t.v > t.u) // forward edge
                    {
                        foreach (Neighbor x in neighbors_node_u)
                        {
                            if (o.map.ContainsValue(x.id) || x.label != t.l_v || x.edge != t.l_w)
                            {
                                continue;
                            }

                            // copy o to o1
                            Isomorphism o1 = new Isomorphism() { map = new Dictionary<int, int>(o.map) };
                            o1.map.Add(t.v, x.id);
                            // add o1 to iso1
                            iso1.Add(o1);
                        }
                    }
                    else // backward edge
                    {
                        bool isNeighbor = false;
                        Node node_v = new Node() { id = o.map[t.v], label = t.l_v }; // node v in G         
                        // check if node_v is a neighbor of node_u in G
                        foreach (Neighbor x in neighbors_node_u)
                        {
                            if (node_v.id == x.id && node_v.label == x.label)
                            {
                                isNeighbor = true;
                                break;
                            }
                        }

                        if (isNeighbor)
                        {
                            iso1.Add(o); // valid isomorphism
                        }
                    }
                }

                // replace iso by iso1                
                iso.Clear();
                iso.AddRange(iso1);
            }

            return iso;
        }

        private List<Neighbor> getNeighbors(Node node, Graph G)
        {
            List<Neighbor> neighbors = new List<Neighbor>();

            foreach (DFS_Code edge in G.edges)
            {
                if (node.id == edge.u && node.label == edge.l_u) // node is u ==> its neighbor is v
                {
                    neighbors.Add(new Neighbor() { id = edge.v, label = edge.l_v, edge = edge.l_w });
                }
                else if (node.id == edge.v && node.label == edge.l_v) // node is v ==> its neighbor is u
                {
                    neighbors.Add(new Neighbor() { id = edge.u, label = edge.l_u, edge = edge.l_w });
                }
            }

            return neighbors;
        }
    }
}
