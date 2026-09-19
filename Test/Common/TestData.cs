using Core.Model;
using Core.Model.Analytics;

namespace Tests.Common;

internal static class TestData
{
    public static IReadOnlyList<Sale> CreateSales()
    {
        return
        [
            new Sale
            {
                OrderId = 1,
                OrderDate = new DateOnly(2026, 9, 1),
                CustomerId = 10,
                ProductCategory = "Electronics",
                Region = "North",
                Quantity = 2,
                UnitPrice = 100m,
                Discount = 0.1m,
                PaymentMethod = "Card",
                DeliveryDays = 3,
                CustomerRating = 4.5m,
                Revenue = 180m
            }
        ];
    }

    public static AnalyticsResult CreateAnalyticsResult()
    {
        // Здесь содержание не важно, т.кю orchestration-тесты проверяют передачу самого объекта
        return new AnalyticsResult(
            null!,
            null!,
            null!,
            null!,
            null!,
            null!);
    }
}