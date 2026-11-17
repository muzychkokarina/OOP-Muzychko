using System.Diagnostics;

namespace InvertedIndexDemo;

public static class PerformanceTest
{
    public static List<Document> GetBaseDocuments()
    {
        return new List<Document>
        {
            new Document
            {
                Id = 1,
                Title = "Introduction to C#",
                Content = "C# is a modern object-oriented programming language developed by Microsoft."
            },
            new Document
            {
                Id = 2,
                Title = "Java Basics",
                Content = "Java is a popular programming language known for its portability and performance."
            },
            new Document
            {
                Id = 3,
                Title = "Python Overview",
                Content = "Python is a high-level programming language with simple syntax."
            },
            new Document
            {
                Id = 4,
                Title = "C# Advanced Features",
                Content = "C# supports LINQ, async/await, and many modern programming paradigms."
            },
            new Document
            {
                Id = 5,
                Title = "Comparing Languages",
                Content = "C#, Java, and Python are among the most popular programming languages today."
            }
        };
    }

    public static List<Document> GenerateDocuments(int count)
    {
        var baseDocs = GetBaseDocuments();
        var extraWords = new[]
        {
            "collections", "dictionary", "hashset", "algorithm", "performance",
            "memory", "architecture", "cloud", "database", "testing",
            "asynchronous", "compiler", "framework", "interface", "generics"
        };

        var random = new Random(42);
        var result = new List<Document>(count);

        for (int i = 1; i <= count; i++)
        {
            var template = baseDocs[(i - 1) % baseDocs.Count];
            var randomExtras = string.Join(" ", Enumerable.Range(0, 8)
                .Select(_ => extraWords[random.Next(extraWords.Length)]));

            result.Add(new Document
            {
                Id = i,
                Title = $"{template.Title} #{i}",
                Content = $"{template.Content} Extra tags: {randomExtras}."
            });
        }

        return result;
    }

    public static void CompareSearchMethods(List<Document> documents, string searchWord, int iterations = 1000)
    {
        var normalizedWord = TextProcessor.Tokenize(searchWord).FirstOrDefault() ?? searchWord.ToLowerInvariant();

        var buildSw = Stopwatch.StartNew();
        var index = new InvertedIndex();
        foreach (var doc in documents)
            index.AddDocument(doc);
        buildSw.Stop();

        _ = documents.Where(d => TextProcessor.Tokenize(d.Content).Contains(normalizedWord)).ToList();
        _ = index.SearchSingleWord(searchWord);

        List<Document> linearResults = new();
        var sw = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            linearResults = documents
                .Where(d => TextProcessor.Tokenize(d.Content).Contains(normalizedWord))
                .ToList();
        }
        sw.Stop();
        long linearTicks = sw.ElapsedTicks;
        double linearMs = sw.Elapsed.TotalMilliseconds;

        List<Document> indexResults = new();
        sw.Restart();
        for (int i = 0; i < iterations; i++)
        {
            indexResults = index.SearchSingleWord(searchWord);
        }
        sw.Stop();
        long indexTicks = sw.ElapsedTicks;
        double indexMs = sw.Elapsed.TotalMilliseconds;

        double speedup = indexTicks > 0 ? (double)linearTicks / indexTicks : 0;

        Console.WriteLine($"=== Тест на {documents.Count} документах (пошук слова \"{searchWord}\", {iterations} ітерацій) ===");
        Console.WriteLine($"Час побудови індексу: {buildSw.Elapsed.TotalMilliseconds:F3} ms ({buildSw.ElapsedTicks} ticks)");
        Console.WriteLine($"Linear search:       {linearMs:F3} ms ({linearTicks} ticks)");
        Console.WriteLine($"Inverted Index:      {indexMs:F3} ms ({indexTicks} ticks)");
        Console.WriteLine($"Прискорення:         у {speedup:F1} разів");
        Console.WriteLine($"Перевірка: Linear found = {linearResults.Count}, Index found = {indexResults.Count}\n");
    }
}