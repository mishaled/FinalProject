using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using DAL;
using Model;

namespace BL
{
    public class PatternMatcher
    {
        public List<Graph> Match(Graph query)
        {
            List<Graph> unverifiedMatches = GetUnverifiedMatches(query);
            DIFactory.Resolve<ILogger>().WriteDebug(string.Format("Found {0} unverified matches", unverifiedMatches.Count));

            List<Graph> verifiedMatches = unverifiedMatches
                .Where(match => Verify(match, query))
                .ToList();
            DIFactory.Resolve<ILogger>().WriteDebug(string.Format("Found {0} verified matches", verifiedMatches.Count));

            return verifiedMatches;
        }

        private List<Graph> GetUnverifiedMatches(Graph query)
        {
            List<int> unverifiedMatchesIds = GetUnverifiedMatchesIds(query);
            INeo4jDAL dal = DIFactory.Resolve<INeo4jDAL>();
            List<Graph> matches = new List<Graph>();

            unverifiedMatchesIds.ForEach(matchId =>
            {
                matches.Add(dal.GetGraphById(matchId));
            });

            return matches;
        }

        private List<int> GetUnverifiedMatchesIds(Graph query)
        {
            INeo4jDAL dal = DIFactory.Resolve<INeo4jDAL>();

            GraphPathsGenerator graphPathsGenerator = new GraphPathsGenerator();
            List<List<DFS_Code>> paths = graphPathsGenerator.Generate(query);

            DIFactory.Resolve<ILogger>().WriteDebug(string.Format("Found {0} paths in the query", paths.Count));

            List<int> idsList = new List<int>();

            foreach (var path in paths)
            {
                idsList.AddRange(dal.GetMatchingGraphsIds(path));
            }

            return idsList.Distinct().ToList();
        }

        public bool Verify(Graph match, Graph query)
        {
            SubgraphIsomorphismGenerator generator = new SubgraphIsomorphismGenerator();
            return generator.IsSubgraphIsomorphic(query, match);
        }
    }
}
