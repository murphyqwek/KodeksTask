using System.Text.Json;
using Core.Model.Analytics;

namespace Core.Service.Writer;

public sealed class JsonResultWriter : IResultWriter
{
    private readonly JsonSerializerOptions _options;

    public JsonResultWriter()
    {
        _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public void Write(
        AnalyticsResult result,
        TextWriter writer)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(writer);

        var json = JsonSerializer.Serialize(result, _options);

        writer.Write(json);
    }

    public async Task WriteAsync(
        AnalyticsResult result,
        TextWriter writer,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(writer);

        var json = JsonSerializer.Serialize(result, _options);

        await writer.WriteAsync(json.AsMemory(), cancellationToken);
    }
}