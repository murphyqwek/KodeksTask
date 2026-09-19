using Core.Exceptions;
using Core.Service.Parser;
using Core.Service.Parser.Csv;

namespace Tests.Service.Parser;

public class CsvParserTests
{
    private readonly ISaleParser _parser = new CsvParser();

    [Fact]
    public void ParseToSale_ValidCsvString_ReturnsSale()
    {
        const string csv =
            "1,9/18/2026,42,Electronics,Europe,2,750.25,0.15,Card,3,4.8,1500.50";

        var sale = _parser.ParseToSale(csv);

        Assert.Equal(1, sale.OrderId);
        Assert.Equal(new DateOnly(2026, 9, 18), sale.OrderDate);
        Assert.Equal(42, sale.CustomerId);
        Assert.Equal("Electronics", sale.ProductCategory);
        Assert.Equal("Europe", sale.Region);

        Assert.Equal(2, sale.Quantity);
        Assert.Equal(750.25m, sale.UnitPrice);

        Assert.Equal(0.15m, sale.Discount);
        Assert.Equal("Card", sale.PaymentMethod);
        Assert.Equal(3, sale.DeliveryDays);
        Assert.Equal(4.8m, sale.CustomerRating);
        Assert.Equal(1500.50m, sale.Revenue);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void ParseToSale_EmptyString_ThrowsArgumentException(string csv)
    {
        var act = () => _parser.ParseToSale(csv);

        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void ParseToSale_InvalidDateFormat_ThrowsSaleParsingException()
    {
        const string csv =
            "1,18.09.2026,42,Electronics,Europe,2,750.25,0.15,Card,3,4.8,1500.50";

        var exception = Assert.Throws<SaleParsingException>(
            () => _parser.ParseToSale(csv));

        Assert.NotNull(exception.InnerException);
    }
}