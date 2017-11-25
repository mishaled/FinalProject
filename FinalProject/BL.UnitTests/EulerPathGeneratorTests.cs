using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using Common;
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
            Graph graph = MockGraphFactory.GenerateGraphWithOneEdge();

            EulerPathGenerator generator = new EulerPathGenerator();

            bool containsEulerPath = generator.DoesGraphContainEulerPath(graph);

            Assert.IsTrue(containsEulerPath);
        }

        [TestMethod]
        public void DoesGraphContainEulerPath_AllNodesHaveEvenDegree_ShouldReturnTrue()
        {
            Graph graph = MockGraphFactory.GenerateSquareGraph();

            EulerPathGenerator generator = new EulerPathGenerator();

            bool containsEulerPath = generator.DoesGraphContainEulerPath(graph);

            Assert.IsTrue(containsEulerPath);
        }

        [TestMethod]
        public void DoesGraphContainEulerPath_ExactlyOneNodeHasOddDegree_ShouldReturnFalse()
        {
            Graph graph = MockGraphFactory.GenerateSquareGraphWithTwoLines();

            EulerPathGenerator generator = new EulerPathGenerator();

            bool containsEulerPath = generator.DoesGraphContainEulerPath(graph);

            Assert.IsFalse(containsEulerPath);
        }

        [TestMethod]
        public void Generate_OneEdge_ShouldFindCorrectPath()
        {
            Graph graph = MockGraphFactory.GenerateGraphWithOneEdge();

            EulerPathGenerator generator = new EulerPathGenerator();

            List<List<DFS_Code>> actualLists = generator.Generate(graph);
            List<List<DFS_Code>> expectedList = new List<List<DFS_Code>>() { new List<DFS_Code>() { graph.edges.First() } };

            CollectionAssert.AreEqual(actualLists.First(), expectedList.First());
        }

        [TestMethod]
        public void Generate_TheGraphIsASqaure_ShouldReturnTrue()
        {
            Graph graph = MockGraphFactory.GenerateSquareGraph();

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
            Graph graph = MockGraphFactory.GenerateEulerianGraph();

            EulerPathGenerator generator = new EulerPathGenerator();

            List<List<DFS_Code>> actualLists = generator.Generate(graph);

            Assert.AreEqual("[2 0 2 3 0],[2 4 2 3 4],[4 5 4 5 5],[5 2 5 6 2],[3 2 3 2 2],[1 3 1 1 3],[0 1 0 0 1]", string.Join(",", actualLists[2]));
        }
    }
}
