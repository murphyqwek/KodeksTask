using CLI.ApplicationRunner.Strategy.Output;
using Core.Model.Analytics;
using Core.Service.Writer;

public sealed class JsonFileOutputStrategy : IOutputStrategy
{
    private readonly IResultWriter _writer;
    private readonly Func<string, TextWriter> _writerFactory;

    public JsonFileOutputStrategy(IResultWriter writer, Func<string, TextWriter>? writerFactory = null)
    {
        _writer = writer;
        _writerFactory = writerFactory ?? (path => new StreamWriter(path));
    }

    public Task WriteAsync(AnalyticsResult result, string? outputPath, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(outputPath))
        {
            throw new ArgumentException("Output path is required for file output", nameof(outputPath));
        }

        using var writer = _writerFactory(outputPath);

        _writer.Write(result, writer);

        return Task.CompletedTask;
    }
}