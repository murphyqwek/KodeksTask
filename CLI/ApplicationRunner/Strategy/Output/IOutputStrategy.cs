using Core.Model.Analytics;

namespace CLI.ApplicationRunner.Strategy.Output;

public interface IOutputStrategy
{
    Task WriteAsync(
        AnalyticsResult result,
        string? outputPath,
        CancellationToken cancellationToken = default);
}
