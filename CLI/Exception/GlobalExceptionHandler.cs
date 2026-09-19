using CLI.Exceptions;
using Core.Exceptions;

namespace CLI;

public static class GlobalExceptionHandler
{
    public static int Handle(Exception exception)
    {
        return exception switch
        {
            InputFileException ex => HandleInputFileException(ex),

            OutputFileException ex => HandleOutputFileException(ex),

            SaleParsingException ex => HandleSaleParsingException(ex),

            ArgumentException ex => WriteError($"Invalid arguments: {ex.Message}"),

            OperationCanceledException => WriteError("Operation cancelled"),

            _ => HandleUnexpectedException(exception)
        };
    }

    private static int HandleSaleParsingException(SaleParsingException exception)
    {
        return WriteError($"Failed to parse sales data: {exception.Message}");
    }

    private static int HandleInputFileException(InputFileException exception)
    {
        var message = exception.InnerException switch
        {
            FileNotFoundException => $"Input file was not found: '{exception.Path}'",

            DirectoryNotFoundException => $"Input directory was not found: '{exception.Path}'",

            UnauthorizedAccessException => $"No permission to read input file: '{exception.Path}'",

            IOException ex => $"Failed to read input file '{exception.Path}': {ex.Message}",

            _ => $"Failed to read input file: '{exception.Path}'"
        };

        return WriteError(message);
    }

    private static int HandleOutputFileException(OutputFileException exception)
    {
        var message = exception.InnerException switch
        {
            DirectoryNotFoundException =>$"Output directory does not exist: '{exception.Path}'",

            UnauthorizedAccessException => $"No permission to create or write output file: '{exception.Path}'",

            IOException ex => $"Failed to write output file '{exception.Path}': {ex.Message}",

            _ => $"Failed to write output file: '{exception.Path}'"
        };

        return WriteError(message);
    }

    private static int HandleUnexpectedException(Exception exception)
    {
        Console.Error.WriteLine($"Unexpected error: {exception.Message}");

        return 1;
    }

    private static int WriteError(string message)
    {
        Console.Error.WriteLine($"Error: {message}");

        return 1;
    }
}