using Core.Model;
using Core.Model.Analytics;
using Core.Service.Analytics;

namespace CLI.ApplicationRunner.Strategy.Analytics;

public class ParallelAnalyticsStrategy : IAnalyticsStrategy
{
    private readonly IAnalyticsService _analyticsService;

    public ParallelAnalyticsStrategy(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    public Task<AnalyticsResult> AnalyzeAsync(
        IReadOnlyCollection<Sale> sales,
        DateOnly? startDate,
        DateOnly? endDate,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var result = _analyticsService.AnalyzeParallel(sales, startDate, endDate);

        return Task.FromResult(result);
    }
}
