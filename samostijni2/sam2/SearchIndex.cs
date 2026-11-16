using System;
using System.Collections.Generic;
using System.Linq;

namespace sam2
{
    public class SearchIndex
    {
        private readonly Dictionary<string, HashSet<int>> _invertedIndex;

        public SearchIndex()
        {
            _invertedIndex = new Dictionary<string, HashSet<int>>(StringComparer.OrdinalIgnoreCase);
        }

        public void AddDocument(int docId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return;

            char[] separators = { ' ', ',', '.', '!', '?', ';', ':', '-', '\n', '\r', '\t' };
            string[] words = content.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in words)
            {
                if (!_invertedIndex.TryGetValue(word, out var docIds))
                {
                    docIds = new HashSet<int>();
                    _invertedIndex[word] = docIds;
                }

                docIds.Add(docId);
            }
        }

        public HashSet<int> Search(string word)
        {
            if (_invertedIndex.TryGetValue(word, out var docIds))
            {
                return new HashSet<int>(docIds);
            }

            return new HashSet<int>();
        }

        public HashSet<int> SearchMultiple(params string[] words)
        {
            if (words == null || words.Length == 0)
                return new HashSet<int>();

            HashSet<int>? result = null;

            foreach (string word in words)
            {
                var docsForWord = Search(word);
                if (result == null)
                {
                    result = docsForWord;
                }
                else
                {
                    result.IntersectWith(docsForWord);
                }
            }

            return result ?? new HashSet<int>();
        }
    }
}