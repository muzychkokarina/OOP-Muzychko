namespace IndependentWork12;

public static class MathWorkload
{
    public static List<int> GenerateData(int count, int seed = 42)
    {
        var random = new Random(seed);
        var list = new List<int>(count);
        for (int i = 0; i < count; i++)
        {
            list.Add(random.Next(1000, 100000));
        }
        return list;
    }

    public static bool IsPrime(int number)
    {
        if (number < 2) return false;
        if (number == 2) return true;
        if (number % 2 == 0) return false;

        int boundary = (int)Math.Sqrt(number);
        for (int i = 3; i <= boundary; i += 2)
        {
            if (number % i == 0)
                return false;
        }
        return true;
    }

    public static double HeavyMathTransform(int number)
    {
        double acc = 0;
        for (int i = 1; i <= 25; i++)
        {
            acc += Math.Sqrt(number * i) * Math.Sin(i) + Math.Log(number + i);
        }
        return acc;
    }
}