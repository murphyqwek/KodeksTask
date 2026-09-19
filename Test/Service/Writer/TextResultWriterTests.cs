using Core.Service.Writer;
using static Test.Service.Writer.TestAnalyticsResult;

namespace Tests.Service.Writer;

public sealed class TextResultWriterTests
{
    private readonly TextResultWriter _writer = new();

    [Fact]
    public void Write_ShouldWriteReportHeader()
    {
        var result = CreateAnalyticsResult();

        using var textWriter = new StringWriter();

        _writer.Write(result, textWriter);

        var output = textWriter.ToString();

        Assert.Contains("SALES ANALYTICS REPORT", output);
    }

    [Fact]
    public void Write_ShouldWriteAllSections()
    {
        var result = CreateAnalyticsResult();

        using var textWriter = new StringWriter();

        _writer.Write(result, textWriter);

        var output = textWriter.ToString();

        Assert.Contains("SALES BY CATEGORY", output);

        Assert.Contains("TOP CATEGORIES BY QUANTITY", output);

        Assert.Contains("MONTHLY AVERAGE PRICE", output);

        Assert.Contains("TOP CUSTOMERS BY RATING", output);

        Assert.Contains("AVERAGE DELIVERY", output);

        Assert.Contains("MONTHLY AVERAGE DISCOUNT BY CATEGORY", output);
    }

    [Fact]
    public void Write_ShouldWriteCategories()
    {
        var result = CreateAnalyticsResult();

        using var textWriter = new StringWriter();

        _writer.Write(result, textWriter);

        var output = textWriter.ToString();

        Assert.Contains("Electronics", output);

        Assert.Contains("Books", output);
    }

    [Fact]
    public void Write_ShouldWriteMonthlyDates()
    {
        var result = CreateAnalyticsResult();

        using var textWriter = new StringWriter();

        _writer.Write(result, textWriter);

        var output = textWriter.ToString();

        Assert.Contains("2026-01", output);

        Assert.Contains("2026-02", output);
    }

    [Fact]
    public void Write_ShouldWriteCustomerIds()
    {
        var result = CreateAnalyticsResult();

        using var textWriter = new StringWriter();

        _writer.Write(result, textWriter);

        var output = textWriter.ToString();

        Assert.Contains("Customer 1", output);

        Assert.Contains("Customer 2", output);
    }

    [Fact]
    public void Write_ShouldWriteAverageDelivery()
    {
        var result = CreateAnalyticsResult();

        using var textWriter = new StringWriter();

        _writer.Write(result, textWriter);

        var output = textWriter.ToString();

        Assert.Contains("Average delivery time:", output);

        Assert.Contains("days", output);
    }

    [Fact]
    public async Task WriteAsync_ShouldProduceSameResultAsWrite()
    {
        var result = CreateAnalyticsResult();

        using var syncWriter = new StringWriter();
        using var asyncWriter = new StringWriter();

        _writer.Write(result, syncWriter);

        await _writer.WriteAsync(result, asyncWriter);

        Assert.Equal(syncWriter.ToString(), asyncWriter.ToString());
    }

    [Fact]
    public void Write_WhenResultIsNull_ShouldThrowArgumentNullException()
    {
        using var textWriter = new StringWriter();

        var act = () =>
            _writer.Write(null!, textWriter);

        Assert.Throws<ArgumentNullException>(act);
    }

    [Fact]
    public void Write_WhenWriterIsNull_ShouldThrowArgumentNullException()
    {
        var result = CreateAnalyticsResult();

        var act = () =>
            _writer.Write(result, null!);

        Assert.Throws<ArgumentNullException>(act);
    }

    [Fact]
    public async Task WriteAsync_WhenResultIsNull_ShouldThrowArgumentNullException()
    {
        using var textWriter = new StringWriter();

        var act = async () => await _writer.WriteAsync(null!, textWriter);

        await Assert.ThrowsAsync<ArgumentNullException>(act);
    }

    [Fact]
    public async Task WriteAsync_WhenWriterIsNull_ShouldThrowArgumentNullException()
    {
        var result = CreateAnalyticsResult();

        var act = async () => await _writer.WriteAsync(result, null!);

        await Assert.ThrowsAsync<ArgumentNullException>(act);
    }
}