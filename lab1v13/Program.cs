using System;

namespace lab1v13
{
    public class Car
    {
        private string brand;
        private int year;
        private decimal price;

        public string Brand => brand;
        public int Year => year;

        public decimal Price
        {
            get => price;
            set => price = value >= 0 ? value : 0;
        }

        public Car(string brand, int year, decimal price)
        {
            this.brand = brand;
            this.year = year;
            Price = price;
        }

        public void Drive()
        {
            Console.WriteLine($"Автомобіль {brand} ({year} року, ціна: ${price:N0}) вирушив у дорогу!");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Лабораторна робота №1 (Варіант 13) ===\n");

            Car car1 = new Car("BMW", 2021, 35000);
            Car car2 = new Car("Audi", 2019, 28000);
            Car car3 = new Car("Tesla", 2023, 45000);

            car1.Drive();
            car2.Drive();
            car3.Drive();
        }
    }
}
