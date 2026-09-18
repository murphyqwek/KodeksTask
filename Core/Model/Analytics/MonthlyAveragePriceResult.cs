namespace Core.Model.Analytics;

public sealed record MonthlyAveragePriceResult(
    IReadOnlyList<MonthlyAveragePrice> Months
);