using System;
using System.Text;

namespace lab19v13
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== ЛАБОРАТОРНА РОБОТА №13 (Варіант 13: Обробка вхідних даних) ===\n");

            var processor = new InputProcessor();

            Console.WriteLine("--- 1. Обробка власного винятку InvalidInputException (ParseInt) ---");

            string[] numberInputs = { "42", "12abc", "" };

            foreach (var raw in numberInputs)
            {
                try
                {
                    int number = processor.ParseInt(raw);
                    Console.WriteLine($"[Успіх] Рядок '{raw}' розпарсено у число: {number}");
                }
                catch (InvalidInputException ex)
                {
                    Console.WriteLine($"[Виняток InvalidInputException] Поле: '{ex.FieldName}' | Причина: {ex.Reason}");
                }
            }

            Console.WriteLine("\n--- 2. Обробка очікуваних помилок через Result<T> (ValidateString) ---");

            var stringTests = new (string Text, int MinLen)[]
            {
                ("CSharp_LINQ", 6),
                ("abc", 5),
                ("", 3)
            };

            foreach (var test in stringTests)
            {
                Result<bool> validationResult = processor.ValidateString(test.Text, test.MinLen);

                if (validationResult.IsSuccess)
                {
                    Console.WriteLine($"[Result.Success] Рядок '{test.Text}' пройшов перевірку (мін. довжина {test.MinLen}): {validationResult.Value}");
                }
                else
                {
                    Console.WriteLine($"[Result.Failure] Помилка валідації: {validationResult.Error}");
                }
            }

            Console.WriteLine("\n--- 3. Демонстрація Result<T>.Failure(Exception) ---");
            try
            {
                processor.ParseInt("not_a_number");
            }
            catch (InvalidInputException ex)
            {
                Result<int> failedResult = Result<int>.Failure(ex);
                Console.WriteLine($"[Result з винятку] IsSuccess = {failedResult.IsSuccess}, Error = {failedResult.Error}");
            }
        }
    }
}