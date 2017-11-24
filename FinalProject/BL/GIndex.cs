using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common;
using DAL;
using Model;
using VDS.Common.Tries;

namespace BL
{
    public class GIndex// : IDisposable
    {
        private readonly int _minSup;
        //private Trie<string> _trie;
        private StringTrie<string> _trie;
        private List<string> _filesForCleanup;
        private string _gIndexDirectoryPath;

        public GIndex(int minSup = -1)
        {
            _minSup = minSup;
            _trie = new StringTrie<string>();
            _filesForCleanup = new List<string>();

            string basePath = System.Environment.CurrentDirectory;
            _gIndexDirectoryPath = Path.Combine(basePath, "GIndex");

            if (Directory.Exists(_gIndexDirectoryPath))
            {
                Directory.Delete(_gIndexDirectoryPath, true);
            }

            Directory.CreateDirectory(_gIndexDirectoryPath);

            DIFactory.Resolve<ILogger>().WriteDebug("Initialized GIndex");
        }

        ~GIndex()
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
            foreach (string file in _filesForCleanup)
            {
                File.Delete(file);
            }
        }

        public void Fill(List<Graph> graphDb)
        {
            if (_minSup <= 0)
            {
                throw new ArgumentException("Must initialize with positive minSup");
            }

            FrequentFeatureSelector ffSelector = new FrequentFeatureSelector();
            Dictionary<Graph, List<int>> frequentFeaturesMap = ffSelector.Select(graphDb, _minSup);

            Fill(frequentFeaturesMap);
        }

        public void Fill(Dictionary<Graph, List<int>> frequentFeaturesMap)
        {
            foreach (KeyValuePair<Graph, List<int>> ff in frequentFeaturesMap)
            {
                GenerateFileAndInsertIntoTrie(ff);
            }

            DIFactory.Resolve<ILogger>().WriteDebug("Filled gIndex with frequent features");
        }

        private void GenerateFileAndInsertIntoTrie(KeyValuePair<Graph, List<int>> ff)
        {
            string filename = Path.Combine(_gIndexDirectoryPath, Guid.NewGuid().ToString());
            FrequentFeatureSelector ffSelector = new FrequentFeatureSelector();
            string key = string.Join(",", ffSelector.ComputeCanonicalLabel(ff.Key));
            string value = string.Join(",", ff.Value);

            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.WriteLine(value);
            }

            _filesForCleanup.Add(filename);

            if (!_trie.ContainsKey(ff.Key.ToString()))
            {
                _trie.Add(key, filename);
            }
            else
            {
                _trie[key] = _trie[key] + ";" + filename;
            }
        }

        public List<Graph> Search(Graph query)
        {
            Dictionary<Graph, string> fragmentsToCanonicalLabelsDict = FindQueryFragments(query);

            DIFactory.Resolve<ILogger>().WriteDebug(string.Format("Found {0} fragments in the query", fragmentsToCanonicalLabelsDict.Count));

            List<int> idsList = new List<int>();

            foreach (Graph key in fragmentsToCanonicalLabelsDict.Keys)
            {
                List<int> graphIds = GetGraphIdsForFragment(fragmentsToCanonicalLabelsDict, key);
                idsList.AddRange(graphIds);

                DIFactory.Resolve<ILogger>().WriteDebug(string.Format("Fragment {0} yielded {1} ids", fragmentsToCanonicalLabelsDict[key], graphIds.Count));
            }

            var isomorhpismChecker = new SubgraphIsomorphismGenerator();
            var unverifiedGraphs =
                idsList
                    .Distinct()
                    .Select(x => DIFactory.Resolve<INeo4jDAL>().GetGraphById(x))
                    .ToList();

            DIFactory.Resolve<ILogger>().WriteDebug(string.Format("GIndex Found {0} unverified graphs that answer the query", unverifiedGraphs.Count));

            var verifiedGraphs =
                unverifiedGraphs.Where(x => isomorhpismChecker.IsSubgraphIsomorphic(query, x))
                .ToList();

            DIFactory.Resolve<ILogger>().WriteDebug(string.Format("GIndex Found {0} verified graphs that answer the query", verifiedGraphs.Count));

            return verifiedGraphs;
        }

        private List<int> GetGraphIdsForFragment(Dictionary<Graph, string> fragmentsToCanonicalLabelsDict, Graph key)
        {
            string filenames;
            if (!_trie.TryGetValue(fragmentsToCanonicalLabelsDict[key], out filenames))
            {
                return new List<int>();
            }

            List<string> idsLists = new List<string>();

            foreach (var filename in filenames.Split(';'))
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    idsLists.Add(sr.ReadLine());
                }
            }

            if (!idsLists.Any())
            {
                return new List<int>();
            }

            return
                string.Join(",", idsLists)
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

            foreach (var value in fragmentsToCanonicalLabelsDict.Values)
            {
                DIFactory.Resolve<ILogger>().WriteDebug(string.Format("A fragment's canonical label: {0}", value));
            }

            return fragmentsToCanonicalLabelsDict;
        }

        public override string ToString()
        {
            return PrintPretty("", _trie.Root);
        }

        private string PrintPretty(string indent, ITrieNode<char, string> trieNode)
        {
            string result = string.Empty;

            result += indent;

            if (trieNode.IsLeaf || trieNode.IsRoot)
            {
                result += "└─";
                indent += "  ";
            }
            else
            {
                result += "├─";
                indent += "| ";
                //indent += "  ";
            }

            result += trieNode.KeyBit + "\n";
            result += trieNode.Value;

            for (int i = 0; i < trieNode.Descendants.Count(); i++)
            {
                result += "\n" + PrintPretty(indent, trieNode.Descendants.ElementAt(i));
            }

            return result;
        }
    }
}
