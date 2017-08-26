using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class GraphPathsGenerator
    {
        public List<List<DFS_Code>> Generate(Graph graph)
        {
            EulerPathGenerator eulerPathGenerator = new EulerPathGenerator();

            if (eulerPathGenerator.DoesGraphContainEulerPath(graph))
            {
                return eulerPathGenerator.Generate(graph);
            }

            return GenerateAllPathsUpToSizeOfGraph(graph).Select(x => x.ToList()).ToList();
        }

        private List<List<DFS_Code>> GenerateAllPathsUpToSizeOfGraph(Graph graph)
        {
            List<List<DFS_Code>> lists = new List<List<DFS_Code>>();
            foreach (var node in graph.nodes)
            {
                lists.AddRange(GenerateAllPathsUpToLength(graph, node, graph.Size));
            }

            return lists;
        }

        private List<List<DFS_Code>> GenerateAllPathsUpToLength(Graph graph, Node startNode, int size)
        {
            var lists = new List<List<DFS_Code>>();
            for (int i = 0; i < size; i++)
            {
                lists.AddRange(PathFinder.FindAndCast(graph, graph.edges, startNode, i));
            }

            return lists;
        }

        //private List<LinkedList<DFS_Code>> GenerateAllPathsOfLength(Graph graph, Node currentNode, int size)
        //{
        //    List<LinkedList<DFS_Code>> aggregatedPaths = new List<LinkedList<DFS_Code>>();
        //    if (size <= 0)
        //    {
        //        return new List<LinkedList<DFS_Code>>();
        //    }

        //    List<DFS_Code> adjacentEdges = graph.edges.Where(x => x.u == currentNode.id || x.v == currentNode.id).ToList();

        //    foreach (DFS_Code edge in adjacentEdges)
        //    {
        //        Node nextNode = FindNextNode(graph, currentNode, edge);

        //        List<LinkedList<DFS_Code>> lists = GenerateAllPathsUpToLength(graph, nextNode, size - 1);
        //        lists.ForEach(x => x.AddFirst(edge));
        //        return lists;
        //    }

        //    return new List<LinkedList<DFS_Code>>();
        //}

        //public List<LinkedList<DFS_Code>> GenerateHelper(Graph graph, List<DFS_Code> unusedEdges, Node currentNode, int size)
        //{
        //    if (size <= 0)
        //    {
        //        return new List<LinkedList<DFS_Code>>();
        //    }

        //    List<DFS_Code> adjacentEdges = graph.edges.Where(x => x.u == currentNode.id || x.v == currentNode.id).ToList();
        //    List<DFS_Code> unusedAdjacentEdges = adjacentEdges.Where(unusedEdges.Contains).ToList();
        //    List<LinkedList<DFS_Code>> aggregatedPaths = new List<LinkedList<DFS_Code>>();

        //    foreach (DFS_Code edge in unusedAdjacentEdges)
        //    {
        //        Node nextNode = FindNextNode(graph, currentNode, edge);

        //        List<DFS_Code> unusedEdgesExcludingCurrentEdge = unusedEdges.Where(x => x != edge).ToList();
        //        List<LinkedList<DFS_Code>> lists = GenerateHelper(graph, unusedEdgesExcludingCurrentEdge, nextNode, size - 1);

        //        lists.ForEach(x => x.AddFirst(edge));

        //        if (!lists.Any())
        //        {
        //            LinkedList<DFS_Code> list = new LinkedList<DFS_Code>();
        //            list.AddFirst(edge);
        //            lists.Add(list);
        //        }

        //        aggregatedPaths.AddRange(lists);
        //    }

        //    return aggregatedPaths;
        //}

        //private static Node FindNextNode(Graph graph, Node currentNode, DFS_Code edge)
        //{
        //    Node nextNode = null;

        //    if (edge.u == currentNode.id)
        //    {
        //        nextNode = graph.nodes.First(x => x.id == edge.v);
        //    }
        //    else if (edge.v == currentNode.id)
        //    {
        //        nextNode = graph.nodes.First(x => x.id == edge.u);
        //    }
        //    return nextNode;
        //}
    }
}
