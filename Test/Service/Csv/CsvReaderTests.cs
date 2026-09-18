using Core.Model;
using Core.Service.Parser;
using Core.Service.Csv;
using Moq;

namespace Tests.Service.Reader;

public class CsvReaderTests
{
    [Fact]
    public void Read_ShouldSkipHeaderAndParseAllRows()
    {
        const string csv = """
            order_id,order_date,customer_id,product_category,region,discount,payment_method,delivery_days,customer_rating,revenue
            1,2026-01-01,100,Electronics,Europe,0.1,Card,3,4.5,1500
            2,2026-01-02,101,Books,Asia,0.0,Cash,5,4.8,500
            """;

        using var textReader = new StringReader(csv);

        var firstSale = CreateSale(
            orderId: 1,
            orderDate: new DateOnly(2026, 1, 1),
            customerId: 100,
            productCategory: "Electronics",
            region: "Europe",
            discount: 0.1m,
            paymentMethod: "Card",
            deliveryDays: 3,
            customerRating: 4.5m,
            revenue: 1500m);

        var secondSale = CreateSale(
            orderId: 2,
            orderDate: new DateOnly(2026, 1, 2),
            customerId: 101,
            productCategory: "Books",
            region: "Asia",
            discount: 0.0m,
            paymentMethod: "Cash",
            deliveryDays: 5,
            customerRating: 4.8m,
            revenue: 500m);

        var parserMock = new Mock<ISaleParser>();

        parserMock
            .Setup(x => x.ParseToSale(
                "1,2026-01-01,100,Electronics,Europe,0.1,Card,3,4.5,1500"))
            .Returns(firstSale);

        parserMock
            .Setup(x => x.ParseToSale(
                "2,2026-01-02,101,Books,Asia,0.0,Cash,5,4.8,500"))
            .Returns(secondSale);

        var reader = new CsvReader(parserMock.Object);

        var result = reader.Read(textReader);

        Assert.Equal(2, result.Count);

        Assert.Same(firstSale, result[0]);
        Assert.Same(secondSale, result[1]);

        parserMock.Verify(
            x => x.ParseToSale(
                "1,2026-01-01,100,Electronics,Europe,0.1,Card,3,4.5,1500"),
            Times.Once);

        parserMock.Verify(
            x => x.ParseToSale(
                "2,2026-01-02,101,Books,Asia,0.0,Cash,5,4.8,500"),
            Times.Once);
    }

    [Fact]
    public async Task ReadAsync_ShouldSkipHeaderAndParseAllRows()
    {
        const string csv = """
            order_id,order_date,customer_id,product_category,region,discount,payment_method,delivery_days,customer_rating,revenue
            1,2026-01-01,100,Electronics,Europe,0.1,Card,3,4.5,1500
            """;

        using var textReader = new StringReader(csv);

        var sale = CreateSale(
            orderId: 1,
            orderDate: new DateOnly(2026, 1, 1),
            customerId: 100,
            productCategory: "Electronics",
            region: "Europe",
            discount: 0.1m,
            paymentMethod: "Card",
            deliveryDays: 3,
            customerRating: 4.5m,
            revenue: 1500m);

        var parserMock = new Mock<ISaleParser>();

        parserMock
            .Setup(x => x.ParseToSale(
                "1,2026-01-01,100,Electronics,Europe,0.1,Card,3,4.5,1500"))
            .Returns(sale);

        var reader = new CsvReader(parserMock.Object);

        var result = await reader.ReadAsync(textReader);

        Assert.Single(result);
        Assert.Same(sale, result[0]);

        parserMock.Verify(
            x => x.ParseToSale(
                "1,2026-01-01,100,Electronics,Europe,0.1,Card,3,4.5,1500"),
            Times.Once);
    }

    private static Sale CreateSale(
        int orderId,
        DateOnly orderDate,
        int customerId,
        string productCategory,
        string region,
        decimal discount,
        string paymentMethod,
        int deliveryDays,
        decimal customerRating,
        decimal revenue)
    {
        return new Sale
        {
            OrderId = orderId,
            OrderDate = orderDate,
            CustomerId = customerId,
            ProductCategory = productCategory,
            Region = region,
            Discount = discount,
            PaymentMethod = paymentMethod,
            DeliveryDays = deliveryDays,
            CustomerRating = customerRating,
            Revenue = revenue
        };
    }
}