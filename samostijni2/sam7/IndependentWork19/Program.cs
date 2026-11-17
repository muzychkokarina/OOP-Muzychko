using System.Text;
using IndependentWork19;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

var simulator = new ExternalServicesSimulator();

ResilienceScenarios.RunCustomExceptionDemo();
ResilienceScenarios.RunPaymentRetryScenario(simulator);
ResilienceScenarios.RunWarehouseCircuitBreakerScenario(simulator);
ResilienceScenarios.RunShippingTimeoutAndFallbackScenario(simulator);