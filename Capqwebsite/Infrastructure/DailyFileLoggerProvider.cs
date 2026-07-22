using System.Collections.Concurrent;

namespace Capqwebsite.Infrastructure;

public sealed class DailyFileLoggerProvider : ILoggerProvider
{
    private readonly string _logsDirectory;
    private readonly ConcurrentDictionary<string, DailyFileLogger> _loggers = new();
    private readonly object _writeLock = new();

    public DailyFileLoggerProvider(string logsDirectory)
    {
        _logsDirectory = logsDirectory;
        Directory.CreateDirectory(_logsDirectory);
    }

    public ILogger CreateLogger(string categoryName) =>
        _loggers.GetOrAdd(categoryName, category => new DailyFileLogger(category, WriteLine));

    private void WriteLine(string message)
    {
        try
        {
            lock (_writeLock)
            {
                var path = Path.Combine(_logsDirectory, $"application-{DateTime.Now:yyyy-MM-dd}.log");
                File.AppendAllText(path, message + Environment.NewLine);
            }
        }
        catch (IOException ioException)
        {
            Console.Error.WriteLine($"File logging failed: {ioException}");
        }
        catch (UnauthorizedAccessException accessException)
        {
            Console.Error.WriteLine($"File logging access failed: {accessException}");
        }
    }

    public void Dispose() => _loggers.Clear();

    private sealed class DailyFileLogger(string category, Action<string> writeLine) : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
        public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Information;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
            Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            var exceptionText = exception is null ? string.Empty : Environment.NewLine + exception;
            writeLine($"{DateTimeOffset.Now:yyyy-MM-dd HH:mm:ss.fff zzz} [{logLevel}] " +
                      $"{category} ({eventId.Id}) {formatter(state, exception)}{exceptionText}");
        }
    }
}
