namespace IndependentWork14;

public class ServiceSimulator
{
    private int _transientCallCount;
    private int _permanentCallCount;

    public string FetchExchangeRate(string currencyPair)
    {
        _transientCallCount++;
        if (_transientCallCount <= 3)
        {
            throw new HttpRequestException($"Сервер курсів валют тимчасово перевантажений (HTTP 503). Виклик #{_transientCallCount}");
        }
        return $"{currencyPair} = 41.35 UAH (отримано з 4-ї спроби)";
    }

    public int SaveReportToDeadDatabase(string reportName)
    {
        _permanentCallCount++;
        throw new TimeoutException($"Таймаут підключення до резервної БД для '{reportName}' (виклик #{_permanentCallCount})");
    }

    public string ParseInvalidConfig(string rawConfig)
    {
        throw new FormatException($"Некоректний формат конфігурації: '{rawConfig}' (повтор не має сенсу)");
    }
}