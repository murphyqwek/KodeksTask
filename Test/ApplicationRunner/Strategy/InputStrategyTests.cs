using CLI.ApplicationRunner.Strategy.Input;
using Core.Service.Csv;
using Moq;
using Tests.Common;

namespace Tests.ApplicationRunner.Strategy;

public sealed class InputStrategyTests
{
    [Fact]
    public async Task SyncStrategy_ShouldUseSynchronousCsvReader()
    {
        var sales = TestData.CreateSales();
        var textReader = new StringReader("test");

        var csvReader = new Mock<ICsvReader>(MockBehavior.Strict);

        csvReader.Setup(x => x.Read(textReader)).Returns(sales);

        string? receivedPath = null;

        var strategy = new SyncInputStrategy(
            csvReader.Object,
            path =>
            {
                receivedPath = path;
                return textReader;
            });

        var result = await strategy.ReadAsync("sales.csv");

        Assert.Same(sales, result);
        Assert.Equal("sales.csv", receivedPath);

        csvReader.Verify(x => x.Read(textReader), Times.Once);
    }

    [Fact]
    public async Task AsyncStrategy_ShouldUseAsynchronousCsvReader()
    {
        var sales = TestData.CreateSales();
        var textReader = new StringReader("test");

        using var cts = new CancellationTokenSource();
        var token = cts.Token;

        var csvReader = new Mock<ICsvReader>(MockBehavior.Strict);

        csvReader
            .Setup(x => x.ReadAsync(
                textReader,
                token))
            .ReturnsAsync(sales);

        string? receivedPath = null;

        var strategy = new AsyncInputStrategy(
            csvReader.Object,
            path =>
            {
                receivedPath = path;
                return textReader;
            });

        var result = await strategy.ReadAsync("sales.csv", token);

        Assert.Same(sales, result);
        Assert.Equal("sales.csv", receivedPath);

        csvReader.Verify(
            x => x.ReadAsync(
                textReader,
                token),
            Times.Once);
    }

    [Fact]
    public async Task SyncStrategy_WhenCancelled_ShouldNotCreateReader()
    {
        var factoryCalled = false;

        var strategy = new SyncInputStrategy(
            Mock.Of<ICsvReader>(),
            _ =>
            {
                factoryCalled = true;
                return new StringReader("");
            });

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(() => strategy.ReadAsync("sales.csv", cts.Token));

        Assert.False(factoryCalled);
    }

    [Fact]
    public async Task AsyncStrategy_WhenCancelled_ShouldNotCreateReader()
    {
        var factoryCalled = false;

        var strategy = new AsyncInputStrategy(
            Mock.Of<ICsvReader>(),
            _ =>
            {
                factoryCalled = true;
                return new StringReader("");
            });

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(
            () => strategy.ReadAsync(
                "sales.csv",
                cts.Token));

        Assert.False(factoryCalled);
    }
}