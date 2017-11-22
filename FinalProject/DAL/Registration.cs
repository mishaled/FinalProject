using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class Registration
    {
        public static void RegisterDal()
        {
            string neo4jUrl = ConfigurationManager.AppSettings.Get("Neo4jUrl");
            string neo4jUsername = ConfigurationManager.AppSettings.Get("Neo4jUsername");
            string neo4jPassword = ConfigurationManager.AppSettings.Get("Neo4jPassword");
            RegisterDal(neo4jUrl, neo4jUsername, neo4jPassword);
        }

        public static void RegisterDal(string neo4jUrl, string neo4jUsername, string neo4jPassword)
        {
            Neo4jDAL dal = new Neo4jDAL(neo4jUrl, neo4jUsername, neo4jPassword);
            DIFactory.Register<INeo4jDAL>(dal);
        }
    }
}
