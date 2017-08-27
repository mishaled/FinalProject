using Model;

namespace Common
{
    public static class MockGraphFactory
    {
        public static Graph GenerateGraphWithOneEdge(int graphId = 0)
        {
            Graph graph = new Graph()
            {
                id = graphId
            };

            graph.nodes.Add(new Node()
            {
                id = 1,
                label = 2,
                graphId = graph.id
            });

            graph.nodes.Add(new Node()
            {
                id = 2,
                label = 3,
                graphId = graph.id
            });

            graph.edges.Add(new DFS_Code()
            {
                u = 1,
                v = 2,
                l_u = 2,
                l_v = 3,
                l_w = 4,
                GraphID = graph.id
            });

            return graph;
        }

        public static Graph GenerateSquareGraph(int graphId = 0)
        {
            Graph graph = new Graph(graphId);

            graph.nodes.Add(new Node()
            {
                id = 1,
                graphId = graphId
            });

            graph.nodes.Add(new Node()
            {
                id = 2,
                graphId = graphId
            });

            graph.nodes.Add(new Node()
            {
                id = 3,
                graphId = graphId
            });

            graph.nodes.Add(new Node()
            {
                id = 4,
                graphId = graphId
            });

            graph.edges.Add(new DFS_Code()
            {
                u = 1,
                v = 2,
                GraphID = graphId
            });

            graph.edges.Add(new DFS_Code()
            {
                u = 2,
                v = 3,
                GraphID = graphId
            });

            graph.edges.Add(new DFS_Code()
            {
                u = 3,
                v = 4,
                GraphID = graphId
            });

            graph.edges.Add(new DFS_Code()
            {
                u = 4,
                v = 1,
                GraphID = graphId
            });

            return graph;
        }

        public static Graph GenerateSquareGraphWithTwoLines(int graphId = 0)
        {
            Graph graph = new Graph(graphId);

            graph.nodes.Add(new Node()
            {
                id = 1,
                label = 2,
                graphId = graph.id
            });

            graph.nodes.Add(new Node()
            {
                id = 2,
                label = 3,
                graphId = graph.id
            });

            graph.edges.Add(new DFS_Code()
            {
                u = 1,
                v = 2,
                l_u = 2,
                l_v = 3,
                l_w = 4,
                GraphID = graph.id
            });

            graph.nodes.Add(new Node()
            {
                id = 3,
                label = 2,
                graphId = graphId
            });

            graph.nodes.Add(new Node()
            {
                id = 4,
                label = 3,
                graphId = graphId
            });

            graph.nodes.Add(new Node()
            {
                id = 5,
                label = 2,
                graphId = graphId
            });

            graph.nodes.Add(new Node()
            {
                id = 6,
                label = 3,
                graphId = graphId
            });

            graph.edges.Add(new DFS_Code()
            {
                u = 1,
                v = 2,
                GraphID = graphId
            });

            graph.edges.Add(new DFS_Code()
            {
                u = 2,
                v = 3,
                GraphID = graphId
            });

            graph.edges.Add(new DFS_Code()
            {
                u = 3,
                v = 4,
                GraphID = graphId
            });

            graph.edges.Add(new DFS_Code()
            {
                u = 3,
                v = 5,
                GraphID = graphId
            });

            graph.edges.Add(new DFS_Code()
            {
                u = 4,
                v = 6,
                GraphID = graphId
            });

            return graph;
        }

        public static Graph GenerateEulerianGraph(int graphId = 0)
        {
            Graph graph = new Graph(graphId);

            graph.nodes.Add(new Node()
            {
                id = 1,
                graphId = graphId
            });

            graph.nodes.Add(new Node()
            {
                id = 2,
                graphId = graphId
            });

            graph.nodes.Add(new Node()
            {
                id = 3,
                graphId = graphId
            });

            graph.nodes.Add(new Node()
            {
                id = 4,
                graphId = graphId
            });

            graph.nodes.Add(new Node()
            {
                id = 5,
                graphId = graphId
            });

            graph.nodes.Add(new Node()
            {
                id = 6,
                graphId = graphId
            });

            graph.edges.Add(new DFS_Code()
            {
                u = 1,
                v = 2,
                GraphID = graphId
            });

            graph.edges.Add(new DFS_Code()
            {
                u = 2,
                v = 4,
                GraphID = graphId
            });

            graph.edges.Add(new DFS_Code()
            {
                u = 4,
                v = 3,
                GraphID = graphId
            });

            graph.edges.Add(new DFS_Code()
            {
                u = 3,
                v = 1,
                GraphID = graphId
            });

            graph.edges.Add(new DFS_Code()
            {
                u = 3,
                v = 5,
                GraphID = graphId
            });

            graph.edges.Add(new DFS_Code()
            {
                u = 5,
                v = 6,
                GraphID = graphId
            });

            graph.edges.Add(new DFS_Code()
            {
                u = 6,
                v = 3,
                GraphID = graphId
            });

            return graph;
        }
    }
}
