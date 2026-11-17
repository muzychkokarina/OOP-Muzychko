using System.Text;
using InvertedIndexDemo;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

Console.WriteLine("=====================================================");
Console.WriteLine("  ЧАСТИНА 1-2: Тестування методів Inverted Index");
Console.WriteLine("=====================================================\n");

var index = new InvertedIndex();
foreach (var doc in PerformanceTest.GetBaseDocuments())
{
    index.AddDocument(doc);
}

Console.WriteLine($"Loaded {index.DocumentCount} documents");
Console.WriteLine($"Index contains {index.UniqueWords} unique words\n");

Console.WriteLine("--- Single Word Search: 'java' ---");
foreach (var doc in index.SearchSingleWord("java"))
    Console.WriteLine($"  {doc}");

Console.WriteLine("\n--- Multiple Words Search (AND): 'programming', 'language' ---");
foreach (var doc in index.SearchMultipleWords("programming", "language"))
    Console.WriteLine($"  {doc}");

Console.WriteLine("\n--- Any Word Search (OR): 'python', 'linq' ---");
foreach (var doc in index.SearchAnyWord("python", "linq"))
    Console.WriteLine($"  {doc}");

Console.WriteLine("\n=====================================================");
Console.WriteLine("  ЧАСТИНА 3: Вимірювання продуктивності");
Console.WriteLine("=====================================================\n");

int[] sizes = { 10, 100, 1000, 10000 };
foreach (var size in sizes)
{
    var docs = PerformanceTest.GenerateDocuments(size);
    PerformanceTest.CompareSearchMethods(docs, "programming", iterations: 500);
}

Console.WriteLine("=====================================================");
Console.WriteLine("  ЧАСТИНА 4: Інтерактивний пошук (AND)");
Console.WriteLine("=====================================================");

while (true)
{
    Console.Write("\nEnter search query (or 'exit'): ");
    var query = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(query) || query.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase))
        break;

    var words = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    Console.WriteLine("\n--- Search Results (AND) ---");
    var results = index.SearchMultipleWords(words);

    foreach (var doc in results)
    {
        Console.WriteLine(doc);
        Console.WriteLine($"  {doc.Content.Substring(0, Math.Min(100, doc.Content.Length))}...");
    }

    Console.WriteLine($"\nFound {results.Count} documents");
}