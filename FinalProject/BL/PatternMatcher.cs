using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace BL
{
    public class PatternMatcher
    {
        public List<int> Match(Graph query)
        {
            GraphPathsGenerator graphPathsGenerator = new GraphPathsGenerator();
            var paths = graphPathsGenerator.Generate(query);

            foreach (var path in paths)
            {
                new Neo4jDAL
            }
        }
    }
}
