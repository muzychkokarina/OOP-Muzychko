using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lab17v13
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            List<Dealership> dealerships = SeedData();

            Console.WriteLine("=== ЛАБОРАТОРНА РОБОТА №11 (Варіант 13: Автосалони та автомобілі) ===\n");

            Console.WriteLine("--- 1. SelectMany: Усі опції всіх автомобілів у всіх автосалонах ---");
            var allOptions = dealerships
                .SelectMany(d => d.Cars)
                .SelectMany(c => c.Options)
                .ToList();

            Console.WriteLine($"Загальна кількість встановлених опцій: {allOptions.Count}");
            foreach (var option in allOptions)
            {
                Console.WriteLine($"  • [ID: {option.Id}] {option.Name} — ${option.Price:N0}");
            }

            Console.WriteLine("\n--- 2. Пагінація: Усі автомобілі (по 5 шт. на сторінку) ---");
            var allCars = dealerships.SelectMany(d => d.Cars).ToList();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling((double)allCars.Count / pageSize);

            for (int pageNumber = 1; pageNumber <= totalPages; pageNumber++)
            {
                Console.WriteLine($"\n[Сторінка {pageNumber} з {totalPages}]");
                var pagedCars = allCars
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);

                foreach (var car in pagedCars)
                {
                    Console.WriteLine($"  {car}");
                }
            }

            Console.WriteLine("\n--- 3. Custom Extension: Повна вартість автомобілів (TotalCarPrice) ---");
            foreach (var dealership in dealerships)
            {
                Console.WriteLine($"\nАвтосалон: {dealership.Name}");
                foreach (var car in dealership.Cars)
                {
                    Console.WriteLine($"  • {car.Model}: Базова ціна = ${car.BasePrice:N0} | З опціями = ${car.TotalCarPrice():N0}");
                }
            }
        }

        private static List<Dealership> SeedData()
        {
            return new List<Dealership>
            {
                new Dealership
                {
                    Id = 1,
                    Name = "AutoLux Київ",
                    Cars = new List<Car>
                    {
                        new Car
                        {
                            Id = 101, Model = "BMW X5", BasePrice = 65000,
                            Options = new List<CarOption>
                            {
                                new() { Id = 1, Name = "M-Пакет", Price = 4500 },
                                new() { Id = 2, Name = "Панорамний дах", Price = 2000 }
                            }
                        },
                        new Car
                        {
                            Id = 102, Model = "Audi Q7", BasePrice = 62000,
                            Options = new List<CarOption>
                            {
                                new() { Id = 3, Name = "Пневмопідвіска", Price = 3200 },
                                new() { Id = 4, Name = "Акустика Bang & Olufsen", Price = 2800 }
                            }
                        },
                        new Car
                        {
                            Id = 103, Model = "Mercedes-Benz E-Class", BasePrice = 58000,
                            Options = new List<CarOption>
                            {
                                new() { Id = 5, Name = "Шкіряний салон Nappa", Price = 3500 }
                            }
                        },
                        new Car
                        {
                            Id = 104, Model = "Volkswagen Touareg", BasePrice = 54000,
                            Options = new List<CarOption>
                            {
                                new() { Id = 6, Name = "Матричні фари IQ.Light", Price = 1800 },
                                new() { Id = 7, Name = "Нічне бачення", Price = 2200 }
                            }
                        }
                    }
                },
                new Dealership
                {
                    Id = 2,
                    Name = "WestMotors Львів",
                    Cars = new List<Car>
                    {
                        new Car
                        {
                            Id = 201, Model = "Toyota Camry", BasePrice = 34000,
                            Options = new List<CarOption>
                            {
                                new() { Id = 8, Name = "Підігрів керма та сидінь", Price = 900 },
                                new() { Id = 9, Name = "Камери 360°", Price = 1200 }
                            }
                        },
                        new Car
                        {
                            Id = 202, Model = "Lexus RX 350", BasePrice = 56000,
                            Options = new List<CarOption>
                            {
                                new() { Id = 10, Name = "Аудіосистема Mark Levinson", Price = 2500 },
                                new() { Id = 11, Name = "Проєкційний дисплей", Price = 1400 }
                            }
                        },
                        new Car
                        {
                            Id = 203, Model = "Mazda CX-5", BasePrice = 31000,
                            Options = new List<CarOption>
                            {
                                new() { Id = 12, Name = "Колір Soul Red Crystal", Price = 700 }
                            }
                        },
                        new Car
                        {
                            Id = 204, Model = "Honda CR-V Hybrid", BasePrice = 37000,
                            Options = new List<CarOption>
                            {
                                new() { Id = 13, Name = "Електропривод багажника", Price = 800 },
                                new() { Id = 14, Name = "Адаптивний круїз-контроль", Price = 1100 }
                            }
                        }
                    }
                },
                new Dealership
                {
                    Id = 3,
                    Name = "RivneDrive Авто",
                    Cars = new List<Car>
                    {
                        new Car
                        {
                            Id = 301, Model = "Skoda Octavia A8", BasePrice = 26000,
                            Options = new List<CarOption>
                            {
                                new() { Id = 15, Name = "Цифрова панель приладів", Price = 650 },
                                new() { Id = 16, Name = "Спортивні сидіння", Price = 950 }
                            }
                        },
                        new Car
                        {
                            Id = 302, Model = "Hyundai Tucson", BasePrice = 29500,
                            Options = new List<CarOption>
                            {
                                new() { Id = 17, Name = "Вентиляція сидінь", Price = 1100 }
                            }
                        },
                        new Car
                        {
                            Id = 303, Model = "Kia Sportage", BasePrice = 28500,
                            Options = new List<CarOption>
                            {
                                new() { Id = 18, Name = "Панорамний люк", Price = 1300 },
                                new() { Id = 19, Name = "Бездротова зарядка", Price = 400 }
                            }
                        },
                        new Car
                        {
                            Id = 304, Model = "Volvo XC60", BasePrice = 53000,
                            Options = new List<CarOption>
                            {
                                new() { Id = 20, Name = "Пакет безпеки Pilot Assist", Price = 2400 },
                                new() { Id = 21, Name = "Кришталевий селектор Orrefors", Price = 850 }
                            }
                        }
                    }
                }
            };
        }
    }
}