using System;
using System.Collections.Generic;
using System.Text;

namespace lab15v13
{
    public class ChatMessage
    {
        public string Sender { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }

        public ChatMessage(string sender, string text)
        {
            Sender = sender;
            Text = text;
            Timestamp = DateTime.Now;
        }

        public override string ToString()
        {
            return $"[{Timestamp:HH:mm:ss}] {Sender}: {Text}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== Лабораторна робота №9 | Варіант 13: Система чату ===\n");

            Console.WriteLine("--- 1. HashSet<string> (Користувачі онлайн) ---");
            HashSet<string> onlineUsers = new HashSet<string>();

            Console.WriteLine($"Додаємо 'Олена': {onlineUsers.Add("Олена")}");
            Console.WriteLine($"Додаємо 'Іван': {onlineUsers.Add("Іван")}");
            Console.WriteLine($"Додаємо 'Максим': {onlineUsers.Add("Максим")}");
            Console.WriteLine($"Додаємо дублікат 'Олена': {onlineUsers.Add("Олена")}");

            Console.WriteLine($"Чи є 'Іван' онлайн? {onlineUsers.Contains("Іван")}");
            Console.WriteLine($"Чи є 'Петро' онлайн? {onlineUsers.Contains("Петро")}");

            onlineUsers.Remove("Максим");

            Console.WriteLine("Поточні користувачі онлайн:");
            foreach (var user in onlineUsers)
            {
                Console.WriteLine($" - {user}");
            }
            Console.WriteLine();

            Console.WriteLine("--- 2. Queue<ChatMessage> (Черга повідомлень — FIFO) ---");
            Queue<ChatMessage> messageQueue = new Queue<ChatMessage>();

            messageQueue.Enqueue(new ChatMessage("Олена", "Привіт усім!"));
            messageQueue.Enqueue(new ChatMessage("Іван", "Вітаю, Олено!"));
            messageQueue.Enqueue(new ChatMessage("Олена", "Як успіхи з лабораторною?"));

            Console.WriteLine($"Перше повідомлення у черзі (Peek): {messageQueue.Peek()}");
            Console.WriteLine($"Кількість повідомлень у черзі: {messageQueue.Count}");

            Console.WriteLine("\nОбробка черги повідомлень:");
            while (messageQueue.Count > 0)
            {
                ChatMessage msg = messageQueue.Dequeue();
                Console.WriteLine($"[Оброблено]: {msg}");
            }
            Console.WriteLine($"Залишок у черзі: {messageQueue.Count}\n");

            Console.WriteLine("--- 3. Stack<ChatMessage> (Історія повідомлень — LIFO) ---");
            Stack<ChatMessage> messageHistory = new Stack<ChatMessage>();

            messageHistory.Push(new ChatMessage("Олена", "Перше повідомлення"));
            messageHistory.Push(new ChatMessage("Іван", "Друге повідомлення"));
            messageHistory.Push(new ChatMessage("Олена", "Останнє повідомлення"));

            Console.WriteLine($"Останнє надіслане повідомлення (Peek): {messageHistory.Peek()}");

            ChatMessage lastMessage = messageHistory.Pop();
            Console.WriteLine($"Видалено останнє повідомлення (Pop): {lastMessage}");

            Console.WriteLine("\nПерегляд історії повідомлень:");
            foreach (var msg in messageHistory)
            {
                Console.WriteLine($" - {msg}");
            }

            Console.WriteLine("\n=== Роботу програми завершено успішно ===");
        }
    }
}