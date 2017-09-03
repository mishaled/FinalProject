using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace BL.UnitTests
{
    [TestClass]
    public class FrequentFeatureSelectorTests
    {
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestMethod]
        public void Select_ShouldGenerateCorrectNumberOfFeatures()
        {
            string currentDir = System.AppDomain.CurrentDomain.BaseDirectory;
            string filename = string.Format("{0}Resources\\TestDataSet.data", Path.GetFullPath(Path.Combine(currentDir, @"..\..\")));

            SyntheticGraphDatabaseReader reader = new SyntheticGraphDatabaseReader();
            var graphs = reader.Read(filename);

            FrequentFeatureSelector selector = new FrequentFeatureSelector();
            List<Graph> ff = selector.Select(graphs, 0.5);

            Assert.AreEqual(27, ff.Count);
        }
    }
}
