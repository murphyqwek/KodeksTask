using CLI.ApplicationRunner.Strategy.Analytics;
using Core.Model;
using Core.Service.Analytics;
using Moq;
using Tests.Common;

namespace Tests.ApplicationRunner.Strategy;

public sealed class AnalyticsStrategyTests
{
    [Fact]
    public async Task SequentialStrategy_ShouldUseAnalyze()
    {
        var sales = TestData.CreateSales();
        var expected = TestData.CreateAnalyticsResult();

        var startDate = new DateOnly(2026, 1, 1);
        var endDate = new DateOnly(2026, 10, 1);

        var service = new Mock<IAnalyticsService>(MockBehavior.Strict);

        service
            .Setup(x => x.Analyze(
                sales,
                startDate,
                endDate))
            .Returns(expected);

        var strategy = new SequentialAnalyticsStrategy(service.Object);

        var result = await strategy.AnalyzeAsync(
            sales,
            startDate,
            endDate);

        Assert.Same(expected, result);

        service.Verify(
            x => x.Analyze(
                sales,
                startDate,
                endDate),
            Times.Once);
    }

    [Fact]
    public async Task ParallelStrategy_ShouldUseAnalyzeParallel()
    {
        var sales = TestData.CreateSales();
        var expected = TestData.CreateAnalyticsResult();

        var startDate = new DateOnly(2026, 1, 1);
        var endDate = new DateOnly(2026, 10, 1);

        var service = new Mock<IAnalyticsService>(MockBehavior.Strict);

        service
            .Setup(x => x.AnalyzeParallel(
                sales,
                startDate,
                endDate))
            .Returns(expected);

        var strategy = new ParallelAnalyticsStrategy(service.Object);

        var result = await strategy.AnalyzeAsync(
            sales,
            startDate,
            endDate);

        Assert.Same(expected, result);

        service.Verify(
            x => x.AnalyzeParallel(
                sales,
                startDate,
                endDate),
            Times.Once);
    }

    [Fact]
    public async Task SequentialStrategy_WhenCancelled_ShouldNotCallService()
    {
        var service = new Mock<IAnalyticsService>(MockBehavior.Strict);

        var strategy = new SequentialAnalyticsStrategy(service.Object);

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(
            () => strategy.AnalyzeAsync(
                TestData.CreateSales(),
                null,
                null,
                cts.Token));

        service.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ParallelStrategy_WhenCancelled_ShouldNotCallService()
    {
        var service = new Mock<IAnalyticsService>(
            MockBehavior.Strict);

        var strategy = new ParallelAnalyticsStrategy(
            service.Object);

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(
            () => strategy.AnalyzeAsync(
                TestData.CreateSales(),
                null,
                null,
                cts.Token));

        service.VerifyNoOtherCalls();
    }
}