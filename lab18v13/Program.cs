using System;
using System.IO;
using System.Net.Http;
using System.Text;

namespace lab18v13
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== ЛАБОРАТОРНА РОБОТА №12 (Варіант 13: Читання даних з сенсора) ===\n");

            Func<Exception, bool> retryPolicy = ex =>
                ex is IOException || ex is HttpRequestException;

           
            Console.WriteLine("--- 1. Читання даних локального сенсора (FileProcessor) ---");
            var fileProcessor = new FileProcessor();
            try
            {
                double localSensorValue = RetryHelper.ExecuteWithRetry(
                    operation: () => fileProcessor.ReadSensorData("sensors/temp_01.dat"),
                    retryCount: 3,
                    initialDelay: TimeSpan.FromMilliseconds(200),
                    shouldRetry: retryPolicy
                );
                Console.WriteLine($"Результат з файлу: {localSensorValue} °C\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Критичний збій читання файлу: {ex.Message}\n");
            }

           
            Console.WriteLine("--- 2. Отримання даних віддаленого сенсора (NetworkClient) ---");
            var networkClient = new NetworkClient();
            try
            {
                double remoteSensorValue = RetryHelper.ExecuteWithRetry(
                    operation: () => networkClient.GetRemoteSensorData("https://api.sensors.local/v1/humidity"),
                    retryCount: 4,
                    initialDelay: TimeSpan.FromMilliseconds(150),
                    shouldRetry: retryPolicy
                );
                Console.WriteLine($"Результат з мережі: {remoteSensorValue} %\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Критичний мережевий збій: {ex.Message}\n");
            }

            
            Console.WriteLine("--- 3. Перевірка вибірковості shouldRetry (UnauthorizedAccessException) ---");
            var restrictedProcessor = new FileProcessor();
            try
            {
                RetryHelper.ExecuteWithRetry(
                    operation: () => restrictedProcessor.ReadSensorData("sensors/forbidden_system.dat"),
                    retryCount: 3,
                    initialDelay: TimeSpan.FromMilliseconds(200),
                    shouldRetry: retryPolicy
                );
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Операцію негайно зупинено без повторів: {ex.Message}");
            }
        }
    }
}