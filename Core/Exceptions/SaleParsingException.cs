namespace Core.Exceptions;

public sealed class SaleParsingException : Exception
{
    public SaleParsingException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
