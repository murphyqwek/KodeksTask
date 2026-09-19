using Core.Model.Analytics;

namespace Test.Service.Writer;

public static class TestAnalyticsResult
{
    public static AnalyticsResult CreateAnalyticsResult()
    {
        return new AnalyticsResult(
            new SalesByCategoryResult(
            [
                new CategorySales(
                    "Electronics",
                    1000m),

                new CategorySales(
                    "Books",
                    500m)
            ]),

            new TopCategoriesByQuantityResult(
            [
                new CategoryQuantity(
                    "Electronics",
                    20),

                new CategoryQuantity(
                    "Books",
                    10)
            ]),

            new MonthlyAveragePriceResult(
            [
                new MonthlyAveragePrice(
                    2026,
                    1,
                    125.50m),

                new MonthlyAveragePrice(
                    2026,
                    2,
                    150m)
            ]),

            new TopCustomersByRatingResult(
            [
                new CustomerRating(
                    1,
                    4.9m),

                new CustomerRating(
                    2,
                    4.8m)
            ]),

            new AverageDeliveryResult(
                3.5),

            new MonthlyCategoryDiscountResult(
            [
                new MonthlyCategoryDiscount(
                    2026,
                    1,
                    "Electronics",
                    0.1m),

                new MonthlyCategoryDiscount(
                    2026,
                    1,
                    "Books",
                    0.05m)
            ]));
    }
}
