using System;
using System.Collections.Generic;
using System.Text;

namespace lab9_self_v13
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== САМОСТІЙНА РОБОТА №15: Делегати й лямбда-вирази (CollectionUtils) ===\n");

            Console.WriteLine("--- 1. Колекція цілих чисел (List<int>) ---");
            var numbers = new List<int> { 12, 5, 28, 3, 19, 44, 8 };

            var squared = CollectionUtils.Map(numbers, n => n * n);
            var evenNumbers = CollectionUtils.Filter(numbers, n => n % 2 == 0);
            var maxNumber = CollectionUtils.MaxBy(numbers, n => n);

            Console.Write("Вихідні числа: ");
            CollectionUtils.ForEach(numbers, n => Console.Write($"{n} "));
            Console.WriteLine($"\nКвадрати чисел (Map): {string.Join(", ", squared)}");
            Console.WriteLine($"Парні числа (Filter): {string.Join(", ", evenNumbers)}");
            Console.WriteLine($"Максимальне число (MaxBy): {maxNumber}\n");

            Console.WriteLine("--- 2. Колекція рядків (List<string>) ---");
            var technologies = new List<string> { "C#", "LINQ", "Delegates", "Lambda", "Generics", ".NET" };

            var upperTech = CollectionUtils.Map(technologies, t => t.ToUpper());
            var longWords = CollectionUtils.Filter(technologies, t => t.Length > 4);
            var longestWord = CollectionUtils.MaxBy(technologies, t => t.Length);

            Console.WriteLine($"У верхньому регістрі (Map): {string.Join(", ", upperTech)}");
            Console.WriteLine($"Слова довші за 4 символи (Filter): {string.Join(", ", longWords)}");
            Console.WriteLine($"Найдовше слово (MaxBy): {longestWord} ({longestWord.Length} симв.)\n");

            Console.WriteLine("--- 3. Колекція товарів (List<Product>) ---");
            var products = new List<Product>
            {
                new Product { Name = "Ноутбук Lenovo", Category = "Техніка", Price = 32500m },
                new Product { Name = "Мишка Logitech", Category = "Аксесуари", Price = 1200m },
                new Product { Name = "Монітор Dell 27\"", Category = "Техніка", Price = 14800m },
                new Product { Name = "Клавіатура Keychron", Category = "Аксесуари", Price = 4200m }
            };

            var priceLabels = CollectionUtils.Map(products, p => $"{p.Name} ({p.Price} грн)");
            var techProducts = CollectionUtils.Filter(products, p => p.Category == "Техніка" && p.Price > 10000m);
            var mostExpensive = CollectionUtils.MaxBy(products, p => p.Price);

            Console.WriteLine("Усі прайси (Map): " + string.Join("; ", priceLabels));
            Console.WriteLine($"Найдорожчий товар (MaxBy): {mostExpensive.Name} — {mostExpensive.Price} грн");
            Console.WriteLine("Дорожча техніка (Filter + ForEach):");
            CollectionUtils.ForEach(techProducts, p => Console.WriteLine($"  * {p.Name}: {p.Price} грн"));
            Console.WriteLine();

            Console.WriteLine("--- 4. Колекція студентів (List<Student>) ---");
            var students = new List<Student>
            {
                new Student { FullName = "Музичко Карина", Course = 3, AverageGrade = 96.5 },
                new Student { FullName = "Ковальчук Артем", Course = 3, AverageGrade = 84.0 },
                new Student { FullName = "Бондаренко Софія", Course = 2, AverageGrade = 93.2 }
            };

            var thirdCourseHonor = CollectionUtils.Filter(students, s => s.Course == 3 && s.AverageGrade >= 90);
            var topStudent = CollectionUtils.MaxBy(students, s => s.AverageGrade);
            var summaries = CollectionUtils.Map(thirdCourseHonor, s => $"{s.FullName} (середній бал: {s.AverageGrade})");

            Console.WriteLine($"Відмінники 3-го курсу (Filter + Map): {string.Join(", ", summaries)}");
            Console.WriteLine($"Найкращий студент потоку (MaxBy): {topStudent.FullName} — {topStudent.AverageGrade}");
        }
    }
}
