using CLI.CommandLine;

namespace CLI;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        try
        {
            using var cancellationTokenSource = new CancellationTokenSource();

            Console.CancelKeyPress += (_, eventArgs) =>
            {
                eventArgs.Cancel = true;
                cancellationTokenSource.Cancel();
            };

            var parser = new CommandLineParser();

            var arguments = parser.Parse(args);

            if (arguments.IsHelp)
            {
                PrintHelp();
                return 0;
            }

            var runner = BuildApplicationRunner(arguments);

            await runner.RunAsync(arguments.Options, cancellationTokenSource.Token);

            return 0;
        }
        catch (Exception exception)
        {
            return GlobalExceptionHandler.Handle(exception);
        }
    }

    private static ApplicationRunner.ApplicationRunner BuildApplicationRunner(CommandLineArguments arguments)
    {
        throw new NotImplementedException();
    }

    private static void PrintHelp()
    {
        Console.WriteLine("""
            Kodeks Test Task - Sales Analyzer

            Usage:
              SalesAnalyzer <mode> --input <path> [options]

            Modes:
                console     Synchronous CSV reading, calculation of three analytics, and output to the console
                file        Same as console, but the results are saved to a JSON file. The output path is specified via --output
                async       Asynchronous file reading using StreamReader with ReadLineAsync, and asynchronous JSON writing
                parallel    Parallel processing of records when calculating analytics
                di          Use a DI container to register all services
                full        Combined mode: asynchronous I/O + parallel analytics calculation + dependency injection

            Options:
              --input <path>        Input CSV file
              --output <path>       Output file
              --start-date <date>   Start date
              --end-date <date>     End date
              --help                Show help
            """);
    }
}