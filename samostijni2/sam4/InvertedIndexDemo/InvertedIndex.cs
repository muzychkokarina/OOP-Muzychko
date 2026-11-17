namespace InvertedIndexDemo;

public class InvertedIndex
{
    private readonly Dictionary<string, HashSet<int>> _index;
    private readonly Dictionary<int, Document> _documents;

    public InvertedIndex()
    {
        _index = new Dictionary<string, HashSet<int>>();
        _documents = new Dictionary<int, Document>();
    }

    public void AddDocument(Document doc)
    {
        if (_documents.ContainsKey(doc.Id))
            return;

        _documents[doc.Id] = doc;

        var words = TextProcessor.Tokenize(doc.Content);

        foreach (var word in words)
        {
            if (!_index.TryGetValue(word, out var docIds))
            {
                docIds = new HashSet<int>();
                _index[word] = docIds;
            }
            docIds.Add(doc.Id);
        }
    }

    public List<Document> SearchSingleWord(string word)
    {
        var token = TextProcessor.Tokenize(word).FirstOrDefault();
        if (string.IsNullOrEmpty(token))
            return new List<Document>();

        if (_index.TryGetValue(token, out var docIds))
        {
            return docIds.Select(id => _documents[id]).ToList();
        }

        return new List<Document>();
    }

    public List<Document> SearchMultipleWords(params string[] words)
    {
        var tokens = words
            .SelectMany(TextProcessor.Tokenize)
            .Distinct()
            .ToList();

        if (tokens.Count == 0)
            return new List<Document>();

        if (!_index.TryGetValue(tokens[0], out var firstSet))
            return new List<Document>();

        var resultIds = new HashSet<int>(firstSet);

        for (int i = 1; i < tokens.Count; i++)
        {
            if (_index.TryGetValue(tokens[i], out var nextSet))
            {
                resultIds.IntersectWith(nextSet);
                if (resultIds.Count == 0)
                    break;
            }
            else
            {
                return new List<Document>();
            }
        }

        return resultIds.Select(id => _documents[id]).ToList();
    }

    public List<Document> SearchAnyWord(params string[] words)
    {
        var tokens = words
            .SelectMany(TextProcessor.Tokenize)
            .Distinct()
            .ToList();

        var resultIds = new HashSet<int>();

        foreach (var token in tokens)
        {
            if (_index.TryGetValue(token, out var docIds))
            {
                resultIds.UnionWith(docIds);
            }
        }

        return resultIds.Select(id => _documents[id]).ToList();
    }

    public int DocumentCount => _documents.Count;
    public int UniqueWords => _index.Keys.Count;
}