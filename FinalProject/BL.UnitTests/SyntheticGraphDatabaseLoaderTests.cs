using System;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Common;
using DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace BL.UnitTests
{
    [TestClass]
    public class SyntheticGraphDatabaseLoaderTests
    {
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            string neo4jUrl = ConfigurationManager.AppSettings.Get("Neo4jUrl");
            string neo4jUsername = ConfigurationManager.AppSettings.Get("Neo4jUsername");
            string neo4jPassword = ConfigurationManager.AppSettings.Get("Neo4jPassword");

            Neo4jDAL dal = new Neo4jDAL(neo4jUrl, neo4jUsername, neo4jPassword);
            DIFactory.Register<INeo4jDAL>(dal);
        }

        [TestMethod]
        public void Load_ShouldWriteCorrectGraphsAndReadThemBack()
        {
            string currentDir = System.AppDomain.CurrentDomain.BaseDirectory;
            string filename = string.Format("{0}Resources\\TestDataSet.data", Path.GetFullPath(Path.Combine(currentDir, @"..\..\")));

            SyntheticGraphDatabaseLoader loader = new SyntheticGraphDatabaseLoader(filename);
            var graphs = loader.Load();

            INeo4jDAL dal = DIFactory.Resolve<INeo4jDAL>();
            foreach (var originalGraph in graphs)
            {
                Graph actualGraph = dal.GetGraphById(originalGraph.id);
                Assert.AreEqual(originalGraph, actualGraph);
            }
        }
    }
}
