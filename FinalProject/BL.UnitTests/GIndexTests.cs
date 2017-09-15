using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace BL.UnitTests
{
    [TestClass]
    public class GIndexTests
    {
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestMethod]
        public void Fill_ShouldNotFail()
        {
            List<Graph> graphsDb = new List<Graph>();

            for (int i = 0; i < 5; i++)
            {
                graphsDb.Add(MockGraphFactory.GenerateSquareGraph());
            }

            GIndex gIndex = new GIndex(0);
            gIndex.Fill(graphsDb);

            var query = MockGraphFactory.GenerateSquareGraph();
            List<Graph> graphResults = gIndex.Search(query, graphsDb);

            CollectionAssert.Contains(graphResults, graphsDb.First());
        }
    }
}
