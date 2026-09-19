using Core.Model;
using Core.Model.Analytics;
using Core.Service.Analytics;

namespace Tests.Analytics;

public class AnalyticsServiceTests
{
    private readonly AnalyticsService _service = new();

    [Fact]
    public void Analyze_ShouldCalculateSalesByCategory()
    {
        var sales = CreateSales();

        var result = _service.Analyze(sales, null, null);

        Assert.Equal(
            [
                new CategorySales("Beauty", 200m),
                new CategorySales("Books", 210m),
                new CategorySales("Clothing", 80m),
                new CategorySales("Electronics", 400m),
                new CategorySales("Sports", 60m)
            ],
            result.SalesByCategory.Categories);
    }

    [Fact]
    public void Analyze_ShouldReturnTopFourCategoriesByQuantity()
    {
        var sales = CreateSales();

        var result = _service.Analyze(sales, null, null);

        Assert.Equal(
            [
                new CategoryQuantity("Books", 7),
                new CategoryQuantity("Sports", 6),
                new CategoryQuantity("Beauty", 4),
                new CategoryQuantity("Electronics", 3)
            ],
            result.TopCategories.Categories);
    }

    [Fact]
    public void Analyze_ShouldCalculateMonthlyAveragePrice()
    {
        var sales = CreateSales();

        var result = _service.Analyze(sales, null, null);

        Assert.Equal(
            [
                new MonthlyAveragePrice(2026, 1, 110m),
                new MonthlyAveragePrice(2026, 2, 32.5m)
            ],
            result.MonthlyAveragePrice.Months);
    }

    [Fact]
    public void Analyze_ShouldReturnTopFiveCustomersByAverageRating()
    {
        var sales = CreateSales();

        var result = _service.Analyze(sales, null, null);

        Assert.Equal(
            [
                new CustomerRating(3, 4.8m),
                new CustomerRating(1, 4.5m),
                new CustomerRating(4, 4.5m),
                new CustomerRating(5, 4.2m),
                new CustomerRating(6, 4.1m)
            ],
            result.TopCustomersByRating.Customers);
    }

    [Fact]
    public void Analyze_ShouldCalculateAverageDelivery()
    {
        var sales = CreateSales();

        var result = _service.Analyze(sales, null, null);

        Assert.Equal(8d, result.AverageDelivery.AverageDeliveryDays);
    }

    [Fact]
    public void Analyze_ShouldCalculateMonthlyCategoryDiscount()
    {
        var sales = CreateSales();

        var result = _service.Analyze(sales, null, null);

        Assert.Equal(
            [
                new MonthlyCategoryDiscount(2026, 1, "Books", 0.1m),
                new MonthlyCategoryDiscount(2026, 1, "Electronics", 0.15m),
                new MonthlyCategoryDiscount(2026, 2, "Beauty", 0.2m),
                new MonthlyCategoryDiscount(2026, 2, "Books", 0.3m),
                new MonthlyCategoryDiscount(2026, 2, "Clothing", 0.4m),
                new MonthlyCategoryDiscount(2026, 2, "Sports", 0.5m)
            ],
            result.MonthlyCategoryDiscount.Discounts);
    }

    [Fact]
    public void Analyze_ShouldFilterByStartAndEndDate()
    {
        var sales = CreateSales();

        var result = _service.Analyze(sales, new DateOnly(2026, 1, 10), new DateOnly(2026, 2, 5));

        Assert.Equal(
            [
                new CategorySales("Books", 210m),
                new CategorySales("Electronics", 200m)
            ],
            result.SalesByCategory.Categories);
    }

    [Fact]
    public void Analyze_ShouldIncludeStartDateAndExcludeEndDate()
    {
        var sales = CreateSales();

        var result = _service.Analyze(sales,new DateOnly(2026, 1, 10), new DateOnly(2026, 1, 15));

        var category = Assert.Single(result.SalesByCategory.Categories);

        Assert.Equal("Electronics", category.Category);
        Assert.Equal(200m, category.TotalSales);
    }

    [Fact]
    public async Task AnalyzeAsync_ShouldReturnSameResultAsAnalyze()
    {
        var sales = CreateSales();

        var syncResult = _service.Analyze(sales, null, null);
        var asyncResult = await _service.AnalyzeAsync(sales,null, null);

        AssertResultsEqual(syncResult, asyncResult);
    }

    [Fact]
    public async Task AnalyzeAsync_WithCancelledToken_ShouldThrowOperationCanceledException()
    {
        var sales = CreateSales();

        using var cancellationTokenSource = new CancellationTokenSource();

        cancellationTokenSource.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(
            () => _service.AnalyzeAsync(
                sales,
                null,
                null,
                cancellationTokenSource.Token));
    }

    [Fact]
    public void AnalyzeParallel_ShouldReturnSameResultAsAnalyze()
    {
        var sales = CreateSales();

        var syncResult = _service.Analyze(sales, null, null);
        var parallelResult = _service.AnalyzeParallel(sales,null, null);

        AssertResultsEqual(syncResult, parallelResult);
    }

    [Fact]
    public void Analyze_WithEmptyCollection_ShouldReturnEmptyAnalytics()
    {
        var sales = Array.Empty<Sale>();

        var result = _service.Analyze(sales, null, null);

        Assert.Empty(result.SalesByCategory.Categories);
        Assert.Empty(result.TopCategories.Categories);
        Assert.Empty(result.MonthlyAveragePrice.Months);
        Assert.Empty(result.TopCustomersByRating.Customers);
        Assert.Empty(result.MonthlyCategoryDiscount.Discounts);

        Assert.Equal(0d, result.AverageDelivery.AverageDeliveryDays);
    }

    private static void AssertResultsEqual(AnalyticsResult expected, AnalyticsResult actual)
    {
        Assert.Equal(expected.SalesByCategory.Categories, actual.SalesByCategory.Categories);

        Assert.Equal(expected.TopCategories.Categories, actual.TopCategories.Categories);

        Assert.Equal(expected.MonthlyAveragePrice.Months, actual.MonthlyAveragePrice.Months);

        Assert.Equal(expected.TopCustomersByRating.Customers, actual.TopCustomersByRating.Customers);

        Assert.Equal(expected.AverageDelivery, actual.AverageDelivery);

        Assert.Equal(expected.MonthlyCategoryDiscount.Discounts, actual.MonthlyCategoryDiscount.Discounts);
    }

    private static IReadOnlyCollection<Sale> CreateSales()
    {
        return
        [
            CreateSale(
                1,
                new DateOnly(2026, 1, 5),
                1,
                "Electronics",
                2,
                100m,
                0.1m,
                2,
                4.0m),

            CreateSale(
                2,
                new DateOnly(2026, 1, 10),
                1,
                "Electronics",
                1,
                200m,
                0.2m,
                4,
                5.0m),

            CreateSale(
                3,
                new DateOnly(2026, 1, 15),
                2,
                "Books",
                5,
                30m,
                0.1m,
                6,
                3.0m),

            CreateSale(
                4,
                new DateOnly(2026, 2, 1),
                3,
                "Books",
                2,
                30m,
                0.3m,
                8,
                4.8m),

            CreateSale(
                5,
                new DateOnly(2026, 2, 5),
                4,
                "Beauty",
                4,
                50m,
                0.2m,
                10,
                4.5m),

            CreateSale(
                6,
                new DateOnly(2026, 2, 10),
                5,
                "Clothing",
                2,
                40m,
                0.4m,
                12,
                4.2m),

            CreateSale(
                7,
                new DateOnly(2026, 2, 15),
                6,
                "Sports",
                6,
                10m,
                0.5m,
                14,
                4.1m)
        ];
    }

    private static Sale CreateSale(
        int orderId,
        DateOnly orderDate,
        int customerId,
        string category,
        int quantity,
        decimal unitPrice,
        decimal discount,
        int deliveryDays,
        decimal customerRating)
    {
        return new Sale
        {
            OrderId = orderId,
            OrderDate = orderDate,
            CustomerId = customerId,
            ProductCategory = category,
            Region = "Test",
            Quantity = quantity,
            UnitPrice = unitPrice,
            Discount = discount,
            PaymentMethod = "Card",
            DeliveryDays = deliveryDays,
            CustomerRating = customerRating,
            Revenue = unitPrice * quantity
        };
    }
}