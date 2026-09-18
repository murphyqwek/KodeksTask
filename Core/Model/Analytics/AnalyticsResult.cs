namespace Core.Model.Analytics;

public sealed record AnalyticsResult(
    SalesByCategoryResult SalesByCategory,
    TopCategoriesByQuantityResult TopCategories,
    MonthlyAveragePriceResult MonthlyAveragePrice,
    TopCustomersByRatingResult TopCustomersByRating,
    AverageDeliveryResult AverageDelivery,
    MonthlyCategoryDiscountResult MonthlyCategoryDiscount
);