using System.Text.Json;
using Core.Model.Analytics;
using Core.Service.Writer;
using static Test.Service.Writer.TestAnalyticsResult;

namespace Tests.Service.Writer;


public sealed class JsonResultWriterTests
{
    private readonly JsonResultWriter _writer = new();

    [Fact]
    public void Write_ShouldWriteValidJson()
    {
        var result = CreateAnalyticsResult();

        using var textWriter = new StringWriter();

        _writer.Write(result, textWriter);

        var json = textWriter.ToString();

        using var document = JsonDocument.Parse(json);

        Assert.Equal(JsonValueKind.Object, document.RootElement.ValueKind);
    }

    [Fact]
    public void Write_ShouldUseCamelCasePropertyNames()
    {
        var result = CreateAnalyticsResult();

        using var textWriter = new StringWriter();

        _writer.Write(result, textWriter);

        var json = textWriter.ToString();

        using var document = JsonDocument.Parse(json);

        Assert.True(document.RootElement.TryGetProperty("salesByCategory", out _));

        Assert.False(document.RootElement.TryGetProperty("SalesByCategory", out _));
    }

    [Fact]
    public void Write_ShouldWriteAnalyticsValues()
    {
        var result = CreateAnalyticsResult();

        using var textWriter = new StringWriter();

        _writer.Write(result, textWriter);

        var json = textWriter.ToString();

        using var document = JsonDocument.Parse(json);

        var root = document.RootElement;

        var categories = root.GetProperty("salesByCategory").GetProperty("categories");

        Assert.Equal(2, categories.GetArrayLength());

        var firstCategory = categories[0];

        Assert.Equal("Electronics", firstCategory.GetProperty("category").GetString());

        Assert.Equal(1000m, firstCategory.GetProperty("totalSales").GetDecimal());
    }

    [Fact]
    public async Task WriteAsync_ShouldWriteValidJson()
    {
        var result = CreateAnalyticsResult();

        using var textWriter = new StringWriter();

        await _writer.WriteAsync(result, textWriter);

        var json = textWriter.ToString();

        using var document = JsonDocument.Parse(json);

        Assert.Equal(JsonValueKind.Object, document.RootElement.ValueKind);
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

        var act = () => _writer.Write(null!, textWriter);

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

        var act = async () =>await _writer.WriteAsync(result, null!);

        await Assert.ThrowsAsync<ArgumentNullException>(act);
    }
}