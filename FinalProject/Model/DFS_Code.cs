using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class DFS_Code
    {
        public int u { get; set; } // vertex u
        public int v { get; set; } // vertex v
        public int l_u { get; set; } // label of vertex u
        public int l_v { get; set; } // label of vertex v
        public int l_w { get; set; } // label of edge uv
        public int support { get; set; } // support of this DFS code (edge)
        public int GraphID { get; set; } // graph contains this DFS code (edge)
        //public int u_dbId { get; set; }
        //public int v_dbId { get; set; }

        public DFS_Code()
        {
            //u_dbId = -1;
            //v_dbId = -1;
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
        //public bool Equals(DFS_Code other)
        //{
        //    if (this.u != other.u)
        //    {
        //        return false;
        //    }
        //    if (this.v != other.v)
        //    {
        //        return false;
        //    }

        //    if (this.l_u != other.l_u)
        //    {
        //        return false;
        //    }

        //    if (this.l_v != other.l_v)
        //    {
        //        return false;
        //    }

        //    if (this.l_w != other.l_w)
        //    {
        //        return false;
        //    }

        //    if (this.GraphID != other.GraphID)
        //    {
        //        return false;
        //    }

        //    //if (this.u_dbId >= 0 && other.u_dbId >= 0 && this.u_dbId != other.u_dbId)
        //    //{
        //    //    return false;
        //    //}

        //    //if (this.v_dbId >= 0 && other.v_dbId >= 0 && this.v_dbId != other.v_dbId)
        //    //{
        //    //    return false;
        //    //}

        //    return true;
        //}

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
            return string.Format("[{0} {1} {2}]", v, u, l_w);
        }
    }
}
