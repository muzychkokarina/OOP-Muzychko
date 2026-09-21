using System;

namespace lab2v13
{
    public class Library
    {
        private string _name;
        private string _address;
        private int _booksCount;

        public string Name => _name;
        public string Address => _address;
        public int BooksCount => _booksCount;

        public Library(string name, string address, int initialBooksCount)
        {
            _name = string.IsNullOrWhiteSpace(name) ? "Невідома бібліотека" : name;
            _address = string.IsNullOrWhiteSpace(address) ? "Адреса не вказана" : address;
            _booksCount = initialBooksCount >= 0 ? initialBooksCount : 0;

            Console.WriteLine($"[Конструктор 1] Створено бібліотеку '{_name}' ({_address}) з {_booksCount} книгами.");
        }

    
        public Library(string name, string address) : this(name, address, 0)
        {
            Console.WriteLine($"[Конструктор 2] Викликано перевантажений конструктор (початкова кількість книг: 0).");
        }

        public void AddBook(string bookTitle)
        {
            _booksCount++;
            Console.WriteLine($"У бібліотеку '{_name}' додано книгу: \"{bookTitle}\". Всього книг: {_booksCount}");
        }

        ~Library()
        {
            Console.WriteLine($"[Деструктор] Об'єкт бібліотеки '{_name}' знищено збирачем сміття.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Creating objects ===");

            Library? lib1 = new Library("Центральна міська", "вул. Соборна, 10", 1500);
            lib1.AddBook("Кобзар");

            Console.WriteLine();

            Library? lib2 = new Library("Наукова бібліотека", "вул. Студентська, 5");
            lib2.AddBook("Об’єктно-орієнтоване програмування в C#");

            Console.WriteLine("=== Objects created ===");
            Console.WriteLine();

            Console.WriteLine("=== End of Main, preparing for GC ===");
            
            lib1 = null;
            lib2 = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}