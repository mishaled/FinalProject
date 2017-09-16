using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using DAL.IntegrationTests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace DAL.IntegrationTests
{
    [TestClass]
    public class SyntheticGraphDatabaseWriterTests
    {
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestMethod]
        public void Write_ShouldWriteCorrectGraph()
        {
            string currentDir = System.AppDomain.CurrentDomain.BaseDirectory;
            string filename = string.Format("{0}Resources\\TestDataSet.data", Path.GetFullPath(Path.Combine(currentDir, @"..\..\")));

            SyntheticGraphDatabaseReader reader = new SyntheticGraphDatabaseReader();
            List<Graph> orignalGraphDb = reader.Read(filename);

            SyntheticGraphDatabaseWriter writer = new SyntheticGraphDatabaseWriter();
            string tempFile = Guid.NewGuid().ToString();
            writer.Write(tempFile, orignalGraphDb);

            List<Graph> graphDbAfterWrite = reader.Read(tempFile);

            CollectionAssert.AreEqual(orignalGraphDb, graphDbAfterWrite);

        }
    }
}
