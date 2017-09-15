using System;
using System.Text;
using System.Collections.Generic;
using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BL.UnitTests
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
