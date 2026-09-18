namespace Core.Model.Analytics;

public sealed record SalesByCategoryResult(
    IReadOnlyList<CategorySales> Categories
);
