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
        private static IDriver _neo4jDriver;
        private static object _lock = new object();
        private static IDriver neo4jdriver
        {
            get
            {
                if (_neo4jDriver == null)
                {
                    lock (_lock)
                    {
                        if (_neo4jDriver == null)
                        {
                            _neo4jDriver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "Aa123456"));
                        }
                    }
                }

                return _neo4jDriver;
            }
        }

        public static ISession GetSession()
        {
            return neo4jdriver.Session();
        }
    }
}
