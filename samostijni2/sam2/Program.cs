using System;
using System.Text;

namespace sam2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== САМОСТІЙНА РОБОТА №14: Dictionary<TKey, TValue> та HashSet<T> ===\n");

            Console.WriteLine("--- 1. FileMetadataCache (Dictionary) ---");
            var metadataCache = new FileMetadataCache();
            metadataCache.Add("/docs/report.pdf", new FileMetadata
            {
                FileName = "report.pdf",
                Size = 245760,
                LastModified = DateTime.Now.AddHours(-2),
                Hash = "A1B2C3D4"
            });
            metadataCache.Add("/src/Program.cs", new FileMetadata
            {
                FileName = "Program.cs",
                Size = 4096,
                LastModified = DateTime.Now,
                Hash = "E5F6G7H8"
            });

            Console.WriteLine($"Contains '/DOCS/REPORT.PDF': {metadataCache.Contains("/DOCS/REPORT.PDF")}");
            Console.WriteLine($"Get: {metadataCache.Get("/docs/report.pdf")}");
            metadataCache.Remove("/src/Program.cs");
            Console.WriteLine($"Count після видалення: {metadataCache.Count}\n");

            Console.WriteLine("--- 2. TagManager (Dictionary + HashSet) ---");
            var tagManager = new TagManager();
            tagManager.AddTag("art-1", "C#");
            tagManager.AddTag("art-1", "LINQ");
            tagManager.AddTag("art-1", "Collections");
            tagManager.AddTag("art-1", "c#");

            tagManager.AddTag("art-2", "C#");
            tagManager.AddTag("art-2", "Collections");
            tagManager.AddTag("art-2", "Performance");

            Console.WriteLine($"Усі унікальні теги (UnionWith): {string.Join(", ", tagManager.GetAllUniqueTags())}");
            Console.WriteLine($"Спільні теги art-1 і art-2 (IntersectWith): {string.Join(", ", tagManager.FindCommonTags("art-1", "art-2"))}");
            Console.WriteLine($"Унікальні лише для art-1 (ExceptWith): {string.Join(", ", tagManager.GetExclusiveTags("art-1", "art-2"))}");

            tagManager.RemoveTag("art-1", "LINQ");
            Console.WriteLine($"Унікальні теги після видалення 'LINQ': {string.Join(", ", tagManager.GetAllUniqueTags())}\n");

            Console.WriteLine("--- 3. SearchIndex (Інвертований індекс) ---");
            var searchIndex = new SearchIndex();
            searchIndex.AddDocument(1, "C# collections Dictionary and HashSet provide fast lookup");
            searchIndex.AddDocument(2, "LINQ queries work seamlessly with C# collections");
            searchIndex.AddDocument(3, "HashSet stores unique elements without duplicates");

            Console.WriteLine($"Документи зі словом 'HashSet': [{string.Join(", ", searchIndex.Search("HashSet"))}]");
            Console.WriteLine($"Документи зі словами 'C#' ТА 'collections': [{string.Join(", ", searchIndex.SearchMultiple("C#", "collections"))}]");
        }
    }
}