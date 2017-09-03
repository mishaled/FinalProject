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
        [Ignore]
        public void Select_WholeSyntheticDb_ShouldGenerateCorrectNumberOfFeatures()
        {
            string currentDir = System.AppDomain.CurrentDomain.BaseDirectory;
            string filename = string.Format("{0}Resources\\TestDataSet.data", Path.GetFullPath(Path.Combine(currentDir, @"..\..\")));

            SyntheticGraphDatabaseReader reader = new SyntheticGraphDatabaseReader();
            var graphs = reader.Read(filename);

            FrequentFeatureSelector selector = new FrequentFeatureSelector();
            List<Graph> ff = selector.Select(graphs, 0.5);

            Assert.AreEqual(31, ff.Count);
        }

        [TestMethod]
        public void Select_PartOfSyntheticDb_ShouldGenerateCorrectNumberOfFeatures()
        {
            string currentDir = System.AppDomain.CurrentDomain.BaseDirectory;
            string filename = string.Format("{0}Resources\\TestDataSet.data", Path.GetFullPath(Path.Combine(currentDir, @"..\..\")));

            SyntheticGraphDatabaseReader reader = new SyntheticGraphDatabaseReader();
            var graphs = reader.Read(filename,100);

            FrequentFeatureSelector selector = new FrequentFeatureSelector();
            List<Graph> ff = selector.Select(graphs, 0.5);

            Assert.AreEqual(25, ff.Count);
        }
    }
}
