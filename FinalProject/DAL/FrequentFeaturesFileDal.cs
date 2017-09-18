using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Common;
using Model;

namespace DAL
{
    public static class FrequentFeaturesFileDal
    {
        public static string Write(Dictionary<Graph, List<int>> frequentFeatures, string originalFileName, double minSup)
        {
            FileInfo originalFileInfo = new FileInfo(originalFileName);
            string ffFileName = string.Format("{0}__{1}__{2}.data", originalFileInfo.Name.Replace(".", "_"),
                minSup.ToString().Replace(".", "_"), Guid.NewGuid());

            string ffFilePath = Path.Combine(originalFileInfo.DirectoryName, ffFileName);

            DIFactory.Resolve<ILogger>().WriteInfo("Start writing FF to file: " + ffFilePath);

            BinaryFormatter serializer = new BinaryFormatter();
            using (Stream stream = File.Open(ffFilePath, FileMode.Create))
            {
                serializer.Serialize(stream, frequentFeatures);
            }

            DIFactory.Resolve<ILogger>().WriteInfo("Finish writing FF to file: " + ffFilePath);

            return ffFilePath;
        }

        public static Dictionary<Graph, List<int>> Read(string ffFileName)
        {
            BinaryFormatter serializer = new BinaryFormatter();
            using (Stream stream = File.Open(ffFileName, FileMode.Open))
            {
                var frequentFeatures = (Dictionary<Graph, List<int>>)serializer.Deserialize(stream);
                return frequentFeatures;
            }
        }
    }
}