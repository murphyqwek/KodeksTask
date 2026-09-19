using System.Text;
using Core.Model.Analytics;

namespace Core.Service.Writer;

public sealed class TextResultWriter : IResultWriter
{
    public void Write(
        AnalyticsResult result,
        TextWriter writer)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(writer);

        writer.Write(Format(result));
    }

    public async Task WriteAsync(
        AnalyticsResult result,
        TextWriter writer,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(writer);

        var text = Format(result);

        await writer.WriteAsync(text.AsMemory(), cancellationToken);
    }

    private static string Format(AnalyticsResult result)
    {
        var builder = new StringBuilder();

        builder.AppendLine("========================================");
        builder.AppendLine("         SALES ANALYTICS REPORT         ");
        builder.AppendLine("========================================");
        builder.AppendLine();

        AppendSalesByCategory(builder, result.SalesByCategory);
        AppendTopCategories(builder, result.TopCategories);
        AppendMonthlyAveragePrice(builder, result.MonthlyAveragePrice);
        AppendTopCustomers(builder, result.TopCustomersByRating);
        AppendAverageDelivery(builder, result.AverageDelivery);
        AppendMonthlyDiscount(builder, result.MonthlyCategoryDiscount);

        return builder.ToString();
    }

    private static void AppendSalesByCategory(
        StringBuilder builder,
        SalesByCategoryResult result)
    {
        builder.AppendLine("SALES BY CATEGORY");
        builder.AppendLine("----------------------------------------");

        foreach (var item in result.Categories)
        {
            builder.AppendLine($"{item.Category,-20} {item.TotalSales,15:N2}");
        }

        builder.AppendLine();
    }

    private static void AppendTopCategories(
        StringBuilder builder,
        TopCategoriesByQuantityResult result)
    {
        builder.AppendLine("TOP CATEGORIES BY QUANTITY");
        builder.AppendLine("----------------------------------------");

        var position = 1;

        foreach (var item in result.Categories)
        {
            builder.AppendLine($"{position,2}. {item.Category,-20} {item.TotalQuantity,10:N0}");

            position++;
        }

        builder.AppendLine();
    }

    private static void AppendMonthlyAveragePrice(
        StringBuilder builder,
        MonthlyAveragePriceResult result)
    {
        builder.AppendLine("MONTHLY AVERAGE PRICE");
        builder.AppendLine("----------------------------------------");

        foreach (var item in result.Months)
        {
            builder.AppendLine($"{item.Year}-{item.Month:D2}  {item.AveragePrice,15:N2}");
        }

        builder.AppendLine();
    }

    private static void AppendTopCustomers(
        StringBuilder builder,
        TopCustomersByRatingResult result)
    {
        builder.AppendLine("TOP CUSTOMERS BY RATING");
        builder.AppendLine("----------------------------------------");

        var position = 1;

        foreach (var item in result.Customers)
        {
            builder.AppendLine($"{position,2}. Customer {item.CustomerId,-10} {item.AverageRating:F2}");

            position++;
        }

        builder.AppendLine();
    }

    private static void AppendAverageDelivery(
        StringBuilder builder,
        AverageDeliveryResult result)
    {
        builder.AppendLine("AVERAGE DELIVERY");
        builder.AppendLine("----------------------------------------");

        builder.AppendLine($"Average delivery time: {result.AverageDeliveryDays:F2} days");

        builder.AppendLine();
    }

    private static void AppendMonthlyDiscount(
        StringBuilder builder,
        MonthlyCategoryDiscountResult result)
    {
        builder.AppendLine("MONTHLY AVERAGE DISCOUNT BY CATEGORY");
        builder.AppendLine("----------------------------------------");

        foreach (var item in result.Discounts)
        {
            builder.AppendLine($"{item.Year}-{item.Month:D2} {item.Category,-20} {item.AverageDiscount:P2}");
        }

        builder.AppendLine();
        builder.AppendLine("========================================");
    }
}