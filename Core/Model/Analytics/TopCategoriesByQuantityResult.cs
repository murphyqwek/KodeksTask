namespace Core.Model.Analytics;

public sealed record TopCategoriesByQuantityResult(
    IReadOnlyList<CategoryQuantity> Categories
);