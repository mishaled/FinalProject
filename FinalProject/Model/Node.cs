using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class Node
    {
        public int id { get; set; }
        public int label { get; set; }
        public int graphId { get; set; } // graph contains this DFS code (edge)

        public Node(Node other = null)
        {
            if (other == null)
            {
                return;
            }

            id = other.id;
            label = other.label;
            graphId = other.graphId;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Node;

            if (other == null)
            {
                return false;
            }

            if (this.id == other.id && this.label == other.label)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return id.GetHashCode() + label.GetHashCode();
        }
    }
}
