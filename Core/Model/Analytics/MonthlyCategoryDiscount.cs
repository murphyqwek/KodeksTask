namespace Core.Model.Analytics;

public sealed record MonthlyCategoryDiscount(
    int Year,
    int Month,
    string Category,
    decimal AverageDiscount
);