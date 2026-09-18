namespace Core.Model.Analytics;

public sealed record MonthlyAveragePrice(
    int Year,
    int Month,
    decimal AveragePrice
);