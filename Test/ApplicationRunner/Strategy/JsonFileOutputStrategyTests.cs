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

        var writer = new Mock<JsonResultWriter>(MockBehavior.Strict);

        writer.Setup(x => x.Write(result, textWriter));

        var strategy = new JsonFileOutputStrategy(
            writer.Object,
            path =>
            {
                receivedPath = path;
                return textWriter;
            });

        await strategy.WriteAsync(result, "result.json");

        Assert.Equal("result.json", receivedPath);

        writer.Verify(
            x => x.Write(
                result,
                textWriter),
            Times.Once);
    }

    [Fact]
    public async Task AsyncStrategy_ShouldWriteAsynchronously()
    {
        var result = TestData.CreateAnalyticsResult();
        var textWriter = new StringWriter();

        using var cts = new CancellationTokenSource();
        var token = cts.Token;

        string? receivedPath = null;

        var writer = new Mock<JsonResultWriter>(MockBehavior.Strict);

        writer
            .Setup(x => x.WriteAsync(
                result,
                textWriter,
                token))
            .Returns(Task.CompletedTask);

        var strategy = new JsonFileOutputAsyncStrategy(
            writer.Object,
            path =>
            {
                receivedPath = path;
                return textWriter;
            });

        await strategy.WriteAsync(
            result,
            "result.json",
            token);

        Assert.Equal("result.json", receivedPath);

        writer.VerifyAll();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task SyncStrategy_WhenOutputPathIsMissing_ShouldThrow(string? outputPath)
    {
        var strategy = new JsonFileOutputStrategy(Mock.Of<JsonResultWriter>(), _ => new StringWriter());

        await Assert.ThrowsAsync<ArgumentException>(
            () => strategy.WriteAsync(
                TestData.CreateAnalyticsResult(),
                outputPath));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task AsyncStrategy_WhenOutputPathIsMissing_ShouldThrow(string? outputPath)
    {
        var strategy = new JsonFileOutputAsyncStrategy(Mock.Of<JsonResultWriter>(), _ => new StringWriter());

        await Assert.ThrowsAsync<ArgumentException>(
            () => strategy.WriteAsync(
                TestData.CreateAnalyticsResult(),
                outputPath));
    }

    [Fact]
    public async Task AsyncStrategy_ShouldPassCancellationTokenToWriter()
    {
        var result = TestData.CreateAnalyticsResult();
        var textWriter = new StringWriter();

        using var cts = new CancellationTokenSource();
        var token = cts.Token;

        var writer = new Mock<JsonResultWriter>(MockBehavior.Strict);

        writer
            .Setup(x => x.WriteAsync(
                result,
                textWriter,
                token))
            .Returns(Task.CompletedTask);

        var strategy = new JsonFileOutputAsyncStrategy(writer.Object, _ => textWriter);

        await strategy.WriteAsync(
            result,
            "result.json",
            token);

        writer.VerifyAll();
    }
}