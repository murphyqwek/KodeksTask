using Core.Model.Analytics;

namespace Core.Service.Writer;

public interface IResultWriter
{
    void Write(
        AnalyticsResult result,
        TextWriter writer);

    Task WriteAsync(
        AnalyticsResult result,
        TextWriter writer,
        CancellationToken cancellationToken = default);
}