using CLI.ApplicationRunner.Strategy.Output;
using Core.Service.Writer;
using Moq;
using Tests.Common;

namespace Tests.ApplicationRunner.Strategy;

public sealed class JsonFileOutputStrategyTests
{
    [Fact]
    public async Task SyncStrategy_ShouldWriteToCreatedWriter()
    {
        var result = TestData.CreateAnalyticsResult();
        var textWriter = new StringWriter();

        string? receivedPath = null;

        var strategy = new JsonFileOutputStrategy(
            new JsonResultWriter(),
            path =>
            {
                receivedPath = path;
                return textWriter;
            });

        await strategy.WriteAsync(result, "result.json");

        Assert.Equal("result.json", receivedPath);
        Assert.False(string.IsNullOrWhiteSpace(textWriter.ToString()));
    }

    [Fact]
    public async Task AsyncStrategy_ShouldWriteAsynchronously()
    {
        var result = TestData.CreateAnalyticsResult();
        var textWriter = new StringWriter();

        using var cts = new CancellationTokenSource();

        string? receivedPath = null;

        var strategy = new JsonFileOutputAsyncStrategy(
            new JsonResultWriter(),
            path =>
            {
                receivedPath = path;
                return textWriter;
            });

        await strategy.WriteAsync(
            result,
            "result.json",
            cts.Token);

        Assert.Equal("result.json", receivedPath);
        Assert.False(string.IsNullOrWhiteSpace(textWriter.ToString()));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task SyncStrategy_WhenOutputPathIsMissing_ShouldThrow(
    string? outputPath)
    {
        var strategy = new JsonFileOutputStrategy(new JsonResultWriter(), _ => new StringWriter());

        await Assert.ThrowsAsync<ArgumentException>(
            () => strategy.WriteAsync(
                TestData.CreateAnalyticsResult(),
                outputPath));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task AsyncStrategy_WhenOutputPathIsMissing_ShouldThrow(
    string? outputPath)
    {
        var strategy = new JsonFileOutputAsyncStrategy(new JsonResultWriter(), _ => new StringWriter());

        await Assert.ThrowsAsync<ArgumentException>(
            () => strategy.WriteAsync(
                TestData.CreateAnalyticsResult(),
                outputPath));
    }

    [Fact]
    public async Task AsyncStrategy_WhenCancelled_ShouldThrowOperationCanceledException()
    {
        var result = TestData.CreateAnalyticsResult();
        var textWriter = new StringWriter();

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        var strategy = new JsonFileOutputAsyncStrategy(new JsonResultWriter(), _ => textWriter);

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => strategy.WriteAsync(
                result,
                "result.json",
                cts.Token));
    }
}