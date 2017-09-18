using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace BL
{
    public class FrequentFeatureSelector
    {
        public Dictionary<Graph, List<int>> Select(List<Graph> graphDb, double minSup)
        {
            List<DFS_Code> C = new List<DFS_Code>();
            Dictionary<Graph, List<int>> frequentFeatures = gSpan(C, graphDb, minSup);

            return frequentFeatures;
        }

        public List<DFS_Code> ComputeCanonicalLabel(Graph graph)
        {
            List<DFS_Code> C = new List<DFS_Code>();
            List<List<DFS_Code>> labelCandidates = ComputeCanonicalLabelCandidatesRecurse(C, graph);

            List<DFS_Code> maxLabel = labelCandidates
                .OrderByDescending(x => string.Join(",", x))
                .First();

            return maxLabel;
        }

        private List<List<DFS_Code>> ComputeCanonicalLabelCandidatesRecurse(List<DFS_Code> C, Graph graph)
        {
            List<int> graphIds = new List<int>();
            List<Graph> D = new List<Graph>() { graph };
            List<DFS_Code> extensions = RightMostPath_Extensions(C, D, graphIds); // get extensions of graph G(C)

            foreach (DFS_Code t in extensions)
            {
                // deep copy (clone) C to C1
                List<DFS_Code> C1 = new List<DFS_Code>(C);
                C1.Add(t); // generate a new graph by adding an extension to the old graph                           

                if (t.support >= 1 && IsCanonical(C1, t.support)) // support of the new graph is support of extension t
                {
                    var labelCandidates = ComputeCanonicalLabelCandidatesRecurse(C1, graph);
                    labelCandidates.Add(C1);

                    return labelCandidates;
                }
            }

            return new List<List<DFS_Code>>() { C };
        }

        private Dictionary<Graph, List<int>> gSpan(List<DFS_Code> C, List<Graph> D, double minSup)
        {
            List<int> graphIds = new List<int>();
            List<DFS_Code> extensions = RightMostPath_Extensions(C, D, graphIds); // get extensions of graph G(C)

            Dictionary<Graph, List<int>> dict = new Dictionary<Graph, List<int>>();

            if (graphIds.Any())
            {
                Graph fullGraph = new Graph(C);
                dict.Add(fullGraph, graphIds);
                DIFactory.Resolve<ILogger>().WriteInfo("Found frequent feature");
            }

            foreach (DFS_Code t in extensions)
            {
                // deep copy (clone) C to C1
                List<DFS_Code> C1 = new List<DFS_Code>(C);
                C1.Add(t); // generate a new graph by adding an extension to the old graph                           

                if (t.support >= minSup && IsCanonical(C1, t.support)) // support of the new graph is support of extension t
                {
                    Dictionary<Graph, List<int>> graphs = gSpan(C1, D, minSup);
                    dict = dict.Union(graphs).ToDictionary(x => x.Key, y => y.Value);
                }
            }

            return dict;
        }

        private List<DFS_Code> RightMostPath_Extensions(List<DFS_Code> C, List<Graph> D, List<int> graphIds = null)
        {
            List<Node> R = null;
            Node ur = null;
            //graphIds = new List<int>();

            if (C.Count > 0)
            {
                // find nodes on the rightmost path in C
                R = getNodesOnRightMostPath(C);
                // get DFS Code of the rightmost child in C
                ur = R[0];
            }

            List<DFS_Code> extensions = new List<DFS_Code>();

            foreach (Graph G in D)
            {
                ComputeRightmostPathForGraph(C, graphIds, G, extensions, ur, R);
            }

            // in extensions, there are no duplicate tupes in the same graph 
            // compute the support of each extension            
            RemoveDuplicateTuples(extensions);

            // sort extensions
            SortTuplesInRightmostPath(extensions);

            return extensions;
        }

        private static void SortTuplesInRightmostPath(List<DFS_Code> extensions)
        {
            extensions.Sort();
            //for (int i = 0; i < extensions.Count() - 1; i++)
            //{
            //    for (int j = i + 1; j < extensions.Count(); j++)
            //    {
            //        if (extensions[j].LessThan(extensions[i]))
            //        {
            //            DFS_Code code = extensions[i];
            //            extensions[i] = extensions[j];
            //            extensions[j] = code;
            //        }
            //    }
            //}
        }

        //private static List<DFS_Code> RemoveDuplicateTuples(List<DFS_Code> extensions)
        private static void RemoveDuplicateTuples(List<DFS_Code> extensions)
        {
            for (int i = 0; i < extensions.Count() - 1; i++)
            {
                DFS_Code s = extensions[i];
                for (int j = i + 1; j < extensions.Count(); j++)
                {
                    DFS_Code t = extensions[j];

                    if (s.u == t.u && s.v == t.v && s.l_u == t.l_u && s.l_v == t.l_v && s.l_w == t.l_w)
                    {
                        s.support += 1;
                        extensions.RemoveAt(j);
                        j--;
                    }
                }
            }
        }

        private void ComputeRightmostPathForGraph(List<DFS_Code> C, List<int> graphIds, Graph G, List<DFS_Code> extensions, Node ur, List<Node> R)
        {
            if (C.Count == 0) // root node
            {
                HandleRightmostPathRootNode(G, extensions);
            }
            else
            {
                HandleRightmostPathNonRootNode(C, graphIds, G, ur, R, extensions);
            }
        }

        private void HandleRightmostPathNonRootNode(
            List<DFS_Code> C,
            List<int> graphIds,
            Graph G,
            Node ur,
            List<Node> R,
            List<DFS_Code> extensions)
        {
            SubgraphIsomorphismGenerator generator = new SubgraphIsomorphismGenerator();
            List<Isomorphism> iso = generator.GenerateIsomorphismMappings(C, G);

            if (!iso.Any())
            {
                return;
            }

            if (graphIds != null && !graphIds.Contains(G.id))
            {
                graphIds.Add(G.id);
            }

            foreach (Isomorphism o in iso)
            {
                // backward extensions from the rightmost child
                Node node_ur = new Node() { id = o.map[ur.id], label = ur.label }; // node ur in G
                foreach (Neighbor x in getNeighbors(node_ur, G))
                {
                    GenerateBackwardExtention(C, G, ur, R, extensions, o, x);
                }

                // forward extensions from nodes on rightmost path
                foreach (Node u in R)
                {
                    GenerateForwardExtention(G, ur, extensions, o, u);
                }
            }
        }

        private void GenerateForwardExtention(Graph G, Node ur, List<DFS_Code> extensions, Isomorphism o, Node u)
        {
            Node node_u = new Node() { id = o.map[u.id], label = u.label }; // node u in G
            foreach (Neighbor x in getNeighbors(node_u, G))
            {
                // node v is a mapping of node x in C
                int v = -1;
                foreach (var kv in o.map)
                {
                    if (kv.Value == x.id)
                    {
                        v = kv.Key;
                        break;
                    }
                }
                if (v == -1)
                {
                    DFS_Code f = new DFS_Code()
                    {
                        u = u.id,
                        v = ur.id + 1,
                        l_u = node_u.label,
                        l_v = x.label,
                        l_w = x.edge,
                        support = 1,
                        GraphID = G.id
                    };
                    if (!extensions.Contains(f)) // do not add duplicate tupes
                    {
                        extensions.Add(f);
                    }
                }
            }
        }

        private void GenerateBackwardExtention(List<DFS_Code> C, Graph G, Node ur, List<Node> R, List<DFS_Code> extensions, Isomorphism o, Neighbor x)
        {
            Node node_v = new Node(); // node v is a mapping of node x in C
            node_v.id = -1;
            foreach (var kv in o.map)
            {
                if (kv.Value == x.id)
                {
                    node_v.id = kv.Key;
                    break;
                }
            }
            node_v.label = x.label;
            if (node_v.id != -1) // node v is existing in C
            {
                // rightmost path contains node v && edge (ur, v) is new in C                                
                if (R.Contains(node_v) && !checkEdge(ur.id, node_v.id, C))
                {
                    DFS_Code f = new DFS_Code()
                    {
                        u = ur.id,
                        v = node_v.id,
                        l_u = ur.label,
                        l_v = node_v.label,
                        l_w = x.edge,
                        support = 1,
                        GraphID = G.id
                    };
                    if (!extensions.Contains(f)) // do not add duplicate tupes
                    {
                        extensions.Add(f);
                    }
                }
            }
        }

        private static void HandleRightmostPathRootNode(Graph G, List<DFS_Code> extensions)
        {
            // add distinct label tuples in G as forward extensions
            foreach (DFS_Code dfs in G.edges)
            {
                DFS_Code f = new DFS_Code()
                {
                    u = 0,
                    v = 1,
                    l_u = dfs.l_u,
                    l_v = dfs.l_v,
                    l_w = dfs.l_w,
                    support = 1,
                    GraphID = G.id
                };
                if (!extensions.Contains(f)) // extensions do not contain f yet!
                {
                    extensions.Add(f);
                }

                // NEW CODE
                DFS_Code fReversed = new DFS_Code()
                {
                    u = 0,
                    v = 1,
                    l_u = dfs.l_v,
                    l_v = dfs.l_u,
                    l_w = dfs.l_w,
                    support = 1,
                    GraphID = G.id
                };
                if (!extensions.Contains(fReversed)) // extensions do not contain f yet!
                {
                    extensions.Add(fReversed);
                }
            }
        }

        private bool IsCanonical(List<DFS_Code> C, int support)
        {
            Graph GC = new Graph(C)
            {
                support = support
            };

            // graph corresponding to code C
            List<Graph> DC = new List<Graph> { GC };

            List<DFS_Code> C1 = new List<DFS_Code>(); // initialize canonical DFS code
            foreach (DFS_Code t in C)
            {
                List<DFS_Code> extensions = RightMostPath_Extensions(C1, DC); // extensions of C1
                // get least righmost edge extension of C1
                DFS_Code s = extensions[0];
                if (s.LessThan(t))
                {
                    return false; // C1 is smaller, thus C is not canonical
                }
                C1.Add(s);
            }
            return true; // no smaller code exists; C is canonical
        }

        private List<Node> getNodesOnRightMostPath(List<DFS_Code> C)
        {
            List<Node> nodes = new List<Node>(); // result
            List<Node> rmp = new List<Node>(); // rightmost path
            // create an empty rightmost path
            for (int i = 0; i < C.Count(); i++)
            {
                rmp.Add(new Node());
            }
            int rmp_len = 0; // rightmost path length
            foreach (DFS_Code dfs in C)
            {
                // consider only forward edges
                if (dfs.v > dfs.u && dfs.v > rmp[dfs.u].id)
                {
                    rmp[dfs.u].id = dfs.v;
                    rmp[dfs.u].label = dfs.l_v;
                    rmp_len = dfs.u;
                }
            }

            for (int i = rmp_len; i >= 0; i--)
            {
                nodes.Add(new Node() { id = rmp[i].id, label = rmp[i].label });
            }

            // add the first node
            nodes.Add(new Node() { id = C[0].u, label = C[0].l_u });

            return nodes;
        }

        private bool checkEdge(int x, int y, List<DFS_Code> C)
        {
            foreach (DFS_Code code in C)
            {
                if ((x == code.u && y == code.v) || (x == code.v && y == code.u))
                {
                    return true;
                }
            }

            return false;
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
