using Polly;
using Polly.CircuitBreaker;
using Polly.Fallback;
using Polly.Retry;
using Polly.Timeout;

namespace IndependentWork19;

public static class ResilienceScenarios
{
    public static void RunCustomExceptionDemo()
    {
        Console.WriteLine("=====================================================================");
        Console.WriteLine("  ЧАСТИНА 1: Демонстрація власного типу винятку OrderProcessingException");
        Console.WriteLine("=====================================================================\n");

        try
        {
            ProcessOrder("ORD-2026-101", 4500m, isVerifiedCustomer: false);
        }
        catch (OrderProcessingException ex)
        {
            Console.WriteLine($"[Перехоплено бізнес-виняток]: {ex.Message}");
            Console.WriteLine($"  Номер замовлення: {ex.OrderId}");
            Console.WriteLine($"  Сума замовлення:  {ex.OrderAmount:N2} грн");
            Console.WriteLine($"  Причина відмови:  {ex.FailureReason}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Загальна помилка]: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("  Блок finally: аудит спроби оформлення замовлення завершено.\n");
        }
    }

    private static void ProcessOrder(string orderId, decimal amount, bool isVerifiedCustomer)
    {
        if (!isVerifiedCustomer)
        {
            throw new OrderProcessingException(
                "Неможливо оформити замовлення на велику суму без верифікації клієнта.",
                orderId,
                amount,
                "CustomerNotVerified");
        }
    }

    public static void RunPaymentRetryScenario(ExternalServicesSimulator simulator)
    {
        Console.WriteLine("=====================================================================");
        Console.WriteLine("  СЦЕНАРІЙ 1 (Polly Retry + Exponential Backoff + Jitter)");
        Console.WriteLine("  Проблема: Тимчасові збої HTTP 503 при зверненні до платіжного шлюзу");
        Console.WriteLine("=====================================================================\n");

        var pipeline = new ResiliencePipelineBuilder<string>()
            .AddRetry(new RetryStrategyOptions<string>
            {
                ShouldHandle = new PredicateBuilder<string>().Handle<HttpRequestException>(),
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromMilliseconds(400),
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                OnRetry = args =>
                {
                    Console.WriteLine($"  [Retry #{args.AttemptNumber + 1}] Помилка: {args.Outcome.Exception?.Message}. " +
                                      $"Наступна спроба через {args.RetryDelay.TotalMilliseconds:F0} мс...");
                    return default;
                }
            })
            .Build();

        try
        {
            string transactionId = pipeline.Execute(() => simulator.ChargePaymentGateway("ORD-505", 1890m));
            Console.WriteLine($"  [Успіх] Платіж проведено. ID транзакції: {transactionId}\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  [Критична невдача] Усі спроби вичерпано: {ex.Message}\n");
        }
    }

    public static void RunWarehouseCircuitBreakerScenario(ExternalServicesSimulator simulator)
    {
        Console.WriteLine("=====================================================================");
        Console.WriteLine("  СЦЕНАРІЙ 2 (Polly Circuit Breaker)");
        Console.WriteLine("  Проблема: Тривале падіння БД складу (захист від каскадних відмов)");
        Console.WriteLine("=====================================================================\n");

        var pipeline = new ResiliencePipelineBuilder<string>()
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions<string>
            {
                ShouldHandle = new PredicateBuilder<string>().Handle<TimeoutException>(),
                FailureRatio = 0.5,
                SamplingDuration = TimeSpan.FromSeconds(10),
                MinimumThroughput = 2,
                BreakDuration = TimeSpan.FromSeconds(2),
                OnOpened = args =>
                {
                    Console.WriteLine($"  [Circuit OPEN] Ланцюг розімкнено на {args.BreakDuration.TotalSeconds} с через серію збоїв!");
                    return default;
                },
                OnClosed = _ =>
                {
                    Console.WriteLine("  [Circuit CLOSED] Роботу сервісу відновлено, ланцюг замкнено.");
                    return default;
                },
                OnHalfOpened = _ =>
                {
                    Console.WriteLine("  [Circuit HALF-OPEN] Пробний запит для перевірки стану БД...");
                    return default;
                }
            })
            .Build();

        for (int i = 1; i <= 5; i++)
        {
            try
            {
                Console.WriteLine($"  Запит #{i} до бази даних складу:");
                string status = pipeline.Execute(() => simulator.CheckWarehouseInventory("LAPTOP-PRO-15"));
                Console.WriteLine($"    [Успіх] {status}");
            }
            catch (BrokenCircuitException)
            {
                Console.WriteLine("    [Миттєва відмова] Виклик заблоковано Circuit Breaker (сервіс на паузі).");
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"    [Таймаут БД] {ex.Message}");
            }

            if (i == 4)
            {
                Console.WriteLine("  ...Очікування 2.2 секунди для переходу Circuit Breaker у стан Half-Open...");
                Thread.Sleep(2200);
            }
        }
        Console.WriteLine();
    }

    public static void RunShippingTimeoutAndFallbackScenario(ExternalServicesSimulator simulator)
    {
        Console.WriteLine("=====================================================================");
        Console.WriteLine("  СЦЕНАРІЙ 3 (Комбінація Polly: Fallback + Timeout)");
        Console.WriteLine("  Проблема: Зависання API служби доставки та резервний розрахунок");
        Console.WriteLine("=====================================================================\n");

        var pipeline = new ResiliencePipelineBuilder<string>()
            .AddFallback(new FallbackStrategyOptions<string>
            {
                ShouldHandle = new PredicateBuilder<string>().Handle<TimeoutRejectedException>(),
                FallbackAction = args =>
                {
                    Console.WriteLine("  [Fallback] Зовнішній сервіс доставки перевищив час очікування. Застосовано базовий тариф.");
                    return ValueTask.FromResult(Outcome.FromResult("Стандартний тариф доставки: 150 грн (орієнтовно 2-3 дні)"));
                }
            })
            .AddTimeout(new TimeoutStrategyOptions
            {
                Timeout = TimeSpan.FromSeconds(1),
                OnTimeout = args =>
                {
                    Console.WriteLine($"  [Timeout] Запит скасовано після {args.Timeout.TotalSeconds} с очікування!");
                    return default;
                }
            })
            .Build();

        Console.WriteLine("  Виклик #1 (повільна відповідь сервісу доставки):");
        string route1 = pipeline.Execute(token => simulator.CalculateDeliveryRoute("Рівне", token));
        Console.WriteLine($"    Результат: {route1}\n");

        Console.WriteLine("  Виклик #2 (штатна швидка відповідь сервісу доставки):");
        string route2 = pipeline.Execute(token => simulator.CalculateDeliveryRoute("Рівне", token));
        Console.WriteLine($"    Результат: {route2}\n");
    }
}