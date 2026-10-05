using System;

namespace sam2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ПОРІВНЯННЯ ПРОЦЕДУРНОГО ТА ОБ'ЄКТНОГО ПІДХОДІВ\n");

            ProceduralDemo.Run();

            ObjectOrientedDemo.Run();
        }
    }
}