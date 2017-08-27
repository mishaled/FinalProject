using Neo4j.Driver.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class Neo4jConnectionManager
    {
        private static IDriver neo4jdriver;

        public static void Initialize(string neo4jUrl, string username, string password)
        {
            if (neo4jdriver == null)
            {
                neo4jdriver = GraphDatabase.Driver(neo4jUrl, AuthTokens.Basic(username, password));
            }
        }

        public static ISession GetSession()
        {
            return neo4jdriver.Session();
        }
    }
}
