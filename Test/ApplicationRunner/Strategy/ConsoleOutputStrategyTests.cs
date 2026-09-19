using CLI.ApplicationRunner.Strategy.Output;
using Core.Service.Writer;
using Moq;
using Tests.Common;

namespace Tests.ApplicationRunner.Strategy;

public sealed class ConsoleOutputStrategyTests
{
    [Fact]
    public async Task ConsoleOutputStrategy_ShouldUseWrite()
    {
        var result = TestData.CreateAnalyticsResult();

        var writer = new Mock<IResultWriter>(
            MockBehavior.Strict);

        writer.Setup(x => x.Write(result, Console.Out));

        var strategy = new ConsoleOutputStrategy(
            writer.Object);

        await strategy.WriteAsync(result, null);

        writer.Verify(
            x => x.Write(
                result,
                Console.Out),
            Times.Once);
    }

    [Fact]
    public async Task ConsoleOutputStrategy_WhenCancelled_ShouldNotWrite()
    {
        var writer = new Mock<IResultWriter>(MockBehavior.Strict);

        var strategy = new ConsoleOutputStrategy(writer.Object);

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(
            () => strategy.WriteAsync(
                TestData.CreateAnalyticsResult(),
                null,
                cts.Token));

        writer.VerifyNoOtherCalls();
    }
}