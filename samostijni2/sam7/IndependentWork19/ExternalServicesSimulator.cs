namespace IndependentWork19;

public class ExternalServicesSimulator
{
    private int _paymentAttempts;
    private int _warehouseCalls;
    private int _shippingCalls;

    public string ChargePaymentGateway(string orderId, decimal amount)
    {
        _paymentAttempts++;
        if (_paymentAttempts <= 2)
        {
            throw new HttpRequestException($"Платіжний шлюз тимчасово недоступний (HTTP 503). Спроба #{_paymentAttempts}");
        }
        return $"TRX-{orderId}-{Guid.NewGuid().ToString()[..6].ToUpper()}";
    }

    public string CheckWarehouseInventory(string sku)
    {
        _warehouseCalls++;
        if (_warehouseCalls <= 4)
        {
            throw new TimeoutException($"База даних складу не відповідає (виклик #{_warehouseCalls})");
        }
        return $"Товар {sku}: в наявності 15 од.";
    }

    public string CalculateDeliveryRoute(string city, CancellationToken token)
    {
        _shippingCalls++;
        if (_shippingCalls == 1)
        {
            Task.Delay(TimeSpan.FromSeconds(3), token).GetAwaiter().GetResult();
        }
        return $"Маршрут до м. {city}: доставка за 24 год (вартість 120 грн)";
    }
}