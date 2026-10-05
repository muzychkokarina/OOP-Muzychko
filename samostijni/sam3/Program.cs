using System;
using System.Threading;

namespace sam3
{
    public class ThreadPool : IDisposable
    {
        private bool _disposed = false;
        private bool _isActive;
        private int _poolSize;

        public int PoolSize => _poolSize;
        public bool IsActive => _isActive;

        public ThreadPool(int poolSize)
        {
            _poolSize = poolSize;
            _isActive = true;
            Console.WriteLine($"[ThreadPool] Пул потоків розміром {_poolSize} ініціалізовано та активовано.");
        }

        public void ExecuteTask(string taskName)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ThreadPool), "Неможливо виконати завдання: пул потоків уже закритий!");
            }

            if (_isActive)
            {
                Console.WriteLine($"[ThreadPool] Виконання завдання '{taskName}' у пулі потоків...");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Console.WriteLine("[Dispose(true)] Звільнення керованих ресурсів пулу потоків.");
                }

                if (_isActive)
                {
                    Console.WriteLine("[Dispose] Зупинка та завершення всіх потоків...");
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

        ~ThreadPool()
        {
            Console.WriteLine("[Finalizer] Виклик деструктора ~ThreadPool().");
            Dispose(false);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== СЦЕНАРІЙ 1: Використання блоку using ===");
            using (ThreadPool pool1 = new ThreadPool(4))
            {
                pool1.ExecuteTask("Обробка даних 1");
            } 

            Console.WriteLine("\n=== СЦЕНАРІЙ 2: Явний виклик Dispose() ===");
            ThreadPool pool2 = new ThreadPool(8);
            pool2.ExecuteTask("Завантаження файлу");
            pool2.Dispose(); 
            pool2.Dispose(); 

            Console.WriteLine("\n=== СЦЕНАРІЙ 3: Робота деструктора (GC.Collect) ===");
            CreateUnmanagedPool();

            Console.WriteLine("Запуск GC.Collect()...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            
            Console.WriteLine("\nПрограму успішно завершено.");
        }

        static void CreateUnmanagedPool()
        {
            ThreadPool pool3 = new ThreadPool(2);
            pool3.ExecuteTask("Фонова задача без Dispose");
        }
    }
}