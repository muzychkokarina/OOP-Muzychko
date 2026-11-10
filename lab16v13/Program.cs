using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace lab16v13
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Cuisine { get; set; }
        public int PrepTimeMinutes { get; set; }

        public Recipe(int id, string name, string cuisine, int prepTimeMinutes)
        {
            Id = id;
            Name = name;
            Cuisine = cuisine;
            PrepTimeMinutes = prepTimeMinutes;
        }
    }

    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RecipeId { get; set; }

        public Ingredient(int id, string name, int recipeId)
        {
            Id = id;
            Name = name;
            RecipeId = recipeId;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== Лабораторна робота №10 | Варіант 13: Управління рецептами ===\n");

            List<Recipe> recipes = new List<Recipe>
            {
                new Recipe(1, "Spaghetti Carbonara", "Italian", 25),
                new Recipe(2, "Margherita Pizza", "Italian", 20),
                new Recipe(3, "Lasagna Bolognese", "Italian", 90),
                new Recipe(4, "Minestrone Soup", "Italian", 45),
                new Recipe(5, "Bruschetta", "Italian", 15),
                new Recipe(6, "Tiramisu", "Italian", 30),
                new Recipe(7, "Risotto Mushroom", "Italian", 35),
                new Recipe(8, "Sushi Roll", "Japanese", 40),
                new Recipe(9, "Ramen", "Japanese", 60),
                new Recipe(10, "Chicken Curry", "Indian", 50),
                new Recipe(11, "Tacos", "Mexican", 25),
                new Recipe(12, "Guacamole", "Mexican", 10)
            };

            List<Ingredient> ingredients = new List<Ingredient>
            {
                new Ingredient(101, "Spaghetti", 1),
                new Ingredient(102, "Eggs", 1),
                new Ingredient(103, "Pancetta", 1),
                new Ingredient(104, "Parmesan Cheese", 1),
                new Ingredient(105, "Pizza Dough", 2),
                new Ingredient(106, "Tomato Sauce", 2),
                new Ingredient(107, "Mozzarella", 2),
                new Ingredient(108, "Basil", 2),
                new Ingredient(109, "Pasta Sheets", 3),
                new Ingredient(110, "Minced Meat", 3),
                new Ingredient(111, "Béchamel Sauce", 3),
                new Ingredient(112, "Tomatoes", 4),
                new Ingredient(113, "Beans", 4),
                new Ingredient(114, "Carrots", 4),
                new Ingredient(115, "Bread", 5),
                new Ingredient(116, "Garlic", 5),
                new Ingredient(117, "Olive Oil", 5),
                new Ingredient(118, "Mascarpone", 6),
                new Ingredient(119, "Coffee", 6),
                new Ingredient(120, "Savoiardi", 6),
                new Ingredient(121, "Arborio Rice", 7),
                new Ingredient(122, "Mushrooms", 7),
                new Ingredient(123, "Nori", 8),
                new Ingredient(124, "Rice", 8),
                new Ingredient(125, "Noodles", 9),
                new Ingredient(126, "Chicken", 10),
                new Ingredient(127, "Curry Paste", 10),
                new Ingredient(128, "Tortilla", 11),
                new Ingredient(129, "Avocado", 12)
            };

            Stopwatch sw = new Stopwatch();

            Console.WriteLine("--- Запит 1: Рецепти 'Italian' із часом приготування <= 30 хв ---");
            sw.Start();
            var q1Query = from r in recipes
                          where r.Cuisine == "Italian" && r.PrepTimeMinutes <= 30
                          select r;
            var q1QueryResult = q1Query.ToList();
            sw.Stop();
            long t1Query = sw.ElapsedTicks;

            sw.Restart();
            var q1MethodResult = recipes
                .Where(r => r.Cuisine == "Italian" && r.PrepTimeMinutes <= 30)
                .ToList();
            sw.Stop();
            long t1Method = sw.ElapsedTicks;

            foreach (var r in q1MethodResult)
            {
                Console.WriteLine($" - {r.Name} ({r.Cuisine}, {r.PrepTimeMinutes} хв)");
            }
            Console.WriteLine($"Час виконання (Query syntax): {t1Query} тиків | (Method syntax): {t1Method} тиків\n");

            Console.WriteLine("--- Запит 2: Об'єднання рецептів з їхніми інгредієнтами (Join) ---");
            sw.Restart();
            var q2Query = from r in recipes
                          join i in ingredients on r.Id equals i.RecipeId
                          select new { RecipeName = r.Name, IngredientName = i.Name };
            var q2QueryResult = q2Query.ToList();
            sw.Stop();
            long t2Query = sw.ElapsedTicks;

            sw.Restart();
            var q2MethodResult = recipes
                .Join(ingredients,
                      r => r.Id,
                      i => i.RecipeId,
                      (r, i) => new { RecipeName = r.Name, IngredientName = i.Name })
                .ToList();
            sw.Stop();
            long t2Method = sw.ElapsedTicks;

            foreach (var item in q2MethodResult.Take(8))
            {
                Console.WriteLine($" - {item.RecipeName}: {item.IngredientName}");
            }
            Console.WriteLine($"... (усього записів: {q2MethodResult.Count})");
            Console.WriteLine($"Час виконання (Query syntax): {t2Query} тиків | (Method syntax): {t2Method} тиків\n");

            Console.WriteLine("--- Запит 3: Кількість інгредієнтів для кожного рецепту (GroupJoin / Aggregation) ---");
            sw.Restart();
            var q3Query = from r in recipes
                          join i in ingredients on r.Id equals i.RecipeId into recipeIngredients
                          select new { RecipeName = r.Name, Count = recipeIngredients.Count() };
            var q3QueryResult = q3Query.ToList();
            sw.Stop();
            long t3Query = sw.ElapsedTicks;

            sw.Restart();
            var q3MethodResult = recipes
                .GroupJoin(ingredients,
                           r => r.Id,
                           i => i.RecipeId,
                           (r, recipeIngredients) => new { RecipeName = r.Name, Count = recipeIngredients.Count() })
                .ToList();
            sw.Stop();
            long t3Method = sw.ElapsedTicks;

            foreach (var item in q3MethodResult)
            {
                Console.WriteLine($" - {item.RecipeName}: {item.Count} інгр.");
            }
            Console.WriteLine($"Час виконання (Query syntax): {t3Query} тиків | (Method syntax): {t3Method} тиків\n");

            Console.WriteLine("--- Запит 4: Рецепт з найдовшим часом приготування (Max/First) ---");
            sw.Restart();
            int maxTimeQuery = (from r in recipes select r.PrepTimeMinutes).Max();
            var q4Query = (from r in recipes where r.PrepTimeMinutes == maxTimeQuery select r).FirstOrDefault();
            sw.Stop();
            long t4Query = sw.ElapsedTicks;

            sw.Restart();
            int maxTimeMethod = recipes.Max(r => r.PrepTimeMinutes);
            var q4MethodResult = recipes.FirstOrDefault(r => r.PrepTimeMinutes == maxTimeMethod);
            sw.Stop();
            long t4Method = sw.ElapsedTicks;

            if (q4MethodResult != null)
            {
                Console.WriteLine($" - {q4MethodResult.Name} ({q4MethodResult.Cuisine}, {q4MethodResult.PrepTimeMinutes} хв)");
            }
            Console.WriteLine($"Час виконання (Query syntax): {t4Query} тиків | (Method syntax): {t4Method} тиків\n");

            Console.WriteLine("=== Роботу програми завершено успішно ===");
        }
    }
}