using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Newtonsoft.Json;

// Интерфейс репозитория
public interface ILoggerRepository
{
    void Log(string message);
}

// Реализация репозитория для текстового файла
public class FileLoggerRepository : ILoggerRepository
{
    private readonly string _filePath;

    public FileLoggerRepository(string filePath)
    {
        _filePath = filePath;
    }

    public void Log(string message)
    {
        File.AppendAllText(_filePath, $"{DateTime.Now}: {message}\n");
    }
}

// Реализация репозитория для JSON-файла
public class JsonLoggerRepository : ILoggerRepository
{
    private readonly string _filePath;
    private readonly List<LogEntry> _logEntries = new List<LogEntry>();

    public JsonLoggerRepository(string filePath)
    {
        _filePath = filePath;
    }

    public void Log(string message)
    {
        _logEntries.Add(new LogEntry { Timestamp = DateTime.Now, Message = message });
        var json = JsonConvert.SerializeObject(_logEntries, Newtonsoft.Json.Formatting.Indented);
        File.WriteAllText(_filePath, json);
    }

    private class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
    }
}

// Класс MyLogger
public class MyLogger
{
    private readonly ILoggerRepository _repository;

    public MyLogger(ILoggerRepository repository)
    {
        _repository = repository;
    }

    public void Log(string message)
    {
        _repository.Log(message);
    }
}

// Пример использования
class Program
{
    static void Main(string[] args)
    {
        var fileLogger = new MyLogger(new FileLoggerRepository("log.txt"));
        fileLogger.Log("Сообщение в текстовый файл");

        var jsonLogger = new MyLogger(new JsonLoggerRepository("log.json"));
        jsonLogger.Log("Сообщение в JSON-файл");

        Console.WriteLine("Логирование завершено.");
    }
}
