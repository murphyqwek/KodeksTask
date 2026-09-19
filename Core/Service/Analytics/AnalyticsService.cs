using Core.Model;
using Core.Model.Analytics;

namespace Core.Service.Analytics;

public class AnalyticsService : IAnalyticsService
{
    public AnalyticsResult Analyze(IReadOnlyCollection<Sale> sales, DateOnly? startDate, DateOnly? endDate)
    {
        var filteredSales = AnalyticsCalculator.FilterSales(sales, startDate, endDate);

        SalesByCategoryResult salesByCategory = AnalyticsCalculator.CalculateSalesByCategory(filteredSales);
        TopCategoriesByQuantityResult topCategories = AnalyticsCalculator.CalculateTopCategoriesByQuantity(filteredSales);
        MonthlyAveragePriceResult monthlyAveragePrice = AnalyticsCalculator.CalculateMonthlyAveragePrice(filteredSales);
        TopCustomersByRatingResult topCustomersByRating = AnalyticsCalculator.CalculateTopCustomersByRating(filteredSales);
        AverageDeliveryResult averageDelivery = AnalyticsCalculator.CalculateAverageDelivery(filteredSales);
        MonthlyCategoryDiscountResult monthlyCategoryDiscount = AnalyticsCalculator.CalculateMonthlyCategoryDiscount(filteredSales);

        var result = new AnalyticsResult(
            salesByCategory,
            topCategories,
            monthlyAveragePrice,
            topCustomersByRating,
            averageDelivery,
            monthlyCategoryDiscount
        );

        return result;
    }

    public async Task<AnalyticsResult> AnalyzeAsync(IReadOnlyCollection<Sale> sales, DateOnly? startDate, DateOnly? endDate, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await Task.FromResult(Analyze(sales, startDate, endDate));
    }

    public AnalyticsResult AnalyzeParallel(IReadOnlyCollection<Sale> sales, DateOnly? startDate, DateOnly? endDate)
    {
        var filteredSales = AnalyticsParallelCalculator.FilterSales(sales, startDate, endDate);

        SalesByCategoryResult salesByCategory = AnalyticsParallelCalculator.CalculateSalesByCategory(filteredSales);
        TopCategoriesByQuantityResult topCategories = AnalyticsParallelCalculator.CalculateTopCategoriesByQuantity(filteredSales);
        MonthlyAveragePriceResult monthlyAveragePrice = AnalyticsParallelCalculator.CalculateMonthlyAveragePrice(filteredSales);
        TopCustomersByRatingResult topCustomersByRating = AnalyticsParallelCalculator.CalculateTopCustomersByRating(filteredSales);
        AverageDeliveryResult averageDelivery = AnalyticsParallelCalculator.CalculateAverageDelivery(filteredSales);
        MonthlyCategoryDiscountResult monthlyCategoryDiscount = AnalyticsParallelCalculator.CalculateMonthlyCategoryDiscount(filteredSales);

        var result = new AnalyticsResult(
            salesByCategory,
            topCategories,
            monthlyAveragePrice,
            topCustomersByRating,
            averageDelivery,
            monthlyCategoryDiscount
        );

        return result;
    }
}