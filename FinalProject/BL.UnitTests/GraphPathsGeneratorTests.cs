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
        [TestMethod]
        public void Generate_EulerianPathExists_ShouldReturnEulerianPaths()
        {
            Graph graph = MockGraphFactory.GenerateEulerianGraph();

            GraphPathsGenerator generator = new GraphPathsGenerator();

            List<List<DFS_Code>> actualLists = generator.Generate(graph);

            Assert.AreEqual(actualLists.Count, 4);
            Assert.AreEqual(string.Join(",", actualLists[2]), "[1 3 0],[5 3 0],[6 5 0],[3 6 0],[3 4 0],[4 2 0],[2 1 0]");
        }

        [TestMethod]
        public void Generate_EulerianPathDoesNotExist_ShouldReturnAllPaths()
        {
            Graph graph = MockGraphFactory.GenerateSquareGraphWithTwoLines();

            GraphPathsGenerator generator = new GraphPathsGenerator();

            List<List<DFS_Code>> actualLists = generator.Generate(graph);

            Assert.AreEqual(actualLists.Count, 60);
        }
    }
}
