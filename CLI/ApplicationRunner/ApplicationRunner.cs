using CLI.ApplicationRunner.Strategy.Analytics;
using CLI.ApplicationRunner.Strategy.Input;
using CLI.ApplicationRunner.Strategy.Output;
using System.Diagnostics;

namespace CLI.ApplicationRunner;

public class ApplicationRunner
{
    private readonly IInputStrategy _inputStrategy;
    private readonly IAnalyticsStrategy _analyticsStrategy;
    private readonly IOutputStrategy _outputStrategy;

    public ApplicationRunner(
        IInputStrategy inputStrategy,
        IAnalyticsStrategy analyticsStrategy,
        IOutputStrategy outputStrategy)
    {
        _inputStrategy = inputStrategy;
        _analyticsStrategy = analyticsStrategy;
        _outputStrategy = outputStrategy;
    }

    public async Task RunAsync(ApplicationRunOptions options, CancellationToken cancellationToken = default)
    {
        ValidateOptions(options);

        var sales = await _inputStrategy.ReadAsync(
            options.InputPath,
            cancellationToken);

        var result = await _analyticsStrategy.AnalyzeAsync(
            sales,
            options.StartDate,
            options.EndDate,
            cancellationToken);

        await _outputStrategy.WriteAsync(
            result,
            options.OutputPath,
            cancellationToken);
    }

    private static void ValidateOptions(ApplicationRunOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (string.IsNullOrWhiteSpace(options.InputPath))
        {
            throw new ArgumentException("Input path cannot be empty", nameof(options.InputPath));
        }

        if (options.StartDate is not null &&
            options.EndDate is not null &&
            options.StartDate >= options.EndDate)
        {
            throw new ArgumentException("Start date must be earlier than end date");
        }
    }
}