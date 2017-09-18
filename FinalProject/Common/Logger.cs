using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Common;
using NLog.Conditions;
using NLog.Targets;

namespace Common
{
    public class Logger : ILogger
    {
        private static NLog.Logger _nLogger;

        public Logger()
        {
            _nLogger = LogManager.GetCurrentClassLogger();
        }

        public void WriteInfo(string msg)
        {
            _nLogger.Info(msg);
            LogManager.Flush();
        }

        public void WriteWarning(string msg)
        {
            _nLogger.Warn(msg);
            LogManager.Flush();
        }

        public void WriteError(Exception ex, string msg = null)
        {
            _nLogger.Error(ex, msg);
            LogManager.Flush();
        }
    }
}
