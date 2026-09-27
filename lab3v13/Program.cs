using System;

namespace lab3v13
{
    public class CustomThreadPool : IDisposable
    {
        private bool _disposed = false;
        private int _poolSize;
        private bool _isActive;

        public int PoolSize => _poolSize;
        public bool IsActive => _isActive;

        public CustomThreadPool(int poolSize)
        {
            _poolSize = poolSize > 0 ? poolSize : 1;
            _isActive = true;
            Console.WriteLine($"[Пул потоків] Ініціалізовано {_poolSize} потоків. Ресурси виділено.");
        }

        public void ExecuteTask(string taskName)
        {
            if (_disposed || !_isActive)
            {
                Console.WriteLine($"[Помилка] Неможливо виконати завдання '{taskName}': пул потоків зупинено або звільнено.");
                return;
            }

            Console.WriteLine($"[Пул потоків] Виконується завдання: '{taskName}' на одному з {_poolSize} потоків.");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Console.WriteLine("[Dispose] Звільнення керованих ресурсів...");
                }

                if (_isActive)
                {
                    Console.WriteLine($"[Dispose] Завершення всіх {_poolSize} потоків та звільнення ресурсів системного пулу.");
                    _isActive = false;
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); 
        }

        ~CustomThreadPool()
        {
            Console.WriteLine("[~Деструктор] Автоматичне звільнення ресурсів через Garbage Collector!");
            Dispose(false);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== СЦЕНАРІЙ 1: Використання через оператор using ===");
            using (CustomThreadPool pool1 = new CustomThreadPool(4))
            {
                pool1.ExecuteTask("Обробка зображень");
                pool1.ExecuteTask("Завантаження файлів");
            } 
            Console.WriteLine();

            Console.WriteLine("=== СЦЕНАРІЙ 2: Явний виклик Dispose() без using ===");
            CustomThreadPool pool2 = new CustomThreadPool(8);
            pool2.ExecuteTask("Аналіз даних");
            pool2.Dispose(); 
            pool2.ExecuteTask("Спроба виконати нове завдання"); 
            Console.WriteLine();

            Console.WriteLine("=== СЦЕНАРІЙ 3: Об'єкт без Dispose() (робота деструктора) ===");
            CreateUnmanagedPool();

            Console.WriteLine("Примусовий запуск Garbage Collector...");
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine("\nПрограму завершено.");
        }

        static void CreateUnmanagedPool()
        {
            CustomThreadPool pool3 = new CustomThreadPool(2);
            pool3.ExecuteTask("Фонове завдання без Dispose");
        }
    }
}