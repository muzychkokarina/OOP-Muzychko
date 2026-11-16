using System;
using System.Collections.Generic;
using System.Text;

namespace IndependentWork7
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== САМОСТІЙНА РОБОТА №13 (IndependentWork7: FIFO Cache<TKey, TValue>) ===\n");

            var cache = new Cache<string, string>(capacity: 3);

            Console.WriteLine("1. Додаємо 3 елементи (ємність кешу = 3):");
            cache.Add("user:1", "Карина");
            cache.Add("user:2", "Олександр");
            cache.Add("user:3", "Марія");
            Console.WriteLine($"   Кількість елементів у кеші (Count): {cache.Count}");
            Console.WriteLine($"   Отримання 'user:1': {cache.Get("user:1")}");

            Console.WriteLine("\n2. Додаємо 4-й елемент 'user:4' (має витіснити найстаріший 'user:1'):");
            cache.Add("user:4", "Дмитро");
            Console.WriteLine($"   Кількість елементів у кеші (Count): {cache.Count}");
            Console.WriteLine($"   ContainsKey('user:1'): {cache.ContainsKey("user:1")} (витіснено за FIFO)");
            Console.WriteLine($"   ContainsKey('user:4'): {cache.ContainsKey("user:4")}");

            Console.WriteLine("\n3. Спроба отримати видалений ключ 'user:1':");
            try
            {
                cache.Get("user:1");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"   Очікуваний виняток: {ex.Message}");
            }
        }
    }
}