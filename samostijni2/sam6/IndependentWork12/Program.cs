using System.Text;
using IndependentWork12;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

Console.WriteLine("=====================================================================");
Console.WriteLine($"  ЧАСТИНА 1: Порівняння продуктивності LINQ vs PLINQ (Ядер CPU: {Environment.ProcessorCount})");
Console.WriteLine("=====================================================================\n");

int[] sizes = { 1_000_000, 5_000_000, 10_000_000 };
foreach (var size in sizes)
{
    PerformanceBenchmark.RunHeavyBenchmark(size);
}

Console.WriteLine("=====================================================================");
Console.WriteLine("  ЧАСТИНА 2: Коли PLINQ програє LINQ (накладні витрати на тривіальних задачах)");
Console.WriteLine("=====================================================================\n");

PerformanceBenchmark.RunLightweightOverheadBenchmark(100_000);
PerformanceBenchmark.RunLightweightOverheadBenchmark(1_000_000);

ThreadSafetyDemo.RunSideEffectsExperiment();