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
        //private static Logger _logger;
        //private static object _lock = new object();
        //public static ILogger Instance
        //{
        //    get
        //    {
        //        if (_logger == null)
        //        {
        //            lock (_lock)
        //            {
        //                if (_logger == null)
        //                {
        //                    _logger = new Logger();
        //                }
        //            }
        //        }

        //        return _logger;
        //    }
        //}

        public Logger()
        {
            _nLogger = LogManager.GetCurrentClassLogger();
        }

        public void WriteInfo(string msg)
        {
            _nLogger.Info(msg);
        }

        public void WriteWarning(string msg)
        {
            _nLogger.Warn(msg);
        }

        public void WriteError(Exception ex, string msg = null)
        {
            _nLogger.Error(ex, msg);
        }
    }
}
