using System.Collections.Generic;

namespace lab17v13
{
    public class CarOption
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public override string ToString() => $"{Name} (+${Price:N0})";
    }

    public class Car
    {
        public int Id { get; set; }
        public string Model { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public List<CarOption> Options { get; set; } = new();

        public override string ToString() =>
            $"[ID: {Id}] {Model} (База: ${BasePrice:N0}, Опцій: {Options.Count})";
    }

    public class Dealership
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Car> Cars { get; set; } = new();
    }
}