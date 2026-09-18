using Core.Model;
using Core.Service.Parser;

namespace Core.Service.Csv;

public sealed class CsvReader : ICsvReader
{
    private readonly ISaleParser _saleParser;

    public CsvReader(ISaleParser saleParser)
    {
        _saleParser = saleParser;
    }

    public IReadOnlyList<Sale> Read(TextReader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);

        var sales = new List<Sale>();

        //Здесь мы пропускаем заголовок
        reader.ReadLine();

        string? line;

        while ((line = reader.ReadLine()) is not null)
        {
            var sale = _saleParser.ParseToSale(line);
            sales.Add(sale);
        }

        return sales;
    }

    public async Task<IReadOnlyList<Sale>> ReadAsync(TextReader reader, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(reader);

        var sales = new List<Sale>();

        //Здесь мы пропускаем заголовок
        await reader.ReadLineAsync(cancellationToken);

        string? line;

        while ((line = await reader.ReadLineAsync(cancellationToken)) is not null)
        {
            var sale = _saleParser.ParseToSale(line);
            sales.Add(sale);
        }

        return sales;
    }
}