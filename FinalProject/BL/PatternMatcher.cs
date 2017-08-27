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

        public List<Graph> GetUnverifiedMatches(Graph query)
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

        public List<int> GetUnverifiedMatchesIds(Graph query)
        {
            INeo4jDAL dal = DIFactory.Resolve<INeo4jDAL>();

            GraphPathsGenerator graphPathsGenerator = new GraphPathsGenerator();
            var paths = graphPathsGenerator.Generate(query);
            List<List<int>> idsLists = new List<List<int>>();

            foreach (var path in paths)
            {
                idsLists.Add(dal.GetMatchingGraphsIds(path));
            }

            return IntersectNonEmpty(idsLists);
        }

        public bool Verify(Graph match, Graph query)
        {
            SubgraphIsomorphismGenerator generator = new SubgraphIsomorphismGenerator();
            return generator.IsSubgraphIsomorphic(query, match);
        }

        public static List<T> IntersectNonEmpty<T>(IEnumerable<IEnumerable<T>> lists)
        {
            var nonEmptyLists = lists.Where(l => l.Any());
            return nonEmptyLists.Aggregate((l1, l2) => l1.Intersect(l2)).ToList();
        }
    }
}
