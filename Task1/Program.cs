using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;

// Интерфейс наблюдателя
public interface IObserver
{
    void Update(string directoryPath);
}

// Интерфейс наблюдаемого объекта
public interface ISubject
{
    void Attach(IObserver observer);
    void Detach(IObserver observer);
    void Notify();
}

// Реализация наблюдаемого объекта
public class FileSystemWatcherSimulator : ISubject
{
    private readonly string _directoryPath;
    private readonly System.Timers.Timer _timer; // Теперь компилятор знает, что это System.Timers.Timer
    private readonly List<IObserver> _observers = new List<IObserver>();
    private DateTime _lastCheckTime;

    public FileSystemWatcherSimulator(string directoryPath)
    {
        _directoryPath = directoryPath;
        _timer = new System.Timers.Timer(1000); // Проверка каждую секунду
        _timer.Elapsed += OnTimerElapsed;
        _lastCheckTime = DateTime.Now;
    }

    public void Start()
    {
        _timer.Start();
    }

    public void Stop()
    {
        _timer.Stop();
    }

    private void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        try
        {
            // Попытка получить список файлов в директории
            var currentFiles = Directory.GetFiles(_directoryPath);
            var currentTime = DateTime.Now;

            // Проверка изменений
            foreach (var file in currentFiles)
            {
                var fileInfo = new FileInfo(file);
                if (fileInfo.LastWriteTime > _lastCheckTime)
                {
                    Notify();
                    break;
                }
            }

            _lastCheckTime = currentTime;
        }
        catch (Exception ex) // Ловим все исключения
        {
            // Логируем ошибку или выводим сообщение
            Console.WriteLine($"Ошибка при проверке директории: {ex.Message}");
        }
    }

    public void Attach(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        _observers.Remove(observer);
    }

    public void Notify()
    {
        foreach (var observer in _observers)
        {
            observer.Update(_directoryPath);
        }
    }
}

// Реализация наблюдателя
public class FileChangeObserver : IObserver
{
    public void Update(string directoryPath)
    {
        Console.WriteLine($"Изменение обнаружено в директории: {directoryPath}");
    }
}

// Пример использования
class Program
{
    static void Main(string[] args)
    {
        var directoryPath = @"C:\TestDirectory";
        var watcher = new FileSystemWatcherSimulator(directoryPath);
        var observer = new FileChangeObserver();

        watcher.Attach(observer);
        watcher.Start();

        Console.WriteLine("Наблюдение за директорией запущено. Нажмите Enter для завершения.");
        Console.ReadLine();

        watcher.Stop();
    }
}
