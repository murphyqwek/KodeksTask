using System.Globalization;
using Core.Exceptions;
using Core.Model;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace Core.Service.Csv;

public sealed class CsvReader : ICsvReader
{
    public IReadOnlyList<Sale> Read(TextReader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);

        try
        {
            using var csv = CreateCsvReader(reader);

            return csv
                .GetRecords<Sale>()
                .ToList();
        }
        catch (Exception ex)
        {
            throw ConvertException(ex);
        }
    }

    public async Task<IReadOnlyList<Sale>> ReadAsync(
        TextReader reader,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(reader);

        try
        {
            using var csv = CreateCsvReader(reader);

            var sales = new List<Sale>();

            await foreach (var sale in csv.GetRecordsAsync<Sale>(cancellationToken))
            {
                sales.Add(sale);
            }

            return sales;
        }
        catch (Exception ex)
        {
            throw ConvertException(ex);
        }
    }

    private static CsvHelper.CsvReader CreateCsvReader(TextReader reader)
    {
        var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            TrimOptions = TrimOptions.Trim
        };

        var csv = new CsvHelper.CsvReader(reader, configuration);

        csv.Context.RegisterClassMap<CsvSaleMap>();

        return csv;
    }

    private static SaleParsingException ConvertException(Exception exception)
    {
        return exception switch
        {
            FieldValidationException ex => new SaleParsingException($"Failed to parse sale: invalid value '{ex.Field}'", ex),

            TypeConverterException ex => new SaleParsingException($"Failed to parse sale: cannot convert value '{ex.Text}' to the required type", ex),

            CsvHelper.MissingFieldException ex => new SaleParsingException("Failed to parse sale: one or more required CSV fields are missing", ex),

            BadDataException ex => new SaleParsingException("Failed to parse sale: invalid CSV format", ex),

            CsvHelperException ex => new SaleParsingException("Failed to parse CSV", ex),

            _ => new SaleParsingException("Unexpected error while reading CSV", exception)
        };
    }
}