using Core.Service.Csv;

namespace Tests.Service.Reader;

public class CsvReaderTests
{
    [Fact]
    public void Read_ShouldParseAllRows()
    {
        string csv = """
            order_id,order_date,customer_id,product_category,region,quantity,unit_price,discount,payment_method,delivery_days,customer_rating,revenue
            1,1/1/2006,100,Electronics,Europe,2,750.00,0.1,Card,3,4.5,1500
            2,1/2/2006,101,Books,Asia,1,500.00,0.0,Cash,5,4.8,500
            """;

        using var textReader = new StringReader(csv);

        var reader = new CsvReader();

        var result = reader.Read(textReader);

        Assert.Equal(2, result.Count);

        var firstSale = result[0];

        Assert.Equal(1, firstSale.OrderId);
        Assert.Equal(new DateOnly(2006, 1, 1), firstSale.OrderDate);
        Assert.Equal(100, firstSale.CustomerId);
        Assert.Equal("Electronics", firstSale.ProductCategory);
        Assert.Equal("Europe", firstSale.Region);
        Assert.Equal(2, firstSale.Quantity);
        Assert.Equal(750.00m, firstSale.UnitPrice);
        Assert.Equal(0.1m, firstSale.Discount);
        Assert.Equal("Card", firstSale.PaymentMethod);
        Assert.Equal(3, firstSale.DeliveryDays);
        Assert.Equal(4.5m, firstSale.CustomerRating);
        Assert.Equal(1500m, firstSale.Revenue);

        var secondSale = result[1];

        Assert.Equal(2, secondSale.OrderId);
        Assert.Equal(new DateOnly(2006, 1, 2), secondSale.OrderDate);
        Assert.Equal(101, secondSale.CustomerId);
        Assert.Equal("Books", secondSale.ProductCategory);
        Assert.Equal("Asia", secondSale.Region);
        Assert.Equal(1, secondSale.Quantity);
        Assert.Equal(500.00m, secondSale.UnitPrice);
        Assert.Equal(0.0m, secondSale.Discount);
        Assert.Equal("Cash", secondSale.PaymentMethod);
        Assert.Equal(5, secondSale.DeliveryDays);
        Assert.Equal(4.8m, secondSale.CustomerRating);
        Assert.Equal(500m, secondSale.Revenue);
    }

    [Fact]
    public async Task ReadAsync_ShouldParseAllRows()
    {
        string csv = """
            order_id,order_date,customer_id,product_category,region,quantity,unit_price,discount,payment_method,delivery_days,customer_rating,revenue
            1,1/1/2026,100,Electronics,Europe,2,750.00,0.1,Card,3,4.5,1500
            """;

        using var textReader = new StringReader(csv);

        var reader = new CsvReader();

        var result = await reader.ReadAsync(textReader);

        Assert.Single(result);

        var sale = result[0];

        Assert.Equal(1, sale.OrderId);
        Assert.Equal(new DateOnly(2026, 1, 1), sale.OrderDate);
        Assert.Equal(100, sale.CustomerId);
        Assert.Equal("Electronics", sale.ProductCategory);
        Assert.Equal("Europe", sale.Region);
        Assert.Equal(2, sale.Quantity);
        Assert.Equal(750.00m, sale.UnitPrice);
        Assert.Equal(0.1m, sale.Discount);
        Assert.Equal("Card", sale.PaymentMethod);
        Assert.Equal(3, sale.DeliveryDays);
        Assert.Equal(4.5m, sale.CustomerRating);
        Assert.Equal(1500m, sale.Revenue);
    }
}