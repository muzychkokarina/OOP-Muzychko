using System.Diagnostics;

namespace IndependentWork11;

public class AnalyticsService
{
    private readonly List<Product> _products;
    private readonly List<Customer> _customers;
    private readonly List<Sale> _sales;

    public AnalyticsService(List<Product> products, List<Customer> customers, List<Sale> sales)
    {
        _products = products;
        _customers = customers;
        _sales = sales;
    }

    public void TopProductsByCategory()
    {
        var sw = Stopwatch.StartNew();

        var result = _sales
            .GroupBy(s => s.ProductId)
            .Select(g => new
            {
                ProductId = g.Key,
                TotalQuantity = g.Sum(s => s.Quantity)
            })
            .Join(
                _products,
                stat => stat.ProductId,
                p => p.Id,
                (stat, p) => new
                {
                    p.Name,
                    p.Category,
                    stat.TotalQuantity
                })
            .GroupBy(x => x.Category)
            .Select(catGroup => new
            {
                Category = catGroup.Key,
                TopProducts = catGroup
                    .OrderByDescending(p => p.TotalQuantity)
                    .Take(3)
                    .ToList()
            })
            .OrderBy(x => x.Category)
            .ToList();

        sw.Stop();

        Console.WriteLine("=====================================================================");
        Console.WriteLine("  ПАЙПЛАЙН 1: Найпопулярніші продукти за категоріями (ТОП-3)");
        Console.WriteLine($"  Час виконання: {sw.Elapsed.TotalMilliseconds:F4} ms ({sw.ElapsedTicks} ticks)");
        Console.WriteLine("=====================================================================");

        foreach (var group in result)
        {
            Console.WriteLine($"\nКатегорія: {group.Category}");
            int rank = 1;
            foreach (var prod in group.TopProducts)
            {
                Console.WriteLine($"  {rank}. {prod.Name} — продано {prod.TotalQuantity} од.");
                rank++;
            }
        }
        Console.WriteLine();
    }

    public void TotalRevenueByCustomerCity()
    {
        var sw = Stopwatch.StartNew();

        var result = _sales
            .Join(
                _products,
                s => s.ProductId,
                p => p.Id,
                (s, p) => new
                {
                    s.CustomerId,
                    Revenue = s.Quantity * p.UnitPrice
                })
            .Join(
                _customers,
                sp => sp.CustomerId,
                c => c.Id,
                (sp, c) => new
                {
                    c.City,
                    sp.Revenue
                })
            .GroupBy(x => x.City)
            .Select(g => new
            {
                City = g.Key,
                TotalRevenue = g.Sum(x => x.Revenue),
                OrdersCount = g.Count()
            })
            .OrderByDescending(x => x.TotalRevenue)
            .ToList();

        sw.Stop();

        Console.WriteLine("=====================================================================");
        Console.WriteLine("  ПАЙПЛАЙН 2: Загальний дохід за містами клієнтів");
        Console.WriteLine($"  Час виконання: {sw.Elapsed.TotalMilliseconds:F4} ms ({sw.ElapsedTicks} ticks)");
        Console.WriteLine("=====================================================================");

        foreach (var item in result)
        {
            Console.WriteLine($"  {item.City,-20} | Дохід: {item.TotalRevenue,12:N2} грн | Замовлень: {item.OrdersCount}");
        }
        Console.WriteLine();
    }

    public void SalesByYearAndMonth()
    {
        var sw = Stopwatch.StartNew();

        var result = _sales
            .Join(
                _products,
                s => s.ProductId,
                p => p.Id,
                (s, p) => new
                {
                    s.SaleDate.Year,
                    s.SaleDate.Month,
                    s.Quantity,
                    Revenue = s.Quantity * p.UnitPrice
                })
            .GroupBy(x => new { x.Year, x.Month })
            .Select(g => new
            {
                g.Key.Year,
                g.Key.Month,
                TotalQuantity = g.Sum(x => x.Quantity),
                TotalRevenue = g.Sum(x => x.Revenue)
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ToList();

        sw.Stop();

        Console.WriteLine("=====================================================================");
        Console.WriteLine("  ПАЙПЛАЙН 3: Динаміка продажів за роками та місяцями");
        Console.WriteLine($"  Час виконання: {sw.Elapsed.TotalMilliseconds:F4} ms ({sw.ElapsedTicks} ticks)");
        Console.WriteLine("=====================================================================");

        foreach (var row in result)
        {
            Console.WriteLine($"  {row.Year}-{row.Month:D2} | Продано одиниць: {row.TotalQuantity,5} | Дохід: {row.TotalRevenue,12:N2} грн");
        }
        Console.WriteLine();
    }

    public void TopVipCustomers()
    {
        var sw = Stopwatch.StartNew();

        var result = _sales
            .Where(s => s.Quantity >= 2)
            .Join(
                _products,
                s => s.ProductId,
                p => p.Id,
                (s, p) => new
                {
                    s.CustomerId,
                    s.Quantity,
                    Amount = s.Quantity * p.UnitPrice
                })
            .GroupBy(x => x.CustomerId)
            .Select(g => new
            {
                CustomerId = g.Key,
                TotalSpent = g.Sum(x => x.Amount),
                TotalItems = g.Sum(x => x.Quantity),
                AverageCheck = g.Average(x => x.Amount)
            })
            .Join(
                _customers,
                stat => stat.CustomerId,
                c => c.Id,
                (stat, c) => new
                {
                    c.Name,
                    c.City,
                    stat.TotalSpent,
                    stat.TotalItems,
                    stat.AverageCheck
                })
            .OrderByDescending(x => x.TotalSpent)
            .Take(5)
            .ToList();

        sw.Stop();

        Console.WriteLine("=====================================================================");
        Console.WriteLine("  ПАЙПЛАЙН 4 (додатковий): ТОП-5 VIP-клієнтів (замовлення від 2 од.)");
        Console.WriteLine($"  Час виконання: {sw.Elapsed.TotalMilliseconds:F4} ms ({sw.ElapsedTicks} ticks)");
        Console.WriteLine("=====================================================================");

        int rank = 1;
        foreach (var vip in result)
        {
            Console.WriteLine($"  {rank}. {vip.Name,-22} ({vip.City,-16}) | Витрачено: {vip.TotalSpent,11:N2} грн | Сер. чек: {vip.AverageCheck,9:N2} грн");
            rank++;
        }
        Console.WriteLine();
    }
}