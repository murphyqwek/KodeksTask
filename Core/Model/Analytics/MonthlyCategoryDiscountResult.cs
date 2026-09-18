namespace Core.Model.Analytics;

public sealed record MonthlyCategoryDiscountResult(
    IReadOnlyList<MonthlyCategoryDiscount> Discounts
);