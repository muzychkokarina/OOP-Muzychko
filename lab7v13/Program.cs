using System;
using System.Text;

namespace lab7v13
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== Лабораторна робота №7: Generics & IRepository ===\n");

            var repository = new InMemoryRepository<City>();

            repository.Add(new City { Id = 1, Name = "Рівне", Country = "Україна", Population = 245000 });
            repository.Add(new City { Id = 2, Name = "Київ", Country = "Україна", Population = 2950000 });

            Console.WriteLine("--- Список після додавання: ---");
            foreach (var city in repository.GetAll())
            {
                Console.WriteLine($"ID: {city.Id} | Місто: {city.Name} | Країна: {city.Country} | Населення: {city.Population}");
            }

            var updatedCity = new City { Id = 1, Name = "Рівне (Оновлено)", Country = "Україна", Population = 250000 };
            repository.Update(updatedCity);

            Console.WriteLine("\n--- Після оновлення ID = 1: ---");
            var city1 = repository.GetById(1);
            Console.WriteLine($"ID: {city1?.Id} | Місто: {city1?.Name} | Населення: {city1?.Population}");

            repository.Delete(2);
            Console.WriteLine("\n--- Після видалення ID = 2: ---");
            Console.WriteLine($"Кількість елементів у репозиторії: {((System.Collections.Generic.List<City>)repository.GetAll()).Count}");
        }
    }
}