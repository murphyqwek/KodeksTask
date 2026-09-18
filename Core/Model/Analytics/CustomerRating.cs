namespace Core.Model.Analytics;

public sealed record CustomerRating(
    int CustomerId,
    decimal AverageRating
);