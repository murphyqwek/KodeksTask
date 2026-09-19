using CLI.ApplicationRunner;
using Core.Service.Analytics;
using Core.Service.Csv;
using Core.Service.Writer;
using Moq;

namespace Tests.ApplicationRunner;

public sealed class ApplicationRunnerBuilderTests
{
    private static ApplicationRunnerBuilder CreateBuilder()
    {
        return new ApplicationRunnerBuilder(
            Mock.Of<ICsvReader>(),
            Mock.Of<IAnalyticsService>(),
            new TextResultWriter(),
            new JsonResultWriter());
    }

    [Fact]
    public void Build_WhenInputStrategyIsNotConfigured_ShouldThrow()
    {
        var builder = CreateBuilder();

        var exception = Assert.Throws<InvalidOperationException>(() => builder.Build());

        Assert.Equal("Input strategy is not configured", exception.Message);
    }

    [Fact]
    public void Build_WhenAnalyticsStrategyIsNotConfigured_ShouldThrow()
    {
        var builder = CreateBuilder()
            .UseSyncInput();

        var exception = Assert.Throws<InvalidOperationException>(() => builder.Build());

        Assert.Equal("Analytics strategy is not configured", exception.Message);
    }

    [Fact]
    public void Build_WhenOutputStrategyIsNotConfigured_ShouldThrow()
    {
        var builder = CreateBuilder()
            .UseSyncInput()
            .UseSequentialAnalytics();

        var exception = Assert.Throws<InvalidOperationException>(() => builder.Build());

        Assert.Equal("Output strategy is not configured", exception.Message);
    }

    [Fact]
    public void Build_WhenEverythingIsConfigured_ShouldReturnRunner()
    {
        var builder = CreateBuilder()
            .UseAsyncInput()
            .UseParallelAnalytics()
            .WriteJsonAsync();

        var runner = builder.Build();

        Assert.NotNull(runner);
    }

    [Fact]
    public void ConfigurationMethods_ShouldReturnSameBuilder()
    {
        var builder = CreateBuilder();

        Assert.Same(builder, builder.UseSyncInput());
        Assert.Same(builder, builder.UseAsyncInput());

        Assert.Same(builder, builder.UseSequentialAnalytics());
        Assert.Same(builder, builder.UseParallelAnalytics());

        Assert.Same(builder, builder.WriteToConsole());
        Assert.Same(builder, builder.WriteJson());
        Assert.Same(builder, builder.WriteJsonAsync());
    }
}