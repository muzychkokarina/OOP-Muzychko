using System.Collections.Concurrent;

namespace IndependentWork12;

public static class ThreadSafetyDemo
{
    public static void RunSideEffectsExperiment()
    {
        const int count = 1_000_000;
        var numbers = Enumerable.Range(1, count).ToList();

        Console.WriteLine("=====================================================================");
        Console.WriteLine("  ДОСЛІДЖЕННЯ БЕЗПЕКИ (ПОБІЧНІ ЕФЕКТИ В PLINQ)");
        Console.WriteLine($"  Очікувана кількість парних чисел у діапазоні 1..{count:N0}: {count / 2:N0}");
        Console.WriteLine("=====================================================================\n");

        Console.WriteLine("1. Небезпечна модифікація спільної змінної без синхронізації (Race Condition):");
        for (int attempt = 1; attempt <= 5; attempt++)
        {
            int unsafeCounter = 0;
            var _ = numbers
                .AsParallel()
                .Where(n => n % 2 == 0)
                .Select(n =>
                {
                    unsafeCounter++;
                    return n;
                })
                .Count();

            Console.WriteLine($"   Спроба {attempt}: unsafeCounter = {unsafeCounter:N0} (втрачено: {count / 2 - unsafeCounter:N0})");
        }

        Console.WriteLine("\n2. Небезпечне додавання у звичайний List<int> з PLINQ:");
        for (int attempt = 1; attempt <= 3; attempt++)
        {
            var unsafeList = new List<int>();
            try
            {
                var _ = numbers
                    .AsParallel()
                    .Where(n => n % 2 == 0)
                    .Select(n =>
                    {
                        unsafeList.Add(n);
                        return n;
                    })
                    .Count();

                Console.WriteLine($"   Спроба {attempt}: unsafeList.Count = {unsafeList.Count:N0} (очікувалось {count / 2:N0})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   Спроба {attempt}: Виняток {ex.GetType().Name} — пошкодження внутрішнього масиву List<T>!");
            }
        }

        Console.WriteLine("\n3. Виправлення через Interlocked.Increment:");
        int safeInterlockedCounter = 0;
        var _1 = numbers
            .AsParallel()
            .Where(n => n % 2 == 0)
            .Select(n =>
            {
                Interlocked.Increment(ref safeInterlockedCounter);
                return n;
            })
            .Count();
        Console.WriteLine($"   Результат з Interlocked: {safeInterlockedCounter:N0}");

        Console.WriteLine("\n4. Виправлення через оператор lock:");
        int safeLockCounter = 0;
        object syncRoot = new object();
        var _2 = numbers
            .AsParallel()
            .Where(n => n % 2 == 0)
            .Select(n =>
            {
                lock (syncRoot)
                {
                    safeLockCounter++;
                }
                return n;
            })
            .Count();
        Console.WriteLine($"   Результат з lock:        {safeLockCounter:N0}");

        Console.WriteLine("\n5. Виправлення через потокобезпечну колекцію ConcurrentBag<int>:");
        var safeBag = new ConcurrentBag<int>();
        var _3 = numbers
            .AsParallel()
            .Where(n => n % 2 == 0)
            .Select(n =>
            {
                safeBag.Add(n);
                return n;
            })
            .Count();
        Console.WriteLine($"   Результат ConcurrentBag: {safeBag.Count:N0}");

        Console.WriteLine("\n6. Ідіоматичний чистий PLINQ без побічних ефектів (агрегація):");
        int purePlinqCount = numbers
            .AsParallel()
            .Count(n => n % 2 == 0);
        Console.WriteLine($"   Чистий PLINQ .Count():   {purePlinqCount:N0}\n");
    }
}