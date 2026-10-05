using System;

namespace sam2
{
    /*
    Проблеми процедурного підходу:
    1. Дані про один товар розкидані по трьох паралельних масивах (names, prices, quantities), що ускладнює їхній зв'язок.
    2. Відсутня гарантія однакової довжини масивів — помилка в індексах призведе до IndexOutOfRangeException.
    3. Логіку обчислень та знижок важко повторно використовувати або розширювати для інших сценаріїв кошика.
    */
    public static class ProceduralDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== ПРОЦЕДУРНИЙ ПІДХІД ===");

            string[] names = { "Ноутбук", "Мишка", "Монітор" };
            double[] prices = { 25000.0, 450.0, 7500.0 };
            int[] quantities = { 1, 2, 1 };

            double total = CalculateTotal(prices, quantities);
            Console.WriteLine($"Загальна сума кошика (з урахуванням знижки 10% на товари > 500 грн): {total:F2} грн\n");
        }

        public static double CalculateTotal(double[] prices, int[] quantities)
        {
            double sum = 0;
            for (int i = 0; i < prices.Length; i++)
            {
                double itemTotal = prices[i] * quantities[i];
                if (prices[i] > 500)
                {
                    itemTotal *= 0.90; // знижка 10%
                }
                Console.WriteLine($"Товар {i + 1}: Ціна = {prices[i]} грн, Кількість = {quantities[i]}, Сума зі знижкою = {itemTotal:F2} грн");
                sum += itemTotal;
            }
            return sum;
        }
    }
}