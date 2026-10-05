using System;

namespace sam1
{
    public class Book
    {
        private string _title;
        private int _pageCount;

        public string Title
        {
            get { return _title; }
        }

        public Book(string title, int pageCount)
        {
            _title = title;
            _pageCount = pageCount;
        }

        public double EstimateReadingTime(double minutesPerPage)
        {
            return (_pageCount * minutesPerPage) / 60.0;
        }
    }

    public class BankAccount
    {
        private string _accountNumber;
        private double _balance;

        public string AccountNumber => _accountNumber;
        public double Balance => _balance;

        public BankAccount(string accountNumber, double initialBalance)
        {
            _accountNumber = accountNumber;
            _balance = initialBalance;
        }

        public void Deposit(double amount)
        {
            if (amount > 0)
            {
                _balance += amount;
                Console.WriteLine($"[BankAccount] Поповнено на {amount} грн. Новий баланс: {_balance} грн.");
            }
        }
    }

    public class Course
    {
        private string _title;
        private int _maxStudents;
        private int _enrolledStudents;

        public string Title => _title;
        public int EnrolledStudents => _enrolledStudents;

        public Course(string title, int maxStudents)
        {
            _title = title;
            _maxStudents = maxStudents;
            _enrolledStudents = 0;
        }

        public bool EnrollStudent()
        {
            if (_enrolledStudents < _maxStudents)
            {
                _enrolledStudents++;
                return true;
            }
            return false;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== 1. КЛАС BOOK ===");
            Book book = new Book("Чистий код", 464);
            double hoursNeeded = book.EstimateReadingTime(1.5);
            Console.WriteLine($"Книга: '{book.Title}'");
            Console.WriteLine($"Орієнтовний час читання: {hoursNeeded:F1} годин.");

            Console.WriteLine("\n=== 2. КЛАС BANKACCOUNT ===");
            BankAccount account = new BankAccount("UA1234567890", 1500.0);
            Console.WriteLine($"Рахунок: {account.AccountNumber}, Баланс: {account.Balance} грн.");
            account.Deposit(500.0);

            Console.WriteLine("\n=== 3. КЛАС COURSE ===");
            Course course = new Course("Об'єктно-орієнтоване програмування", 2);
            Console.WriteLine($"Курс: '{course.Title}'");
            Console.WriteLine($"Зараховано студента 1: {course.EnrollStudent()}");
            Console.WriteLine($"Зараховано студента 2: {course.EnrollStudent()}");
            Console.WriteLine($"Зараховано студента 3 (понад ліміт): {course.EnrollStudent()}");
            Console.WriteLine($"Всього зараховано: {course.EnrolledStudents}");
        }
    }
}