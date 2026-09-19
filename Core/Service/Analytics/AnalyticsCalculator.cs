using Core.Model;
using Core.Model.Analytics;

namespace Core.Service.Analytics;

internal static class AnalyticsCalculator
{
    public static IReadOnlyCollection<Sale> FilterSales(IReadOnlyCollection<Sale> sales, DateOnly? startDate, DateOnly? endDate)
    {
        IEnumerable<Sale> query = sales;

        if (startDate.HasValue)
        {
            query = query.Where(sale => sale.OrderDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(sale => sale.OrderDate < endDate.Value);
        }

        return query.ToArray();
    }

    public static SalesByCategoryResult CalculateSalesByCategory(IReadOnlyCollection<Sale> sales)
    {
        var result = sales
            .GroupBy(sale => sale.ProductCategory)
            .Select(group => new CategorySales(
                group.Key,
                group.Sum(sale => sale.UnitPrice * sale.Quantity)))
            .OrderBy(x => x.Category)
            .ToArray();

        return new SalesByCategoryResult(result);
    }

    public static TopCategoriesByQuantityResult CalculateTopCategoriesByQuantity(IReadOnlyCollection<Sale> sales)
    {
        var result = sales
            .GroupBy(sale => sale.ProductCategory)
            .Select(group => new CategoryQuantity(
                group.Key,
                group.Sum(sale => sale.Quantity)))
            .OrderByDescending(x => x.TotalQuantity)
            .Take(4)
            .ToArray();

        return new TopCategoriesByQuantityResult(result);
    }

    public static MonthlyAveragePriceResult CalculateMonthlyAveragePrice(IReadOnlyCollection<Sale> sales)
    {
        var result = sales
            .GroupBy(sale => new
            {
                sale.OrderDate.Year,
                sale.OrderDate.Month
            })
            .Select(group => new MonthlyAveragePrice(
                group.Key.Year,
                group.Key.Month,
                group.Average(sale => sale.UnitPrice)))
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ToArray();

        return new MonthlyAveragePriceResult(result);
    }

    public static TopCustomersByRatingResult CalculateTopCustomersByRating(IReadOnlyCollection<Sale> sales)
    {
        var result = sales
            .GroupBy(sale => sale.CustomerId)
            .Select(group => new CustomerRating(
                group.Key,
                group.Average(sale => sale.CustomerRating)))
            .OrderByDescending(x => x.AverageRating)
            .ThenBy(result => result.CustomerId)
            .Take(5)
            .ToList();

        return new TopCustomersByRatingResult(result);
    }

    public static AverageDeliveryResult CalculateAverageDelivery(IReadOnlyCollection<Sale> sales)
    {
        var average = sales.Count == 0 ? 0 : sales.Average(sale => sale.DeliveryDays);

        return new AverageDeliveryResult(average);
    }

    public static MonthlyCategoryDiscountResult CalculateMonthlyCategoryDiscount(IReadOnlyCollection<Sale> sales)
    {
        var result = sales
            .GroupBy(sale => new
            {
                sale.OrderDate.Year,
                sale.OrderDate.Month,
                sale.ProductCategory
            })
            .Select(group => new MonthlyCategoryDiscount(
                group.Key.Year,
                group.Key.Month,
                group.Key.ProductCategory,
                group.Average(sale => sale.Discount)))
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ThenBy(x => x.Category)
            .ToArray();

        return new MonthlyCategoryDiscountResult(result);
    }
}