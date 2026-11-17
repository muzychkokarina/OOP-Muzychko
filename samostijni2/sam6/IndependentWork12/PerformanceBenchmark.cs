using System.Diagnostics;

namespace IndependentWork12;

public static class PerformanceBenchmark
{
    public static void RunHeavyBenchmark(int size)
    {
        var data = MathWorkload.GenerateData(size);

        _ = data.Take(100).Where(MathWorkload.IsPrime).Select(MathWorkload.HeavyMathTransform).ToList();
        _ = data.Take(100).AsParallel().Where(MathWorkload.IsPrime).Select(MathWorkload.HeavyMathTransform).ToList();

        var swLinq = Stopwatch.StartNew();
        var linqResult = data
            .Where(MathWorkload.IsPrime)
            .Select(MathWorkload.HeavyMathTransform)
            .ToList();
        swLinq.Stop();

        var swPlinq = Stopwatch.StartNew();
        var plinqResult = data
            .AsParallel()
            .Where(MathWorkload.IsPrime)
            .Select(MathWorkload.HeavyMathTransform)
            .ToList();
        swPlinq.Stop();

        double speedup = swPlinq.Elapsed.TotalMilliseconds > 0
            ? swLinq.Elapsed.TotalMilliseconds / swPlinq.Elapsed.TotalMilliseconds
            : 0;

        Console.WriteLine($"--- Важка обчислювальна операція | Розмір: {size:N0} елементів ---");
        Console.WriteLine($"  LINQ:        {swLinq.Elapsed.TotalMilliseconds,9:F2} ms (знайдено простих: {linqResult.Count:N0})");
        Console.WriteLine($"  PLINQ:       {swPlinq.Elapsed.TotalMilliseconds,9:F2} ms (знайдено простих: {plinqResult.Count:N0})");
        Console.WriteLine($"  Прискорення: {speedup,9:F2}x\n");
    }

    public static void RunLightweightOverheadBenchmark(int size)
    {
        var data = MathWorkload.GenerateData(size);

        var swLinq = Stopwatch.StartNew();
        var linqResult = data
            .Where(x => x % 2 == 0)
            .Select(x => x + 1)
            .ToList();
        swLinq.Stop();

        var swPlinq = Stopwatch.StartNew();
        var plinqResult = data
            .AsParallel()
            .Where(x => x % 2 == 0)
            .Select(x => x + 1)
            .ToList();
        swPlinq.Stop();

        Console.WriteLine($"--- Тривіальна операція (x % 2 == 0) | Розмір: {size:N0} елементів ---");
        Console.WriteLine($"  LINQ:  {swLinq.Elapsed.TotalMilliseconds,9:F2} ms (елементів: {linqResult.Count:N0})");
        Console.WriteLine($"  PLINQ: {swPlinq.Elapsed.TotalMilliseconds,9:F2} ms (елементів: {plinqResult.Count:N0})\n");
    }
}