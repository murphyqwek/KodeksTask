namespace Core.Model.Analytics;

public sealed record TopCustomersByRatingResult(
    IReadOnlyList<CustomerRating> Customers
);
