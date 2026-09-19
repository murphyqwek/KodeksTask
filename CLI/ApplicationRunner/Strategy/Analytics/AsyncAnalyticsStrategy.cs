using Core.Model;
using Core.Model.Analytics;
using Core.Service.Analytics;

namespace CLI.ApplicationRunner.Strategy.Analytics;

public class AsyncAnalyticsStrategy : IAnalyticsStrategy
{
    private readonly IAnalyticsService _analyticsService;

    public AsyncAnalyticsStrategy(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    public async Task<AnalyticsResult> AnalyzeAsync(
        IReadOnlyCollection<Sale> sales,
        DateOnly? startDate,
        DateOnly? endDate,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await _analyticsService.AnalyzeAsync(sales, startDate, endDate, cancellationToken);
    }
}