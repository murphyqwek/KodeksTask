using Core.Model;
using Core.Service.Csv;

namespace CLI.ApplicationRunner.Strategy.Input;

public sealed class AsyncInputStrategy : IInputStrategy
{
    private readonly ICsvReader _csvReader;
    private readonly Func<string, TextReader> _readerFactory;

    public AsyncInputStrategy(ICsvReader csvReader, Func<string, TextReader>? readerFactory = null)
    {
        _csvReader = csvReader;
        _readerFactory = readerFactory ?? CreateReader;
    }

    public async Task<IReadOnlyList<Sale>> ReadAsync(string inputPath, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using var reader = _readerFactory(inputPath);

        return await _csvReader.ReadAsync(
            reader,
            cancellationToken);
    }

    private static TextReader CreateReader(string inputPath)
    {
        var stream = new FileStream(
            inputPath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            bufferSize: 4096,
            options: FileOptions.Asynchronous);

        return new StreamReader(stream);
    }
}