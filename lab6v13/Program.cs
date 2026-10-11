using System;
using System.Collections.Generic;

namespace lab6v13
{
    public interface IHasher
    {
        string ComputeHash(string data);
    }

    public class MD5Hasher : IHasher
    {
        public string ComputeHash(string data)
        {
            return "MD5_" + data + "_hash";
        }
    }

    public class SHA256Hasher : IHasher
    {
        public string ComputeHash(string data)
        {
            return "SHA256_" + data + "_hash";
        }
    }

    public abstract class ImageConverter
    {
        public string FilePath = string.Empty;

        public abstract void LoadImage(string path);
        public abstract void ConvertImage();

        public void SaveImage(string path)
        {
            Console.WriteLine("Збережено у файл: " + path);
        }
    }

    public class PngToJpegConverter : ImageConverter
    {
        public override void LoadImage(string path)
        {
            FilePath = path;
            Console.WriteLine("Завантажено PNG: " + FilePath);
        }

        public override void ConvertImage()
        {
            Console.WriteLine("Конвертація з PNG у JPEG...");
        }
    }

    public class BmpToPngConverter : ImageConverter
    {
        public override void LoadImage(string path)
        {
            FilePath = path;
            Console.WriteLine("Завантажено BMP: " + FilePath);
        }

        public override void ConvertImage()
        {
            Console.WriteLine("Конвертація з BMP у PNG...");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<IHasher> hashers = new List<IHasher>();
            hashers.Add(new MD5Hasher());
            hashers.Add(new SHA256Hasher());

            Console.WriteLine("--- Перевірка IHasher ---");
            foreach (IHasher hasher in hashers)
            {
                string result = hasher.ComputeHash("myText");
                Console.WriteLine("Результат: " + result);
            }

            Console.WriteLine();

            List<ImageConverter> converters = new List<ImageConverter>();
            converters.Add(new PngToJpegConverter());
            converters.Add(new BmpToPngConverter());

            Console.WriteLine("--- Перевірка ImageConverter ---");
            foreach (ImageConverter converter in converters)
            {
                converter.LoadImage("photo.png");
                converter.ConvertImage();
                converter.SaveImage("result.jpg");
                Console.WriteLine();
            }
        }
    }
}