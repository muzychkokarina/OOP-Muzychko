using System;
using System.IO;

namespace lab18v13
{
    public class FileProcessor
    {
        private int _attemptCount = 0;

        public double ReadSensorData(string path)
        {
            _attemptCount++;

            if (path.Contains("forbidden", StringComparison.OrdinalIgnoreCase))
            {
                throw new UnauthorizedAccessException($"Доступ до файлу '{path}' заборонено.");
            }

            if (_attemptCount <= 2)
            {
                throw new IOException($"Файл '{path}' заблокований іншим процесом (спроба {_attemptCount}).");
            }

            return 23.85; 
        }
    }
}