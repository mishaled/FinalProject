using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace BL.UnitTests
{
    [TestClass]
    public class GraphPathsGeneratorTests
    {
        //[TestMethod]
        //public void Generate_EulerianPathExists_ShouldReturnEulerianPaths()
        //{
        //    Graph graph = MockGraphFactory.GenerateEulerianGraph();

        //    GraphPathsGenerator generator = new GraphPathsGenerator();

        //    List<List<DFS_Code>> actualLists = generator.Generate(graph);

        //    Assert.AreEqual(actualLists.Count, 4);
        //    Assert.AreEqual("[0 2 3],[4 2 3],[5 4 5],[2 5 6],[2 3 2],[3 1 1],[1 0 0]", string.Join(",", actualLists[2]));
        //}

        [TestMethod]
        public void Generate_OneEdgeGRaph_ShouldReturnCorrectPath()
        {
            Graph graph = MockGraphFactory.GenerateGraphWithOneEdge();

            GraphPathsGenerator generator = new GraphPathsGenerator();

            List<List<DFS_Code>> actualLists = generator.Generate(graph);

            Assert.AreEqual(actualLists.Count, 2);
            Assert.AreEqual("[2 1 4]", string.Join(",", actualLists[0]));
        }

        [TestMethod]
        public void Generate_EulerianPathDoesNotExist_ShouldReturnAllPaths()
        {
            Graph graph = MockGraphFactory.GenerateSquareGraphWithTwoLines();

            GraphPathsGenerator generator = new GraphPathsGenerator();

            List<List<DFS_Code>> actualLists = generator.Generate(graph);

            Assert.AreEqual(85, actualLists.Count);
        }
    }
}
