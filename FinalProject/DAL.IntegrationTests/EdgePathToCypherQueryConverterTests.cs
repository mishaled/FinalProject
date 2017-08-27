using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace DAL.IntegrationTests
{
    [TestClass]
    public class EdgePathToCypherQueryConverterTests
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
        public void Convert_OneEdge_ShouldConvertCorrectly()
        {
            List<DFS_Code> path = new List<DFS_Code>();
            path.Add(new DFS_Code()
            {
                u = 1,
                l_u = 1,
                v = 2,
                l_v = 2,
                l_w = 1
            });

            var converter = new EdgePathToCypherQueryConverter();
            var query = converter.Convert(path);

            Assert.AreEqual(query, "match(n0:Node { id: 1, label: 1})-[:CONNECTED_TO {label: 1 }]-(n1 {id: 2, label: 2}) return DISTINCT n0.graphId as graphId");
        }

        [TestMethod]
        public void Convert_TwoEdgesInCorrectOrder_ShouldConvertCorrectly()
        {
            List<DFS_Code> path = new List<DFS_Code>();
            path.Add(new DFS_Code()
            {
                u = 1,
                l_u = 1,
                v = 2,
                l_v = 2,
                l_w = 1
            });

            path.Add(new DFS_Code()
            {
                u = 2,
                l_u = 2,
                v = 3,
                l_v = 3,
                l_w = 2
            });

            var converter = new EdgePathToCypherQueryConverter();
            var query = converter.Convert(path);

            Assert.AreEqual(query, "match(n0:Node { id: 1, label: 1})-[:CONNECTED_TO {label: 1 }]-(n1 {id: 2, label: 2})-[:CONNECTED_TO {label: 2 }]-(n2 {id: 3, label: 3}) return DISTINCT n0.graphId as graphId");
        }

        [TestMethod]
        public void Convert_TwoEdgesWhenTheFirstIsReversed_ShouldConvertCorrectly()
        {
            List<DFS_Code> path = new List<DFS_Code>();
            path.Add(new DFS_Code()
            {
                u = 2,
                l_u = 2,
                v = 1,
                l_v = 1,
                l_w=1
            });

            path.Add(new DFS_Code()
            {
                u = 2,
                l_u = 2,
                v = 3,
                l_v = 3,
                l_w = 2
            });

            var converter = new EdgePathToCypherQueryConverter();
            var query = converter.Convert(path);

            Assert.AreEqual(query, "match(n0:Node { id: 1, label: 1})-[:CONNECTED_TO {label: 1 }]-(n1 {id: 2, label: 2})-[:CONNECTED_TO {label: 2 }]-(n2 {id: 3, label: 3}) return DISTINCT n0.graphId as graphId");
        }

        [TestMethod]
        public void Convert_TwoEdgesWhenTheSecondIsReversed_ShouldConvertCorrectly()
        {
            List<DFS_Code> path = new List<DFS_Code>();
            path.Add(new DFS_Code()
            {
                u = 1,
                l_u = 1,
                v = 2,
                l_v = 2,
                l_w = 1
            });

            path.Add(new DFS_Code()
            {
                u = 3,
                l_u = 3,
                v = 2,
                l_v = 2,
                l_w = 2
            });

            var converter = new EdgePathToCypherQueryConverter();
            var query = converter.Convert(path);

            Assert.AreEqual(query, "match(n0:Node { id: 1, label: 1})-[:CONNECTED_TO {label: 1 }]-(n1 {id: 2, label: 2})-[:CONNECTED_TO {label: 2 }]-(n2 {id: 3, label: 3}) return DISTINCT n0.graphId as graphId");
        }

        [TestMethod]
        public void Convert_TwoEdgesWhenBothIsReversed_ShouldConvertCorrectly()
        {
            List<DFS_Code> path = new List<DFS_Code>();
            path.Add(new DFS_Code()
            {
                u = 2,
                l_u = 2,
                v = 1,
                l_v = 1,
                l_w=1
            });

            path.Add(new DFS_Code()
            {
                u = 3,
                l_u = 3,
                v = 2,
                l_v = 2,
                l_w = 2
            });

            var converter = new EdgePathToCypherQueryConverter();
            var query = converter.Convert(path);

            Assert.AreEqual(query, "match(n0:Node { id: 1, label: 1})-[:CONNECTED_TO {label: 1 }]-(n1 {id: 2, label: 2})-[:CONNECTED_TO {label: 2 }]-(n2 {id: 3, label: 3}) return DISTINCT n0.graphId as graphId");
        }
    }
}
