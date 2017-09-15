using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DAL.IntegrationTests
{
    [TestClass]
    public class AssemblyInitializer
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)

        {
            DIFactory.Register<ILogger>(new MockLogger());
        }
    }
}
