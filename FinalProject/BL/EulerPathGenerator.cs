using Model;
using System.Collections.Generic;
using System.Linq;

namespace BL
{
    public class EulerPathGenerator
    {
        public List<List<DFS_Code>> Generate(Graph graph)
        {
            return GenerateHelper(graph, graph.edges.ToList(), graph.nodes.First());
        }

        public bool DoesGraphContainEulerPath(Graph graph)
        {
            int numOfOddVertices = 0;

            foreach (var node in graph.nodes)
            {
                var adjacentEdgesCount = graph.edges.Count(x => x.u == node.id || x.v == node.id);

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

        public List<List<DFS_Code>> GenerateHelper(Graph graph, List<DFS_Code> unusedEdges, Node currentNode)
        {
            var adjacentEdges = graph.edges.Where(x => x.u == currentNode.id);
            var unusedAdjacentEdges = adjacentEdges.Where(x => unusedEdges.Contains(x));

            foreach (var edge in unusedAdjacentEdges)
            {
                var list = new List<DFS_Code>();
                list.Add(edge);
                var nextNode = graph.nodes.First(x => x.id == edge.v);

                var lists = GenerateHelper(graph, unusedEdges.Where(x => x != edge).ToList(), nextNode);
                lists.ForEach(x => x.Add(edge));

                return lists;
            }

            return new List<List<DFS_Code>>();
        }
    }
}
