using Core.Model;
using Core.Model.Analytics;

namespace Core.Service.Analytics;

public interface IAnalyticsService
{
    AnalyticsResult Analyze(
        IReadOnlyCollection<Sale> sales,
        DateOnly? startDate,
        DateOnly? endDate);

    Task<AnalyticsResult> AnalyzeAsync(
        IReadOnlyCollection<Sale> sales,
        DateOnly? startDate,
        DateOnly? endDate,
        CancellationToken cancellationToken = default);

    AnalyticsResult AnalyzeParallel(
        IReadOnlyCollection<Sale> sales,
        DateOnly? startDate,
        DateOnly? endDate);
}