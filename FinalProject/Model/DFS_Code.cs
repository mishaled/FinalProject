using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class DFS_Code : IComparable
    {
        public int u { get; set; } // vertex u
        public int v { get; set; } // vertex v
        public int l_u { get; set; } // label of vertex u
        public int l_v { get; set; } // label of vertex v
        public int l_w { get; set; } // label of edge uv
        public int support { get; set; } // support of this DFS code (edge)
        public int GraphID { get; set; } // graph contains this DFS code (edge)

        public DFS_Code()
        {
            support = 1;
        }

        public DFS_Code(DFS_Code other)
        {
            u = other.u;
            v = other.v;
            l_u = other.l_u;
            l_v = other.l_v;
            l_w = other.l_w;
            support = other.support;
            GraphID = other.GraphID;
        }

        public override bool Equals(object obj)
        {
            DFS_Code other = obj as DFS_Code;

            if (other == null)
            {
                return false;
            }

            if (this.u != other.u)
            {
                return false;
            }
            if (this.v != other.v)
            {
                return false;
            }

            if (this.l_u != other.l_u)
            {
                return false;
            }

            if (this.l_v != other.l_v)
            {
                return false;
            }

            if (this.l_w != other.l_w)
            {
                return false;
            }

            if (this.GraphID != other.GraphID)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return u.GetHashCode() + v.GetHashCode() + l_u.GetHashCode() + l_v.GetHashCode() + l_w.GetHashCode() +
                   GraphID.GetHashCode() + support.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            if (this.LessThan((DFS_Code)obj))
            {
                return -1;
            }

            if (this.Equals(obj))
            {
                return 0;
            }

            return 1;
        }

        public bool LessThan(DFS_Code other)
        {
            // compare labels of two edges
            if (this.u == other.u && this.v == other.v)
            {
                if (this.l_u < other.l_u)
                {
                    return true;
                }
                else if (this.l_u == other.l_u)
                {
                    if (this.l_v < other.l_v)
                    {
                        return true;
                    }
                    else if (this.l_v == other.l_v)
                    {
                        if (this.l_w < other.l_w)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            // compare two edges
            // condition 1: both edges are forward
            if (this.u < this.v && other.u < other.v)
            {
                if (this.v < other.v || (this.v == other.v && this.u > other.u))
                {
                    return true;
                }
            }

            // condition 2: both edges are backward
            if (this.u > this.v && other.u > other.v)
            {
                if (this.u < other.u || (this.u == other.u && this.v < other.v))
                {
                    return true;
                }
            }

            // condition 3: this edge is forward and the other is backward
            if (this.u < this.v && other.u > other.v)
            {
                if (this.v <= other.u)
                {
                    return true;
                }
            }

            // condition 4: this edge is backward and the other is forward
            if (this.u > this.v && other.u < other.v)
            {
                if (this.u < other.v)
                {
                    return true;
                }
            }

            return false;
        }

        public override string ToString()
        {
            return string.Format("[{0} {1} {2} {3} {4}]", u, v, l_u, l_w, l_v);
        }
    }
}
