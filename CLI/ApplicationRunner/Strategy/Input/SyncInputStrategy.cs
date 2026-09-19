using Core.Model;
using Core.Service.Csv;

namespace CLI.ApplicationRunner.Strategy.Input;

public sealed class SyncInputStrategy : IInputStrategy
{
    private readonly ICsvReader _csvReader;
    private readonly Func<string, TextReader> _readerFactory;

    public SyncInputStrategy(ICsvReader csvReader, Func<string, TextReader>? readerFactory = null)
    {
        _csvReader = csvReader;
        _readerFactory = readerFactory ?? (path => new StreamReader(path));
    }

    public Task<IReadOnlyList<Sale>> ReadAsync(string inputPath, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using var reader = _readerFactory(inputPath);

        var sales = _csvReader.Read(reader);

        return Task.FromResult(sales);
    }
}