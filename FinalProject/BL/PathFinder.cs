using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace BL
{
    internal static class PathFinder
    {
        public static List<List<DFS_Code>> FindAndCast(Graph graph, List<DFS_Code> unusedEdges, Node currentNode, int size)
        {
            return Find(graph, unusedEdges, currentNode, size)
                .Where(x => x.Count == size)
                .Select(x => x.ToList()).ToList();
        }
        private static List<LinkedList<DFS_Code>> Find(Graph graph, List<DFS_Code> unusedEdges, Node currentNode, int size)
        {
            if (size <= 0)
            {
                return new List<LinkedList<DFS_Code>>();
            }

            List<DFS_Code> adjacentEdges = graph.edges.Where(x => x.u == currentNode.id || x.v == currentNode.id).ToList();
            List<DFS_Code> unusedAdjacentEdges = adjacentEdges.Where(unusedEdges.Contains).ToList();
            List<LinkedList<DFS_Code>> aggregatedPaths = new List<LinkedList<DFS_Code>>();

            foreach (DFS_Code edge in unusedAdjacentEdges)
            {
                Node nextNode = FindNextNode(graph, currentNode, edge);

                List<DFS_Code> unusedEdgesExcludingCurrentEdge = unusedEdges.Where(x => x != edge).ToList();
                List<LinkedList<DFS_Code>> lists = Find(graph, unusedEdgesExcludingCurrentEdge, nextNode, size - 1);

                lists.ForEach(x => x.AddFirst(edge));

                if (!lists.Any())
                {
                    LinkedList<DFS_Code> list = new LinkedList<DFS_Code>();
                    list.AddFirst(edge);
                    lists.Add(list);
                }

                aggregatedPaths.AddRange(lists);
            }

            return aggregatedPaths;
        }

        private static Node FindNextNode(Graph graph, Node currentNode, DFS_Code edge)
        {
            Node nextNode = null;

            if (edge.u == currentNode.id)
            {
                nextNode = graph.nodes.First(x => x.id == edge.v);
            }
            else if (edge.v == currentNode.id)
            {
                nextNode = graph.nodes.First(x => x.id == edge.u);
            }
            return nextNode;
        }
    }
}
