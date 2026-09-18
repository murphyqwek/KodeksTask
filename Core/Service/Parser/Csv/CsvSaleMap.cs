using Core.Model;
using CsvHelper.Configuration;

public sealed class CsvSaleMap : ClassMap<Sale>
{
    public CsvSaleMap()
    {
        Map(x => x.OrderId)
            .Index(0)
            .Validate(
                args => !string.IsNullOrWhiteSpace(args.Field),
                args => "OrderId cannot be empty");

        Map(x => x.OrderDate)
            .Index(1)
            .Validate(
                args => !string.IsNullOrWhiteSpace(args.Field),
                args => "OrderDate cannot be empty")
            .TypeConverterOption.Format("yyyy-MM-dd");

        Map(x => x.CustomerId)
            .Index(2)
            .Validate(
                args => !string.IsNullOrWhiteSpace(args.Field),
                args => "CustomerId cannot be empty");

        Map(x => x.ProductCategory)
            .Index(3)
            .Validate(
                args => !string.IsNullOrWhiteSpace(args.Field),
                args => "ProductCategory cannot be empty");

        Map(x => x.Region)
            .Index(4)
            .Validate(
                args => !string.IsNullOrWhiteSpace(args.Field),
                args => "Region cannot be empty");

        Map(x => x.Discount)
            .Index(5)
            .Validate(
                args => !string.IsNullOrWhiteSpace(args.Field),
                args => "Discount cannot be empty");

        Map(x => x.PaymentMethod)
            .Index(6)
            .Validate(
                args => !string.IsNullOrWhiteSpace(args.Field),
                args => "PaymentMethod cannot be empty");

        Map(x => x.DeliveryDays)
            .Index(7)
            .Validate(
                args => !string.IsNullOrWhiteSpace(args.Field),
                args => "DeliveryDays cannot be empty");

        Map(x => x.CustomerRating)
            .Index(8)
            .Validate(
                args => !string.IsNullOrWhiteSpace(args.Field),
                args => "CustomerRating cannot be empty");

        Map(x => x.Revenue)
            .Index(9)
            .Validate(
                args => !string.IsNullOrWhiteSpace(args.Field),
                args => "Revenue cannot be empty");
    }
}