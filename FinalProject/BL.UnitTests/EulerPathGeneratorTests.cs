using System;
using System.Collections.Generic;
using System.Linq;
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
            });

            graph.nodes.Add(new Node()
            {
                id = 2,
            });

            graph.nodes.Add(new Node()
            {
                id = 3,
            });

            graph.nodes.Add(new Node()
            {
                id = 4,
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
                u = 4,
                v = 1,
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

        [TestMethod]
        public void Generate_TwoEdges_ShouldFindCorrectPath()
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

            List<List<DFS_Code>> actualLists = generator.Generate(graph);
            List<List<DFS_Code>> expectedList = new List<List<DFS_Code>>() { new List<DFS_Code>() { graph.edges.First() } };

            CollectionAssert.AreEqual(actualLists.First(), expectedList.First());
        }

        [TestMethod]
        public void Generate_TheGraphIsASqaure_ShouldReturnTrue()
        {
            Graph graph = new Graph();

            graph.nodes.Add(new Node()
            {
                id = 1,
            });

            graph.nodes.Add(new Node()
            {
                id = 2,
            });

            graph.nodes.Add(new Node()
            {
                id = 3,
            });

            graph.nodes.Add(new Node()
            {
                id = 4,
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
                u = 4,
                v = 1,
            });

            EulerPathGenerator generator = new EulerPathGenerator();

            List<List<DFS_Code>> actualLists = generator.Generate(graph);
            List<List<DFS_Code>> expectedList = new List<List<DFS_Code>>()
            {
                graph.edges,
                graph.edges.OrderByDescending(x => x.u).ToList()
            };

            for (int i = 0; i < 2; i++)
            {
                CollectionAssert.AreEqual(actualLists[i], expectedList[i]);
            }
        }

        [TestMethod]
        public void Generate_EulerianGraph_ShouldReturnTrue()
        {
            Graph graph = new Graph();

            graph.nodes.Add(new Node()
            {
                id = 1,
            });

            graph.nodes.Add(new Node()
            {
                id = 2,
            });

            graph.nodes.Add(new Node()
            {
                id = 3,
            });

            graph.nodes.Add(new Node()
            {
                id = 4,
            });

            graph.nodes.Add(new Node()
            {
                id = 5,
            });

            graph.nodes.Add(new Node()
            {
                id = 6,
            });

            graph.edges.Add(new DFS_Code()
            {
                u = 1,
                v = 2,
            });

            graph.edges.Add(new DFS_Code()
            {
                u = 2,
                v = 4,
            });

            graph.edges.Add(new DFS_Code()
            {
                u = 4,
                v = 3,
            });

            graph.edges.Add(new DFS_Code()
            {
                u = 3,
                v = 1,
            });

            graph.edges.Add(new DFS_Code()
            {
                u = 3,
                v = 5,
            });

            graph.edges.Add(new DFS_Code()
            {
                u = 5,
                v = 6,
            });

            graph.edges.Add(new DFS_Code()
            {
                u = 6,
                v = 3,
            });

            EulerPathGenerator generator = new EulerPathGenerator();

            List<List<DFS_Code>> actualLists = generator.Generate(graph);

            Assert.AreEqual(string.Join(",", actualLists[2]), "[1 3 0],[5 3 0],[6 5 0],[3 6 0],[3 4 0],[4 2 0],[2 1 0]");
        }
    }
}
