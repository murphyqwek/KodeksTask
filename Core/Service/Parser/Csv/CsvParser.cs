using Core.Exceptions;
using Core.Model;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Globalization;

namespace Core.Service.Parser.Csv;

public sealed class CsvParser : ISaleParser
{
    public Sale ParseToSale(string saleString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(saleString);

        var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
            TrimOptions = TrimOptions.Trim
        };

        try
        {
            using var reader = new StringReader(saleString);
            using var csv = new CsvReader(reader, configuration);

            csv.Context.RegisterClassMap<CsvSaleMap>();

            if (!csv.Read())
            {
                throw new SaleParsingException(
                    "Failed to parse sale: CSV row is empty.",
                    new FormatException());
            }

            return csv.GetRecord<Sale>();
        }
        catch (FieldValidationException ex)
        {
            throw new SaleParsingException(
                $"Failed to parse sale: {ex.Message}",
                ex);
        }
        catch (ReaderException ex) when (ex.InnerException is TypeConverterException typeException)
        {
            throw new SaleParsingException(
                $"Failed to parse sale: cannot convert value '{typeException.Text}' to the required type.",
                typeException);
        }
        catch (CsvHelper.MissingFieldException ex)
        {
            throw new SaleParsingException(
                "Failed to parse sale: one or more required CSV fields are missing.",
                ex);
        }
        catch (BadDataException ex)
        {
            throw new SaleParsingException(
                "Failed to parse sale: invalid CSV format.",
                ex);
        }
        catch (CsvHelperException ex)
        {
            throw new SaleParsingException(
                $"Failed to parse sale: {ex.Message}",
                ex);
        }
    }
}