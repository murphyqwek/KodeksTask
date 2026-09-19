namespace CLI.ApplicationRunner.Strategy.Analytics;

using Core.Model;
using Core.Model.Analytics;

public interface IAnalyticsStrategy
{
    Task<AnalyticsResult> AnalyzeAsync(
        IReadOnlyCollection<Sale> sales,
        DateOnly? startDate,
        DateOnly? endDate,
        CancellationToken cancellationToken = default);
}