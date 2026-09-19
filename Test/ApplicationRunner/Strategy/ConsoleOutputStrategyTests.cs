using CLI.ApplicationRunner.Strategy.Output;
using Core.Service.Writer;

using static Test.ApplicationRunner.Strategy.AnalyticsResultTest;

namespace Tests.ApplicationRunner.Strategy;

public class ConsoleOutputStrategyTests
{
    [Fact]
    public async Task ConsoleOutputStrategy_ShouldWriteResultToConsole()
    {
        var resultWriter = new TextResultWriter();
        var strategy = new ConsoleOutputStrategy(resultWriter);

        using var output = new StringWriter();
        var originalOutput = Console.Out;

        try
        {
            Console.SetOut(output);

            var result = CreateResult();

            await strategy.WriteAsync(result, null);

            var text = output.ToString();

            Assert.False(string.IsNullOrWhiteSpace(text));
        }
        finally
        {
            Console.SetOut(originalOutput);
        }
    }
}