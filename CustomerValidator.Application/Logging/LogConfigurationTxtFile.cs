
using CustomerValidator.Application.Interfaces;

namespace CustomerValidator.Application.Logging;

public class LogConfigurationTxtFile : ILogConfiguration
{
    public string LogFilePath => "C:\\Tmp\\test.log";
}

