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

            return GenerateAllPathsUpToSizeOfGraph(graph);
        }

        private List<List<DFS_Code>> GenerateAllPathsUpToSizeOfGraph(Graph graph)
        {
            var lists = new List<List<DFS_Code>>();
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
                lists.AddRange(GenerateAllPathsOfLength(graph, graph.nodes.First(), i));
            }

            return lists;
        }

        private List<List<DFS_Code>> GenerateAllPathsOfLength(Graph graph, Node startNode, int size)//, List<List<DFS_Code>> lists)
        {
            if (size <= 0)
            {
                return new List<List<DFS_Code>>();
            }

            IEnumerable<DFS_Code> adjacentEdges = graph.edges.Where(x => x.u == startNode.id);
            //|| x.v == startNode.id);

            foreach (DFS_Code edge in adjacentEdges)
            {
                Node nextNode = graph.nodes.First(x => x.id == edge.v);

                var lists = GenerateAllPathsUpToLength(graph, nextNode, size - 1);
                lists.ForEach(x => x.Add(edge));
                return lists;
            }

            return new List<List<DFS_Code>>();
        }
    }
}
