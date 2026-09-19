using CLI.ApplicationRunner;
using CLI.ApplicationRunner.Strategy.Analytics;
using CLI.ApplicationRunner.Strategy.Input;
using CLI.ApplicationRunner.Strategy.Output;
using Core.Model;
using Moq;
using Tests.Common;

namespace Tests.ApplicationRunner;

public sealed class ApplicationRunnerTests
{
    [Fact]
    public async Task RunAsync_ShouldExecuteEntirePipeline()
    {
        var sales = TestData.CreateSales();
        var analyticsResult = TestData.CreateAnalyticsResult();

        var options = new ApplicationRunOptions(
            "sales.csv",
            "result.json",
            new DateOnly(2026, 1, 1),
            new DateOnly(2026, 10, 1));

        using var cts = new CancellationTokenSource();
        var token = cts.Token;

        var input = new Mock<IInputStrategy>(MockBehavior.Strict);
        var analytics = new Mock<IAnalyticsStrategy>(MockBehavior.Strict);
        var output = new Mock<IOutputStrategy>(MockBehavior.Strict);

        input
            .Setup(x => x.ReadAsync(
                options.InputPath,
                token))
            .ReturnsAsync(sales);

        analytics
            .Setup(x => x.AnalyzeAsync(
                It.Is<IReadOnlyCollection<Sale>>(
                    s => ReferenceEquals(s, sales)),
                options.StartDate,
                options.EndDate,
                token))
            .ReturnsAsync(analyticsResult);

        output
            .Setup(x => x.WriteAsync(
                analyticsResult,
                options.OutputPath,
                token))
            .Returns(Task.CompletedTask);

        var runner = new CLI.ApplicationRunner.ApplicationRunner(
            input.Object,
            analytics.Object,
            output.Object);

        await runner.RunAsync(options, token);

        input.VerifyAll();
        analytics.VerifyAll();
        output.VerifyAll();
    }

    [Fact]
    public async Task RunAsync_WhenInputPathIsEmpty_ShouldThrowArgumentException()
    {
        var input = new Mock<IInputStrategy>();
        var analytics = new Mock<IAnalyticsStrategy>();
        var output = new Mock<IOutputStrategy>();

        var runner = new CLI.ApplicationRunner.ApplicationRunner(
            input.Object,
            analytics.Object,
            output.Object);

        var options = new ApplicationRunOptions(
            "",
            null,
            null,
            null);

        await Assert.ThrowsAsync<ArgumentException>(
            () => runner.RunAsync(options));

        input.Verify(
            x => x.ReadAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task RunAsync_WhenStartDateEqualsEndDate_ShouldThrowArgumentException()
    {
        var input = new Mock<IInputStrategy>();
        var analytics = new Mock<IAnalyticsStrategy>();
        var output = new Mock<IOutputStrategy>();

        var runner = new CLI.ApplicationRunner.ApplicationRunner(
            input.Object,
            analytics.Object,
            output.Object);

        var date = new DateOnly(2026, 9, 1);

        var options = new ApplicationRunOptions(
            "sales.csv",
            null,
            date,
            date);

        await Assert.ThrowsAsync<ArgumentException>(() => runner.RunAsync(options));

        input.Verify(
            x => x.ReadAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task RunAsync_WhenStartDateIsAfterEndDate_ShouldThrowArgumentException()
    {
        var input = new Mock<IInputStrategy>();
        var analytics = new Mock<IAnalyticsStrategy>();
        var output = new Mock<IOutputStrategy>();

        var runner = new CLI.ApplicationRunner.ApplicationRunner(
            input.Object,
            analytics.Object,
            output.Object);

        var options = new ApplicationRunOptions(
            "sales.csv",
            null,
            new DateOnly(2026, 10, 1),
            new DateOnly(2026, 9, 1));

        await Assert.ThrowsAsync<ArgumentException>(() => runner.RunAsync(options));
    }

    [Fact]
    public async Task RunAsync_WhenOptionsAreNull_ShouldThrowArgumentNullException()
    {
        var runner = new CLI.ApplicationRunner.ApplicationRunner(
            Mock.Of<IInputStrategy>(),
            Mock.Of<IAnalyticsStrategy>(),
            Mock.Of<IOutputStrategy>());

        await Assert.ThrowsAsync<ArgumentNullException>(() => runner.RunAsync(null!));
    }
}