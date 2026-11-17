using System.Text;
using IndependentWork14;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

var retryPolicy = new RetryPolicy(
    maxRetries: 4,
    initialDelay: TimeSpan.FromMilliseconds(300),
    jitterFactor: 0.25);

var simulator = new ServiceSimulator();

Console.WriteLine("=====================================================================");
Console.WriteLine("  СЦЕНАРІЙ 1: Тимчасовий збій -> Успіх після кількох спроб");
Console.WriteLine("=====================================================================\n");

try
{
    string rate = retryPolicy.Execute(
        () => simulator.FetchExchangeRate("USD/UAH"),
        ex => ex is HttpRequestException || ex is TimeoutException);

    Console.WriteLine($"\nРезультат операції: {rate}\n");
}
catch (Exception ex)
{
    Console.WriteLine($"\nНеочікувана остаточна помилка: {ex.Message}\n");
}

Console.WriteLine("=====================================================================");
Console.WriteLine("  СЦЕНАРІЙ 2: Постійний збій -> Вичерпання maxRetries і виняток");
Console.WriteLine("=====================================================================\n");

try
{
    int savedRows = retryPolicy.Execute(
        () => simulator.SaveReportToDeadDatabase("AnnualReport2026"),
        ex => ex is HttpRequestException || ex is TimeoutException);

    Console.WriteLine($"\nЗбережено рядків: {savedRows}\n");
}
catch (Exception ex)
{
    Console.WriteLine($"\n[Перехоплено в Main] Операція остаточно провалилася: {ex.GetType().Name} — {ex.Message}\n");
}

Console.WriteLine("=====================================================================");
Console.WriteLine("  СЦЕНАРІЙ 3: Фільтрація через shouldRetry (негайний вихід для FormatException)");
Console.WriteLine("=====================================================================\n");

try
{
    string config = retryPolicy.Execute(
        () => simulator.ParseInvalidConfig("{broken_json}"),
        ex => ex is HttpRequestException || ex is TimeoutException);

    Console.WriteLine($"\nКонфігурація: {config}\n");
}
catch (Exception ex)
{
    Console.WriteLine($"\n[Перехоплено в Main] Повтор скасовано делегатом shouldRetry: {ex.GetType().Name} — {ex.Message}\n");
}