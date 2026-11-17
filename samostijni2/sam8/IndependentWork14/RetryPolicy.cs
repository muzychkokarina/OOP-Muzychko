namespace IndependentWork14;

public class RetryPolicy
{
    private readonly int _maxRetries;
    private readonly TimeSpan _initialDelay;
    private readonly double _jitterFactor;
    private readonly Random _random;

    public RetryPolicy(int maxRetries, TimeSpan initialDelay, double jitterFactor = 0.2)
    {
        if (maxRetries < 0)
            throw new ArgumentOutOfRangeException(nameof(maxRetries));
        if (initialDelay < TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(initialDelay));
        if (jitterFactor < 0 || jitterFactor > 1)
            throw new ArgumentOutOfRangeException(nameof(jitterFactor));

        _maxRetries = maxRetries;
        _initialDelay = initialDelay;
        _jitterFactor = jitterFactor;
        _random = new Random();
    }

    public T Execute<T>(Func<T> operation, Func<Exception, bool>? shouldRetry = null)
    {
        if (operation == null)
            throw new ArgumentNullException(nameof(operation));

        int retryAttempt = 0;

        while (true)
        {
            try
            {
                Console.WriteLine($"[Спроба #{retryAttempt + 1}] Виконання операції...");
                T result = operation();
                if (retryAttempt > 0)
                {
                    Console.WriteLine($"[Успіх] Операція завершилася успішно після {retryAttempt} повторних спроб.");
                }
                return result;
            }
            catch (Exception ex)
            {
                bool canRetry = retryAttempt < _maxRetries && (shouldRetry == null || shouldRetry(ex));

                if (!canRetry)
                {
                    Console.WriteLine($"[Відмова] Повтор не виконується (спроб використано: {retryAttempt}/{_maxRetries}). Причина: {ex.GetType().Name} — {ex.Message}");
                    throw;
                }

                double exponentialMultiplier = Math.Pow(2, retryAttempt);
                double jitterMultiplier = 1.0 + (_random.NextDouble() * _jitterFactor * 2.0 - _jitterFactor);
                double delayMs = _initialDelay.TotalMilliseconds * exponentialMultiplier * jitterMultiplier;
                var delay = TimeSpan.FromMilliseconds(Math.Max(0, delayMs));

                Console.WriteLine($"[Помилка] Спроба #{retryAttempt + 1} невдала: {ex.Message}");
                Console.WriteLine($"[Очікування] Повтор #{retryAttempt + 1} з {_maxRetries} через {delay.TotalMilliseconds:F0} мс...");

                Thread.Sleep(delay);
                retryAttempt++;
            }
        }
    }
}