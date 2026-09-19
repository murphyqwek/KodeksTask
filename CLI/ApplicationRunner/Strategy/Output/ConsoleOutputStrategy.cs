using Core.Model.Analytics;
using Core.Service.Writer;

namespace CLI.ApplicationRunner.Strategy.Output;

public class ConsoleOutputStrategy : IOutputStrategy
{
    private readonly TextResultWriter _writer;

    public ConsoleOutputStrategy(TextResultWriter writer)
    {
        _writer = writer;
    }

    public Task WriteAsync(AnalyticsResult result, string? outputPath, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _writer.Write(result, Console.Out);

        return Task.CompletedTask;
    }
}