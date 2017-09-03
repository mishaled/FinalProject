using System.Collections.Generic;
using System.Linq;
using Common;

namespace Model
{
    public class Graph
    {
        public int id { get; set; }
        public int support { get; set; }
        public List<Node> nodes { get; set; }
        public List<DFS_Code> edges { get; set; }

        public Graph()
        {
            nodes = new List<Node>();
            edges = new List<DFS_Code>();
        }

        public Graph(List<DFS_Code> edges)
        {
            nodes = new List<Node>();
            this.edges = edges;

            foreach (DFS_Code edge in edges)
            {
                if (!DoesNodeExist(edge.u, edge.l_u))
                {
                    AddNode(edge.u,edge.l_u);
                }

                if (!DoesNodeExist(edge.v, edge.l_v))
                {
                    AddNode(edge.v, edge.l_v);
                }
            }
        }

        public Graph(int id)
        {
            nodes = new List<Node>();
            edges = new List<DFS_Code>();
            this.id = id;
        }

        public void AddNode(int nodeId, int label)
        {
            nodes.Add(new Node()
            {
                id = nodeId,
                label = label,
                graphId = id
            });
        }

        public bool DoesNodeExist(int nodeId, int label)
        {
            return nodes.Exists(x => x.id == nodeId && x.label == label);
        }

        public bool DoesEdgeExist(int u, int v, int label)
        {
            return edges.Exists(x => x.u == u && x.v == v && x.l_w == label);
        }

        public void AddEdge(int u, int v, int label)
        {
            edges.Add(new DFS_Code()
            {
                u = u,
                v = v,
                l_u = nodes.First(x => x.id == u).label,
                l_v = nodes.First(x => x.id == v).label,
                l_w = label,
                GraphID = id
            });
        }

        public int Size
        {
            get { return edges.Count; }
        }

        public override bool Equals(object obj)
        {
            var other = obj as Graph;

            if (other == null)
            {
                return false;
            }

            if (id != other.id)
            {
                return false;
            }

            if (!EnumerableComparer.CompareLists(nodes, other.nodes))
            {
                return false;
            }

            if (!EnumerableComparer.CompareLists(edges, other.edges))
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return id.GetHashCode() + support.GetHashCode() + nodes.GetHashCode() + edges.GetHashCode();
        }
    }
}
