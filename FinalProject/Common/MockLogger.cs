using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class MockLogger : ILogger
    {
        public void WriteInfo(string msg)
        {
            //throw new NotImplementedException();
        }

        public void WriteWarning(string msg)
        {
            //throw new NotImplementedException();
        }

        public void WriteError(string msg)
        {
            //throw new NotImplementedException();
        }
    }
}
