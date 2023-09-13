using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;

namespace CustomerValidator.Application.Logging;

public static class FileLogger
{
    public static ILoggingBuilder BuildLogger(string logFilePath, ILoggingBuilder loggingBuilder)
    {
        loggingBuilder.AddSerilog(CreateLogger(logFilePath), dispose: true);
        return loggingBuilder;
    }

    private static Logger CreateLogger(string logFilePath)
    {
        return new LoggerConfiguration()
           .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
           .CreateLogger();
    }
}

