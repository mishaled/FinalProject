using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Common;
using DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Moq;

namespace BL.UnitTests
{
    [TestClass]
    public class PatternMatcherUnitTests
    {
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestMethod]
        public void Match_DALReturnsEmptyLists_ShouldReturnEmptyList()
        {
            Mock<INeo4jDAL> mockDal = new Mock<INeo4jDAL>();
            mockDal
                .Setup(x => x.GetMatchingGraphsIds(It.IsAny<List<DFS_Code>>()))
                .Returns(new List<int>());
            DIFactory.Register(mockDal.Object);

            var matcher = new PatternMatcher();

            var query = MockGraphFactory.GenerateSquareGraph();
            var matches = matcher.Match(query);

            Assert.IsFalse(matches.Any());
        }
    }
}
