using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface ILogger
    {
        //ILogger Instance { get; }
        void WriteDebug(string msg);
        void WriteInfo(string msg);
        void WriteWarning(string msg);
        void WriteError(Exception e, string msg = null);
    }
}
