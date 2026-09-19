using CLI.ApplicationRunner;
using CLI.CommandLine;

namespace CLI;

public static class ApplicationRunnerBuilderHelper
{
    public static ApplicationRunner.ApplicationRunner BuildApplicationRunner(CommandLineArguments arguments, ApplicationRunnerBuilder builder)
    {
        return arguments.Mode switch
        {
            ApplicationMode.Console => builder
                .UseSyncInput()
                .UseSequentialAnalytics()
                .WriteToConsole()
                .Build(),

            ApplicationMode.File => builder
                .UseSyncInput()
                .UseSequentialAnalytics()
                .WriteJson()
                .Build(),

            ApplicationMode.Async => builder
                .UseAsyncInput()
                .UseAsyncAnalytics()
                .WriteJsonAsync()
                .Build(),

            ApplicationMode.Parallel => builder
                .UseSyncInput()
                .UseParallelAnalytics()
                .WriteToConsole()
                .Build(),

            ApplicationMode.Di => builder
                .UseSyncInput()
                .UseSequentialAnalytics()
                .WriteToConsole()
                .Build(),

            ApplicationMode.Full => builder
                .UseAsyncInput()
                .UseParallelAnalytics()
                .WriteJsonAsync()
                .Build(),

            _ => throw new ArgumentOutOfRangeException(
                nameof(arguments.Mode),
                arguments.Mode,
                "Unsupported application mode.")
        };
    }
}
