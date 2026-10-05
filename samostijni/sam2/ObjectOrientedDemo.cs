using System;
using System.Collections.Generic;

namespace sam2
{
    public class Product
    {
        public string Name { get; set; }
        public double Price { get; set; }

        public Product(string name, double price)
        {
            Name = name;
            Price = price;
        }
    }

    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public CartItem(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        public double GetSubtotal()
        {
            double subtotal = Product.Price * Quantity;
            if (Product.Price > 500)
            {
                subtotal *= 0.90; // знижка 10%
            }
            return subtotal;
        }
    }

    public class Cart
    {
        private List<CartItem> _items = new List<CartItem>();

        public void AddItem(Product product, int quantity)
        {
            _items.Add(new CartItem(product, quantity));
        }

        public double GetTotal()
        {
            double total = 0;
            int index = 1;
            foreach (var item in _items)
            {
                double itemTotal = item.GetSubtotal();
                Console.WriteLine($"Товар {index++}: {item.Product.Name}, Ціна = {item.Product.Price} грн, Кількість = {item.Quantity}, Сума зі знижкою = {itemTotal:F2} грн");
                total += itemTotal;
            }
            return total;
        }
    }

    public static class ObjectOrientedDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== ОБ'ЄКТНО-ОРІЄНТОВАНИЙ ПІДХІД ===");

            Cart cart = new Cart();
            cart.AddItem(new Product("Ноутбук", 25000.0), 1);
            cart.AddItem(new Product("Мишка", 450.0), 2);
            cart.AddItem(new Product("Монітор", 7500.0), 1);

            double total = cart.GetTotal();
            Console.WriteLine($"Загальна сума кошика (з урахуванням знижки 10% на товари > 500 грн): {total:F2} грн\n");
        }
    }
}