using System;
using System.Threading;

namespace lab18v13
{
    public static class RetryHelper
    {
        public static T ExecuteWithRetry<T>(
            Func<T> operation,
            int retryCount = 3,
            TimeSpan initialDelay = default,
            Func<Exception, bool>? shouldRetry = null)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            if (initialDelay == default)
                initialDelay = TimeSpan.FromMilliseconds(200);

            int attempt = 0;

            while (true)
            {
                try
                {
                    attempt++;
                    Console.WriteLine($"[RetryHelper] Виконання спроби #{attempt}...");
                    T result = operation();
                    Console.WriteLine($"[RetryHelper] Успіх на спробі #{attempt}!");
                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[RetryHelper] Помилка ({ex.GetType().Name}): {ex.Message}");

                    bool canRetry = shouldRetry == null || shouldRetry(ex);

                    if (!canRetry)
                    {
                        Console.WriteLine($"[RetryHelper] Виняток {ex.GetType().Name} не підлягає повтору (shouldRetry = false). Переривання.");
                        throw;
                    }

                    if (attempt > retryCount)
                    {
                        Console.WriteLine($"[RetryHelper] Вичерпано ліміт повторних спроб ({retryCount}). Переривання.");
                        throw;
                    }

                    double delayMs = initialDelay.TotalMilliseconds * Math.Pow(2, attempt - 1);
                    TimeSpan delay = TimeSpan.FromMilliseconds(delayMs);

                    Console.WriteLine($"[RetryHelper] Очікування {delay.TotalMilliseconds:F0} мс перед повторною спробою #{attempt + 1}...");
                    Thread.Sleep(delay);
                }
            }
        }
    }
}