using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Model;
using VDS.Common.Tries;

namespace BL
{
    public class GIndex// : IDisposable
    {
        private double _minSup;
        //private Trie<string> _trie;
        private StringTrie<string> _trie;
        

        public GIndex(double minSup)
        {
            _minSup = minSup;
            _trie = new StringTrie<string>();
        }

        //~GIndex()
        //{
        //    Dispose(false);
        //}

        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}

        //private void Dispose(bool disposing)
        //{
        //    foreach (var value in _trie.)
        //    {
        //        File.Delete(value);
        //    }
        //}

        public void Fill(List<Graph> graphDb)
        {
            FrequentFeatureSelector ffSelector = new FrequentFeatureSelector();
            Dictionary<Graph, List<int>> frequentFeaturesMap = ffSelector.Select(graphDb, _minSup);

            foreach (KeyValuePair<Graph, List<int>> ff in frequentFeaturesMap)
            {
                GenerateFileAndInsertIntoTrie(ff);
            }
        }

        private void GenerateFileAndInsertIntoTrie(KeyValuePair<Graph, List<int>> ff)
        {
            string filename = Guid.NewGuid().ToString();
            var ffSelector = new FrequentFeatureSelector();
            string key = string.Join(",", ffSelector.ComputeCanonicalLabel(ff.Key));
            string value = string.Join(",", ff.Value);

            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.WriteLine(value);
            }

            if (!_trie.ContainsKey(ff.Key.ToString()))
            {
                _trie.Add(key, filename);
            }
            else
            {
                _trie[key] = _trie[key] + ";" + filename;
            }
        }

        public List<Graph> Search(Graph query, List<Graph> graphDb)
        {
            Dictionary<Graph, string> fragmentsToCanonicalLabelsDict = FindQueryFragments(query);

            List<int> idsList = new List<int>();
            foreach (Graph key in fragmentsToCanonicalLabelsDict.Keys)
            {
                List<int> graphIds = GetGraphIdsForFragment(fragmentsToCanonicalLabelsDict, key);
                idsList.AddRange(graphIds);
            }

            var isomorhpismChecker = new SubgraphIsomorphismGenerator();
            var graphs =
                idsList
                .Distinct()
                .Select(x => graphDb.First(y => y.id == x))
                .ToList();

            return graphs
                .Where(x => isomorhpismChecker.IsSubgraphIsomorphic(query, x))
                .ToList();
        }

        private List<int> GetGraphIdsForFragment(Dictionary<Graph, string> fragmentsToCanonicalLabelsDict, Graph key)
        {
            string filename;
            if (!_trie.TryGetValue(fragmentsToCanonicalLabelsDict[key], out filename))
            {
                return new List<int>();
            }

            string ids;
            using (StreamReader sr = new StreamReader(filename))
            {
                ids = sr.ReadLine();
            }

            if (string.IsNullOrEmpty(ids))
            {
                return new List<int>();
            }

            return
                    ids
                    .Split(',')
                    .Select(int.Parse)
                    .ToList();
        }

        private static Dictionary<Graph, string> FindQueryFragments(Graph query)
        {
            FrequentFeatureSelector ffSelector = new FrequentFeatureSelector();
            Dictionary<Graph, List<int>> fragments = ffSelector.Select(new List<Graph>() { query }, 1);
            Dictionary<Graph, string> fragmentsToCanonicalLabelsDict = fragments
                .ToDictionary(
                    x => x.Key,
                    y => string.Join(",", ffSelector.ComputeCanonicalLabel(y.Key)));
            return fragmentsToCanonicalLabelsDict;
        }
    }
}
