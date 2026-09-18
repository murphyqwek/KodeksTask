using Core.Model;

namespace Core.Service.Parser;

public interface ISaleParser
{
    public Sale ParseToSale(string saleString);
}