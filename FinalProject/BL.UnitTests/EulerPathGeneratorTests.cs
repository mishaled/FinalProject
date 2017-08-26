using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace BL.UnitTests
{
    [TestClass]
    public class EulerPathGeneratorTests
    {
        [TestMethod]
        public void DoesGraphContainEulerPath_ExactlyTwoNodesHaveOddDegree_ShouldReturnTrue()
        {
            Graph graph = new Graph();

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

            EulerPathGenerator generator = new EulerPathGenerator();

            bool containsEulerPath = generator.DoesGraphContainEulerPath(graph);

            Assert.IsTrue(containsEulerPath);
        }

        [TestMethod]
        public void DoesGraphContainEulerPath_AllNodesHaveEvenDegree_ShouldReturnTrue()
        {
            Graph graph = new Graph();

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
            });

            graph.nodes.Add(new Node()
            {
                id = 4,
                label = 3,
            });

            graph.edges.Add(new DFS_Code()
            {
                u = 1,
                v = 2,
            });

            graph.edges.Add(new DFS_Code()
            {
                u = 2,
                v = 3,
            });

            graph.edges.Add(new DFS_Code()
            {
                u = 3,
                v = 4,
            });

            EulerPathGenerator generator = new EulerPathGenerator();

            bool containsEulerPath = generator.DoesGraphContainEulerPath(graph);

            Assert.IsTrue(containsEulerPath);
        }

        [TestMethod]
        public void DoesGraphContainEulerPath_ExactlyOneNodeHasOddDegree_ShouldReturnFalse()
        {
            Graph graph = new Graph();

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
            });

            graph.nodes.Add(new Node()
            {
                id = 4,
                label = 3,
            });

            graph.nodes.Add(new Node()
            {
                id = 5,
                label = 2,
            });

            graph.nodes.Add(new Node()
            {
                id = 6,
                label = 3,
            });

            graph.edges.Add(new DFS_Code()
            {
                u = 1,
                v = 2,
            });

            graph.edges.Add(new DFS_Code()
            {
                u = 2,
                v = 3,
            });

            graph.edges.Add(new DFS_Code()
            {
                u = 3,
                v = 4,
            });

            graph.edges.Add(new DFS_Code()
            {
                u = 3,
                v = 5,
            });

            graph.edges.Add(new DFS_Code()
            {
                u = 4,
                v = 6,
            });

            EulerPathGenerator generator = new EulerPathGenerator();

            bool containsEulerPath = generator.DoesGraphContainEulerPath(graph);

            Assert.IsFalse(containsEulerPath);
        }
    }
}
