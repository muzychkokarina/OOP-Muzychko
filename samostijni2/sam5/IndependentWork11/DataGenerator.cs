namespace IndependentWork11;

public static class DataGenerator
{
    public static (List<Product> Products, List<Customer> Customers, List<Sale> Sales) GenerateData(
        int productCount = 60,
        int customerCount = 80,
        int saleCount = 500)
    {
        var random = new Random(42);

        var categories = new[]
        {
            "Електроніка", "Ноутбуки та ПК", "Смартфони",
            "Периферія", "Аксесуари", "Мережеве обладнання"
        };

        var prefixes = new[]
        {
            "Pro", "Ultra", "Smart", "Max", "Lite", "Gaming", "Office", "Compact"
        };

        var items = new[]
        {
            "Монітор", "Клавіатура", "Миша", "Навушники", "Роутер",
            "Ноутбук", "Планшет", "Смартфон", "Павербанк", "SSD-накопичувач"
        };

        var products = new List<Product>(productCount);
        for (int i = 1; i <= productCount; i++)
        {
            var category = categories[random.Next(categories.Length)];
            var name = $"{items[random.Next(items.Length)]} {prefixes[random.Next(prefixes.Length)]} #{i}";
            var price = Math.Round((decimal)(random.NextDouble() * 19500 + 500), 2);

            products.Add(new Product
            {
                Id = i,
                Name = name,
                Category = category,
                UnitPrice = price
            });
        }

        var cities = new[]
        {
            "Рівне", "Київ", "Львів", "Луцьк", "Тернопіль",
            "Житомир", "Вінниця", "Івано-Франківськ"
        };

        var firstNames = new[]
        {
            "Олександр", "Марія", "Іван", "Анна", "Дмитро",
            "Софія", "Максим", "Катерина", "Артем", "Олена"
        };

        var lastNames = new[]
        {
            "Ковальчук", "Бондаренко", "Ткаченко", "Мельник", "Шевченко",
            "Кравчук", "Олійник", "Поліщук", "Савчук", "Романюк"
        };

        var customers = new List<Customer>(customerCount);
        for (int i = 1; i <= customerCount; i++)
        {
            var fullName = $"{firstNames[random.Next(firstNames.Length)]} {lastNames[random.Next(lastNames.Length)]}";
            var city = cities[random.Next(cities.Length)];

            customers.Add(new Customer
            {
                Id = i,
                Name = fullName,
                City = city
            });
        }

        var startDate = new DateTime(2024, 1, 1);
        int totalDays = (new DateTime(2026, 11, 1) - startDate).Days;

        var sales = new List<Sale>(saleCount);
        for (int i = 1; i <= saleCount; i++)
        {
            sales.Add(new Sale
            {
                Id = i,
                ProductId = random.Next(1, productCount + 1),
                CustomerId = random.Next(1, customerCount + 1),
                Quantity = random.Next(1, 11),
                SaleDate = startDate.AddDays(random.Next(totalDays))
            });
        }

        return (products, customers, sales);
    }
}