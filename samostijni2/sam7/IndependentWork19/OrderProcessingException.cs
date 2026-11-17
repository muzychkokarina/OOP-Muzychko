namespace IndependentWork19;

public class OrderProcessingException : Exception
{
    public string OrderId { get; } = string.Empty;
    public decimal OrderAmount { get; }
    public string FailureReason { get; } = string.Empty;

    public OrderProcessingException()
    {
    }

    public OrderProcessingException(string message)
        : base(message)
    {
    }

    public OrderProcessingException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public OrderProcessingException(string message, string orderId, decimal orderAmount, string failureReason)
        : base(message)
    {
        OrderId = orderId;
        OrderAmount = orderAmount;
        FailureReason = failureReason;
    }

    public OrderProcessingException(string message, string orderId, decimal orderAmount, string failureReason, Exception innerException)
        : base(message, innerException)
    {
        OrderId = orderId;
        OrderAmount = orderAmount;
        FailureReason = failureReason;
    }
}