using System;
using System.Collections.Generic;
using System.Linq;
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

            GraphPathsGenerator generator = new GraphPathsGenerator();

            List<List<DFS_Code>> actualLists = generator.Generate(graph);

            Assert.AreEqual(actualLists.Count, 4);
            Assert.AreEqual(string.Join(",", actualLists[2]), "[1 3 0],[5 3 0],[6 5 0],[3 6 0],[3 4 0],[4 2 0],[2 1 0]");
        }

        [TestMethod]
        public void Generate_EulerianPathDoesNotExist_ShouldReturnAllPaths()
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

            GraphPathsGenerator generator = new GraphPathsGenerator();

            List<List<DFS_Code>> actualLists = generator.Generate(graph);

            Assert.AreEqual(actualLists.Count, 72);
        }
    }
}
