using System;
using System.Threading;

// Реализация паттерна "Одиночка"
public sealed class SingleRandomizer
{
    private static readonly Lazy<SingleRandomizer> _instance = new Lazy<SingleRandomizer>(() => new SingleRandomizer());
    private readonly Random _random;

    private SingleRandomizer()
    {
        _random = new Random();
    }

    public static SingleRandomizer Instance => _instance.Value;

    public int Next()
    {
        return _random.Next();
    }

    public int Next(int maxValue)
    {
        return _random.Next(maxValue);
    }

    public int Next(int minValue, int maxValue)
    {
        return _random.Next(minValue, maxValue);
    }
}

// Пример использования
class Program
{
    static void Main(string[] args)
    {
        // Создание нескольких потоков
        for (int i = 0; i < 5; i++)
        {
            Thread thread = new Thread(GenerateRandomNumbers);
            thread.Start();
        }
    }

    static void GenerateRandomNumbers()
    {
        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId}: {SingleRandomizer.Instance.Next(1, 100)}");
        }
    }
}
