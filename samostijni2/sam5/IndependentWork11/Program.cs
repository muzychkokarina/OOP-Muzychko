using System.Text;
using IndependentWork11;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

var (products, customers, sales) = DataGenerator.GenerateData(
    productCount: 60,
    customerCount: 80,
    saleCount: 500);

Console.WriteLine($"Згенеровано тестовий набір: {products.Count} товарів, {customers.Count} клієнтів, {sales.Count} продажів.\n");

var analytics = new AnalyticsService(products, customers, sales);

analytics.TopProductsByCategory();
analytics.TotalRevenueByCustomerCity();
analytics.SalesByYearAndMonth();
analytics.TopVipCustomers();