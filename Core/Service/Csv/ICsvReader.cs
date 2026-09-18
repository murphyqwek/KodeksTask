using Core.Model;

namespace Core.Service.Csv;

public interface ICsvReader
{
    IReadOnlyList<Sale> Read(TextReader reader);

    Task<IReadOnlyList<Sale>> ReadAsync(
        TextReader reader,
        CancellationToken cancellationToken = default);
}