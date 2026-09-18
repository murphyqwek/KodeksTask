namespace Core.Model
{
    public class Sale
    {
        public int OrderId { get; init; }
        public DateOnly OrderDate { get; init; }
        public int CustomerId { get; init; }

        public required string ProductCategory { get; init; }
        public required string Region { get; init; }

        public decimal Discount { get; init; }

        public required string PaymentMethod { get; init; }

        public int DeliveryDays { get; init; }
        public decimal CustomerRating { get; init; }
        public decimal Revenue { get; init; }
    }
}
