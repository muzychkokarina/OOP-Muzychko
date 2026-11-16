using System.Net.Http;

namespace lab18v13
{
    public class NetworkClient
    {
        private int _attemptCount = 0;

        public double GetRemoteSensorData(string url)
        {
            _attemptCount++;

            if (_attemptCount <= 4)
            {
                throw new HttpRequestException($"Таймаут або помилка з'єднання з {url} (спроба {_attemptCount}).");
            }

            return 41.20; 
        }
    }
}