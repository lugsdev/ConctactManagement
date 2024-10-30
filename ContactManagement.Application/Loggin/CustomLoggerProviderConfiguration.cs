using Microsoft.Extensions.Logging;

namespace TechChallenge.Api.Loggin;

public class CustomLoggerProviderConfiguration
{
    public LogLevel LogLevel { get; set; } = LogLevel.Warning;
    public int EventId { get; set; } = 0;
}