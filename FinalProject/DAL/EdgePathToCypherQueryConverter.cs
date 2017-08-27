using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace DAL
{
    public class EdgePathToCypherQueryConverter
    {
        private int nextId;
        private int nextLabel;

        public string Convert(List<DFS_Code> edges)
        {
            string query = string.Empty;

            if (edges.Count == 0)
            {
                return query;
            }

            for (var index = 0; index < edges.Count; index++)
            {
                DFS_Code edge = edges[index];
                Tuple<int, int, int, int> order = ComputeEdgeOrder(edges, index, edge);

                if (index == 0)
                {
                    query += string.Format("match(n{0}:Node {{ id: {1}, label: {2}}})",index, order.Item1, order.Item2);
                }

                query += string.Format("-[:CONNECTED_TO {{label: {0} }}]-(n{1} {{id: {2}, label: {3}}})", edge.l_w, index + 1, order.Item3, order.Item4);
            }

            query += " return DISTINCT n0.graphId as graphId";

            return query;
        }

        private Tuple<int, int, int, int> ComputeEdgeOrder(List<DFS_Code> edges, int index, DFS_Code edge)
        {
            Tuple<int, int, int, int> order = null;
            if (index + 1 < edges.Count)
            {
                order = ComputeEdgeOrder(edge, edges[index + 1]);
            }
            else if (index > 0)
            {
                Tuple<int, int, int, int> reversed = ComputeEdgeOrder(edges[index], edges[index - 1]);
                order = Tuple.Create(reversed.Item3, reversed.Item4, reversed.Item1, reversed.Item2);
            }
            else
            {
                order = Tuple.Create(edge.u, edge.l_u, edge.v, edge.l_v);
            }
            return order;
        }

        private Tuple<int, int, int, int> ComputeEdgeOrder(DFS_Code firstEdge, DFS_Code secondEdge)
        {
            if (firstEdge.u == secondEdge.u || firstEdge.u == secondEdge.v)
            {
                return new Tuple<int, int, int, int>(firstEdge.v, firstEdge.l_v, firstEdge.u, firstEdge.l_u);
            }

            return new Tuple<int, int, int, int>(firstEdge.u, firstEdge.l_u, firstEdge.v, firstEdge.l_v);
        }
    }
}
