using CLI.ApplicationRunner.Strategy.Output;
using Core.Model.Analytics;
using Core.Service.Writer;

public sealed class JsonFileOutputAsyncStrategy : IOutputStrategy
{
    private readonly JsonResultWriter _writer;
    private readonly Func<string, TextWriter> _writerFactory;

    public JsonFileOutputAsyncStrategy(JsonResultWriter writer, Func<string, TextWriter>? writerFactory = null)
    {
        _writer = writer;
        _writerFactory = writerFactory ?? (path => new StreamWriter(path));
    }

    public async Task WriteAsync(AnalyticsResult result, string? outputPath, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(outputPath))
        {
            throw new ArgumentException("Output path is required for file output", nameof(outputPath));
        }

        using var writer = _writerFactory(outputPath);

        await _writer.WriteAsync(result, writer, cancellationToken);
    }
}