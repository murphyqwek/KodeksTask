using Core.Model;
using CsvHelper.Configuration;

namespace Core.Service.Parser.Csv;

public sealed class CsvSaleMap : ClassMap<Sale>
{
    public CsvSaleMap()
    {
        Map(x => x.OrderId)
            .Index(0)
            .Validate(args => !string.IsNullOrWhiteSpace(args.Field));

        Map(x => x.OrderDate)
            .Index(1)
            .Validate(args => !string.IsNullOrWhiteSpace(args.Field))
            .TypeConverterOption.Format("yyyy-MM-dd");

        Map(x => x.CustomerId)
            .Index(2)
            .Validate(args => !string.IsNullOrWhiteSpace(args.Field));

        Map(x => x.ProductCategory)
            .Index(3)
            .Validate(args => !string.IsNullOrWhiteSpace(args.Field));

        Map(x => x.Region)
            .Index(4)
            .Validate(args => !string.IsNullOrWhiteSpace(args.Field));

        Map(x => x.Discount)
            .Index(5)
            .Validate(args => !string.IsNullOrWhiteSpace(args.Field));

        Map(x => x.PaymentMethod)
            .Index(6)
            .Validate(args => !string.IsNullOrWhiteSpace(args.Field));

        Map(x => x.DeliveryDays)
            .Index(7)
            .Validate(args => !string.IsNullOrWhiteSpace(args.Field));

        Map(x => x.CustomerRating)
            .Index(8)
            .Validate(args => !string.IsNullOrWhiteSpace(args.Field));

        Map(x => x.Revenue)
            .Index(9)
            .Validate(args => !string.IsNullOrWhiteSpace(args.Field));
    }
}