using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace DAL.IntegrationTests
{
    [TestClass]
    public class FrequentFeaturesFileDalTests
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
        public void Write_ShouldWriteAndReadBackCorrectly()
        {
            Dictionary<Graph, List<int>> features = new Dictionary<Graph, List<int>>();

            features.Add(new Graph(), new List<int>() { 1 });
            var filename = FrequentFeaturesFileDal.Write(features, "harta", 0.1);
            var featuresAfterLoad = FrequentFeaturesFileDal.Read(filename);

            Assert.AreEqual(ToAssertableString(featuresAfterLoad),ToAssertableString(features));
        }

        [TestMethod]
        public void Write_ShouldGenerateFileNameCorrectly()
        {
            Dictionary<Graph, List<int>> features = new Dictionary<Graph, List<int>>();

            features.Add(new Graph(), new List<int>() { 1 });
            var filename = FrequentFeaturesFileDal.Write(features, "harta.data", 0.1);

            Assert.IsTrue(filename.Contains(@"\harta_data__0_1__"));
        }

        public string ToAssertableString(Dictionary<Graph, List<int>> dictionary)
        {
            var pairStrings = dictionary.OrderBy(p => p.Key)
                .Select(p => p.Key + ": " + string.Join(", ", p.Value));
            return string.Join("; ", pairStrings);
        }
    }
}
