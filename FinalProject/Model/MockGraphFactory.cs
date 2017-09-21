using System;
using System.Threading;

namespace Model
{
    public static class MockGraphFactory
    {
        public static Graph GenerateGraphWithOneEdge(int? graphId = null)
        {
            Graph graph = generateGraphWithIdIfNeeded(graphId);

            graph.AddNode(0, 1);
            graph.AddNode(1, 2);
            graph.AddEdge(0, 1, 4);

            return graph;
        }

        public static Graph GenerateSquareGraph(int? graphId = null)
        {
            Graph graph = generateGraphWithIdIfNeeded(graphId);

            graph.AddNode(0, 0);
            graph.AddNode(1, 1);
            graph.AddNode(2, 2);
            graph.AddNode(3, 3);

            graph.AddEdge(0, 1, 0);
            graph.AddEdge(1, 2, 1);
            graph.AddEdge(2, 3, 2);
            graph.AddEdge(3, 0, 3);

            return graph;
        }

        public static Graph GenerateSquareGraphWithTwoLines(int? graphId = null)
        {
            Graph graph = GenerateSquareGraph();

            graph.AddNode(3, 3);
            graph.AddNode(4, 4);
            graph.AddNode(5, 5);

            graph.AddEdge(2, 4, 0);
            graph.AddEdge(3, 5, 0);

            return graph;
        }

        public static Graph GenerateEulerianGraph(int? graphId = null)
        {
            Graph graph = generateGraphWithIdIfNeeded(graphId);

            graph.AddNode(0, 0);
            graph.AddNode(1, 1);
            graph.AddNode(2, 2);
            graph.AddNode(3, 3);
            graph.AddNode(4, 4);
            graph.AddNode(5, 5);

            graph.AddEdge(0, 1, 0);
            graph.AddEdge(1, 3, 1);
            graph.AddEdge(3, 2, 2);
            graph.AddEdge(2, 0, 3);
            graph.AddEdge(2, 4, 3);
            graph.AddEdge(4, 5, 5);
            graph.AddEdge(5, 2, 6);

            return graph;
        }

        private static Graph generateGraphWithIdIfNeeded(int? graphId)
        {
            if (graphId != null)
            {
                return new Graph(graphId.Value);
            }

            Thread.Sleep(1000);

            Random rand = new Random();
            int randomID = 1000000;
            randomID += rand.Next(1000000);

            return new Graph(randomID);
        }
    }
}
