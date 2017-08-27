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

        public Graph(int id)
        {
            nodes = new List<Node>();
            edges = new List<DFS_Code>();
            this.id = id;
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
