using System;

namespace lab4v13
{
    public class Furniture
    {
        public string Material { get; set; }
        public double Weight { get; set; }

        public Furniture(string material, double weight)
        {
            Material = material;
            Weight = weight;
        }

        public virtual void DisplayInfo()
        {
            Console.WriteLine($"[Меблі] Матеріал: {Material}, Вага: {Weight} кг");
        }

        public void ShowType()
        {
            Console.WriteLine("Це об'єкт класу Furniture");
        }
    }

    public class Chair : Furniture
    {
        public bool HasArmrests { get; set; }

        public Chair(string material, double weight, bool hasArmrests) 
            : base(material, weight)
        {
            HasArmrests = hasArmrests;
        }

        public override void DisplayInfo()
        {
            string armrests = HasArmrests ? "так" : "ні";
            Console.WriteLine($"[Стілець] Матеріал: {Material}, Вага: {Weight} кг, Підлокітники: {armrests}");
        }

        public new void ShowType()
        {
            Console.WriteLine("Це об'єкт класу Chair");
        }
    }

    public class Table : Furniture
    {
        public int LegsCount { get; set; }

        public Table(string material, double weight, int legsCount) 
            : base(material, weight)
        {
            LegsCount = legsCount;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"[Стіл] Матеріал: {Material}, Вага: {Weight} кг, Кількість ніжок: {LegsCount}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== 1. Створення об'єктів та демонстрація base ===");
            Furniture basicItem = new Furniture("Дерево", 12.0);
            Chair myChair = new Chair("Дуб", 5.5, true);
            Table myTable = new Table("Скло", 18.0, 4);

            basicItem.DisplayInfo();
            myChair.DisplayInfo();
            myTable.DisplayInfo();

            Console.WriteLine("\n=== 2. Демонстрація Поліморфізму (virtual / override) ===");
            Furniture polyItem = new Chair("Пластик", 3.0, false);
            polyItem.DisplayInfo();

            Console.WriteLine("\n=== 3. Демонстрація різниці між override та new ===");
            Chair chairObj = new Chair("Метал", 4.0, true);
            Furniture refToChair = chairObj;

            Console.WriteLine("— Демонстрація new:");
            refToChair.ShowType();
            chairObj.ShowType();

            Console.WriteLine("— Демонстрація override:");
            refToChair.DisplayInfo();
            chairObj.DisplayInfo();
        }
    }
}