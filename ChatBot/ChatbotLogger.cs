using Microsoft.Extensions.Logging;
using System;

public class ChatbotLogger : ILogger
{
    private readonly string _categoryName;

    public ChatbotLogger(string categoryName)
    {
        _categoryName = categoryName;
    }

    public IDisposable BeginScope<TState>(TState state) => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception exception,
        Func<TState, Exception, string> formatter)
    {
        var message = formatter(state, exception);
        if (string.IsNullOrEmpty(message))
            return;

        var log = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{logLevel}] [{_categoryName}] {message}";
        if (exception != null)
        {
            log += $"{Environment.NewLine}{exception}";
        }
        Console.WriteLine(log);
    }
}

public class ChatbotLoggerProvider : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName)
        => new ChatbotLogger(categoryName);

    public void Dispose() { }
}