using Core.Model.Analytics;

namespace Test.ApplicationRunner.Strategy;

public static class AnalyticsResultTest
{
    public static AnalyticsResult CreateResult()
    {
        return new AnalyticsResult(
            new SalesByCategoryResult(
            [
                new CategorySales("Electronics", 12500m),
            new CategorySales("Books", 4300m)
            ]),

            new TopCategoriesByQuantityResult(
            [
                new CategoryQuantity("Electronics", 42),
            new CategoryQuantity("Books", 27)
            ]),

            new MonthlyAveragePriceResult(
            [
                new MonthlyAveragePrice(2026, 1, 149.99m),
            new MonthlyAveragePrice(2026, 2, 175.50m)
            ]),

            new TopCustomersByRatingResult(
            [
                new CustomerRating(101, 4.9m),
            new CustomerRating(205, 4.7m)
            ]),

            new AverageDeliveryResult(3.5),

            new MonthlyCategoryDiscountResult(
            [
                new MonthlyCategoryDiscount(
                2026,
                1,
                "Electronics",
                0.10m),

            new MonthlyCategoryDiscount(
                2026,
                1,
                "Books",
                0.05m)
            ])
        );
    }
}
