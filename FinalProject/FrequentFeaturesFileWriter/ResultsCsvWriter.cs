using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Model;

namespace FrequentFeaturesFileWriter
{
    public class ResultsCsvWriter
    {
        private StreamWriter _sw;
        private int count;

        public ResultsCsvWriter()
        {
            _sw = new StreamWriter("Results_" + Guid.NewGuid() + ".csv");
            _sw.WriteLine("size,time");
            count = 0;
        }

        ~ResultsCsvWriter()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            if (_sw.BaseStream != null)
            {
                _sw.Close();
            }
        }

        public void WriteResult(int size, TimeSpan time)
        {
            _sw.WriteLine(string.Format("{0},{1}", size, time));
            _sw.Flush();
        }
    }
}
