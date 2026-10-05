using System;
using System.Collections.Generic;

namespace lab5v13
{
    public class Sensor
    {
        public string Location { get; set; }

        public Sensor(string location)
        {
            Location = location;
        }

        public virtual double GetReading()
        {
            return 0.0;
        }
    }

    public class TemperatureSensor : Sensor
    {
        public string Unit { get; set; }
        private double _temperature;

        public TemperatureSensor(string location, string unit, double temperature) 
            : base(location)
        {
            Unit = unit;
            _temperature = temperature;
        }

        public override double GetReading()
        {
            Console.WriteLine($"[Температурний сенсор] Локація: {Location}, Показник: {_temperature} °{Unit}");
            return _temperature;
        }
    }

    public class PressureSensor : Sensor
    {
        public double MaxPressure { get; set; }
        private double _pressure;

        public PressureSensor(string location, double maxPressure, double pressure) 
            : base(location)
        {
            MaxPressure = maxPressure;
            _pressure = pressure;
        }

        public override double GetReading()
        {
            Console.WriteLine($"[Сенсор тиску] Локація: {Location}, Показник: {_pressure} Бар (Макс: {MaxPressure} Бар)");
            return _pressure;
        }
    }

    public class LightSensor : Sensor
    {
        public string LuxRange { get; set; }
        private double _lightLevel;

        public LightSensor(string location, string luxRange, double lightLevel) 
            : base(location)
        {
            LuxRange = luxRange;
            _lightLevel = lightLevel;
        }

        public override double GetReading()
        {
            Console.WriteLine($"[Сенсор освітленості] Локація: {Location}, Показник: {_lightLevel} Люкс (Діапазон: {LuxRange})");
            return _lightLevel;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Sensor> sensors = new List<Sensor>
            {
                new TemperatureSensor("Цех №1", "C", 23.5),
                new PressureSensor("Котельня", 10.0, 4.2),
                new LightSensor("Офіс 204", "0-1000", 450.0),
                new TemperatureSensor("Серверна", "C", 18.0)
            };

            Console.WriteLine("=== ЗЧИТУВАННЯ ПОКАЗНИКІВ СЕНСОРІВ (ПОЛІМОРФІЗМ) ===");
            double sumReadings = 0;

            foreach (Sensor sensor in sensors)
            {
                sumReadings += sensor.GetReading();
            }

            double averageReading = sumReadings / sensors.Count;

            Console.WriteLine("\n=== АГРЕГАЦІЯ РЕЗУЛЬТАТІВ ===");
            Console.WriteLine($"Загальна кількість сенсорів: {sensors.Count}");
            Console.WriteLine($"Середнє значення показників усіх сенсорів: {averageReading:F2}");
        }
    }
}