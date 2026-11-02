using System;
using System.Collections.Generic;

namespace lab14v13
{
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }

        public Product(string name, decimal price, string category)
        {
            Name = name;
            Price = price;
            Category = category;
        }

        public override string ToString() => $"{Name} ({Category}) - {Price:C}";
    }

    public static class CollectionHelper<T>
    {
        public static IEnumerable<T> DistinctBy<TKey>(IEnumerable<T> collection, Func<T, TKey> keySelector)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            var seenKeys = new HashSet<TKey>();

            foreach (var item in collection)
            {
                TKey key = keySelector(item);
                if (seenKeys.Add(key))
                {
                    yield return item;
                }
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== Лабораторна робота №8 (Варіант 13: DistinctBy) ===");
            Console.WriteLine();

            List<string> words = new List<string> { "Яблуко", "Банан", "Ананас", "Вишня", "Кавун", "Борщ" };

            Console.WriteLine("Оригінальний список слів:");
            Console.WriteLine(string.Join(", ", words));

            var uniqueByFirstLetter = CollectionHelper<string>.DistinctBy(words, w => w[0]);

            Console.WriteLine("\nСлова з унікальними першими літерами:");
            foreach (var word in uniqueByFirstLetter)
            {
                Console.WriteLine($" - {word} (Перша літера: '{word[0]}')");
            }

            Console.WriteLine(new string('-', 50));

            List<Product> products = new List<Product>
            {
                new Product("Ноутбук", 25000m, "Електроніка"),
                new Product("Мишка", 500m, "Електроніка"),
                new Product("Крісло", 4500m, "Меблі"),
                new Product("Стіл", 3200m, "Меблі"),
                new Product("Клавіатура", 1200m, "Електроніка"),
                new Product("Яблуко", 30m, "Продукти харчування")
            };

            Console.WriteLine("Оригінальний список продуктів:");
            foreach (var p in products)
            {
                Console.WriteLine($" - {p}");
            }

            var uniqueCategories = CollectionHelper<Product>.DistinctBy(products, p => p.Category);

            Console.WriteLine("\nУнікальні продукти за категоріями:");
            foreach (var product in uniqueCategories)
            {
                Console.WriteLine($" - Категорія: \"{product.Category}\" -> Перший представник: {product.Name}");
            }
        }
    }
}