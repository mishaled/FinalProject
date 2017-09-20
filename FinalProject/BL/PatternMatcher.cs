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
            List<Graph> verifiedMatches = new List<Graph>();

            foreach (var match in unverifiedMatches)
            {
                if (Verify(match, query))
                {
                    verifiedMatches.Add(match);
                }
            }

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
            var paths = graphPathsGenerator.Generate(query);
            List<int> idsList = new List<int>();

            foreach (var path in paths)
            {
                idsList.AddRange(dal.GetMatchingGraphsIds(path));
            }

            return idsList.Distinct().ToList();

            //return UnionNonEmpty(idsLists);
        }

        public bool Verify(Graph match, Graph query)
        {
            SubgraphIsomorphismGenerator generator = new SubgraphIsomorphismGenerator();
            return generator.IsSubgraphIsomorphic(query, match);
        }

        private static List<T> UnionNonEmpty<T>(IEnumerable<IEnumerable<T>> lists)
        {
            List<IEnumerable<T>> nonEmptyLists = lists.Where(l => l.Any()).ToList();

            if (!nonEmptyLists.Any())
            {
                return new List<T>();
            }

            return nonEmptyLists.Aggregate((l1, l2) => l1.Union(l2)).ToList();
        }
    }
}
