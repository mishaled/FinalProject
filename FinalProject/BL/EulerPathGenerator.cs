using Model;
using System.Collections.Generic;
using System.Linq;

namespace BL
{
    public class EulerPathGenerator
    {
        public List<List<DFS_Code>> Generate(Graph graph)
        {
            return GenerateHelper(graph, graph.edges.ToList(), graph.nodes.First())
                .Where(x => x.Count == graph.Size)
                .Select(x => x.ToList()).ToList();
        }

        public bool DoesGraphContainEulerPath(Graph graph)
        {
            int numOfOddVertices = 0;

            foreach (var node in graph.nodes)
            {
                int adjacentEdgesCount = graph.edges.Count(x => x.u == node.id || x.v == node.id);

                if (adjacentEdgesCount % 2 == 0)
                {
                    continue;
                }

                numOfOddVertices++;
            }

            if (numOfOddVertices > 0 && numOfOddVertices != 2)
            {
                return false;
            }

            return true;
        }

        public List<LinkedList<DFS_Code>> GenerateHelper(Graph graph, List<DFS_Code> unusedEdges, Node currentNode)
        {
            List<DFS_Code> adjacentEdges = graph.edges.Where(x => x.u == currentNode.id || x.v == currentNode.id).ToList();
            List<DFS_Code> unusedAdjacentEdges = adjacentEdges.Where(unusedEdges.Contains).ToList();
            List<LinkedList<DFS_Code>> aggregatedPaths = new List<LinkedList<DFS_Code>>();

            foreach (DFS_Code edge in unusedAdjacentEdges)
            {
                Node nextNode = FindNextNode(graph, currentNode, edge);

                List<DFS_Code> unusedEdgesExcludingCurrentEdge = unusedEdges.Where(x => x != edge).ToList();
                List<LinkedList<DFS_Code>> lists = GenerateHelper(graph, unusedEdgesExcludingCurrentEdge, nextNode);

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
