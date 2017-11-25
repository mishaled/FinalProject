using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Castle.Components.DictionaryAdapter;
using Common;
using DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Moq;

namespace BL.UnitTests
{
    [TestClass]
    public class GIndexTests
    {
        private static List<Graph> graphsDb;

        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            Mock<INeo4jDAL> mockDal = new Mock<INeo4jDAL>();
            mockDal
                .Setup(x => x.GetGraphById(It.IsAny<int>()))
                .Returns((int id) =>
                {
                    return graphsDb.First(x => x.id == id);
                });
            DIFactory.Register(mockDal.Object);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            graphsDb = new List<Graph>();
        }

        [TestMethod]
        public void Fill_ShouldNotFail()
        {
            for (int i = 0; i < 5; i++)
            {
                graphsDb.Add(MockGraphFactory.GenerateSquareGraph());
            }

            GIndex gIndex = new GIndex(1);
            gIndex.Fill(graphsDb);

            var query = MockGraphFactory.GenerateSquareGraph();
            List<Graph> graphResults = gIndex.Search(query);

            CollectionAssert.Contains(graphResults, graphsDb.First());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Fill_NoMinSupSpecified_ShouldFail()
        {
            for (int i = 0; i < 5; i++)
            {
                graphsDb.Add(MockGraphFactory.GenerateSquareGraph());
            }

            GIndex gIndex = new GIndex();
            gIndex.Fill(graphsDb);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Fill_MinSupZero_ShouldFail()
        {
            for (int i = 0; i < 5; i++)
            {
                graphsDb.Add(MockGraphFactory.GenerateSquareGraph());
            }

            GIndex gIndex = new GIndex(0);
            gIndex.Fill(graphsDb);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Fill_MinSupNegative_ShouldFail()
        {
            for (int i = 0; i < 5; i++)
            {
                graphsDb.Add(MockGraphFactory.GenerateSquareGraph());
            }

            GIndex gIndex = new GIndex(-1);
            gIndex.Fill(graphsDb);
        }

        [TestMethod]
        public void Search_OneGraphMatchingQuery_ShouldReturnCorrectGraph()
        {
            var firstSquare = MockGraphFactory.GenerateSquareGraph();
            graphsDb.Add(firstSquare);

            FrequentFeatureSelector selector = new FrequentFeatureSelector();
            Dictionary<Graph, List<int>> features = selector.Select(graphsDb, 1);
            GIndex gIndex = new GIndex(1);
            gIndex.Fill(features);

            var query = MockGraphFactory.GenerateSquareGraph();
            List<Graph> graphResults = gIndex.Search(query);
            var resultIds = graphResults.Select(x => x.id).ToList();

            CollectionAssert.Contains(resultIds, firstSquare.id);
        }

        [TestMethod]
        public void Search_TwoDifferentGraphsMatchingQuery_ShouldReturnCorrectGraphs()
        {
            var firstSquare = MockGraphFactory.GenerateSquareGraph();
            graphsDb.Add(firstSquare);

            var squareGraphWithTwoLines = MockGraphFactory.GenerateSquareGraphWithTwoLines();
            graphsDb.Add(squareGraphWithTwoLines);

            FrequentFeatureSelector selector = new FrequentFeatureSelector();
            Dictionary<Graph, List<int>> features = selector.Select(graphsDb, 1);
            GIndex gIndex = new GIndex(1);
            gIndex.Fill(features);

            var query = MockGraphFactory.GenerateSquareGraph();
            List<Graph> graphResults = gIndex.Search(query);
            var resultIds = graphResults.Select(x => x.id).ToList();

            CollectionAssert.Contains(resultIds, firstSquare.id);
            CollectionAssert.Contains(resultIds, squareGraphWithTwoLines.id);
        }

        [TestMethod]
        public void Search_TwoSameGraphsMatchingQuery_ShouldReturnCorrectGraphs()
        {
            var firstSquare = MockGraphFactory.GenerateSquareGraph();
            graphsDb.Add(firstSquare);

            var secondSquare = MockGraphFactory.GenerateSquareGraph();
            graphsDb.Add(secondSquare);

            FrequentFeatureSelector selector = new FrequentFeatureSelector();
            Dictionary<Graph, List<int>> features = selector.Select(graphsDb, 1);
            GIndex gIndex = new GIndex(1);
            gIndex.Fill(features);

            var query = MockGraphFactory.GenerateSquareGraph();
            List<Graph> graphResults = gIndex.Search(query);
            var resultIds = graphResults.Select(x => x.id).ToList();

            CollectionAssert.Contains(resultIds, firstSquare.id);
            CollectionAssert.Contains(resultIds, secondSquare.id);
        }

        [TestMethod]
        public void Search_BunchOfDifferentGraphs_ShouldWorkCorrectly()
        {
            var firstSquare = MockGraphFactory.GenerateSquareGraph();
            graphsDb.Add(firstSquare);

            var secondSquare = MockGraphFactory.GenerateSquareGraph();
            graphsDb.Add(secondSquare);

            var eulerianGraph = MockGraphFactory.GenerateEulerianGraph();
            graphsDb.Add(eulerianGraph);

            var squareGraphWithTwoLines = MockGraphFactory.GenerateSquareGraphWithTwoLines();
            graphsDb.Add(squareGraphWithTwoLines);

            var oneEdge = MockGraphFactory.GenerateGraphWithOneEdge();
            graphsDb.Add(oneEdge);

            FrequentFeatureSelector selector = new FrequentFeatureSelector();
            Dictionary<Graph, List<int>> features = selector.Select(graphsDb, 1);
            GIndex gIndex = new GIndex();
            gIndex.Fill(features);

            var query = MockGraphFactory.GenerateSquareGraph();
            List<Graph> graphResults = gIndex.Search(query);
            var resultIds = graphResults.Select(x => x.id).ToList();

            CollectionAssert.Contains(resultIds, firstSquare.id);
            CollectionAssert.Contains(resultIds, secondSquare.id);
            CollectionAssert.Contains(resultIds, squareGraphWithTwoLines.id);
            CollectionAssert.DoesNotContain(resultIds, eulerianGraph.id);
            CollectionAssert.DoesNotContain(resultIds, oneEdge.id);
        }
    }
}
