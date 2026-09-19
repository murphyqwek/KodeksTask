using CLI.ApplicationRunner.Strategy.Analytics;
using CLI.ApplicationRunner.Strategy.Input;
using CLI.ApplicationRunner.Strategy.Output;

namespace CLI.ApplicationRunner;

public sealed class ApplicationRunnerBuilder
{
    private readonly SyncInputStrategy _syncInputStrategy;
    private readonly AsyncInputStrategy _asyncInputStrategy;

    private readonly SequentialAnalyticsStrategy _sequentialAnalyticsStrategy;
    private readonly ParallelAnalyticsStrategy _parallelAnalyticsStrategy;
    private readonly AsyncAnalyticsStrategy _asyncAnalyticsStrategy;

    private readonly ConsoleOutputStrategy _consoleOutputStrategy;
    private readonly JsonFileOutputStrategy _jsonFileOutputStrategy;
    private readonly JsonFileOutputAsyncStrategy _jsonFileOutputAsyncStrategy;

    private IInputStrategy? _inputStrategy;
    private IAnalyticsStrategy? _analyticsStrategy;
    private IOutputStrategy? _outputStrategy;

    public ApplicationRunnerBuilder(
        SyncInputStrategy syncInputStrategy,
        AsyncInputStrategy asyncInputStrategy,
        SequentialAnalyticsStrategy sequentialAnalyticsStrategy,
        ParallelAnalyticsStrategy parallelAnalyticsStrategy,
        AsyncAnalyticsStrategy asyncAnalyticsStrategy,
        ConsoleOutputStrategy consoleOutputStrategy,
        JsonFileOutputStrategy jsonFileOutputStrategy,
        JsonFileOutputAsyncStrategy jsonFileOutputAsyncStrategy)
    {
        _syncInputStrategy = syncInputStrategy;
        _asyncInputStrategy = asyncInputStrategy;

        _sequentialAnalyticsStrategy = sequentialAnalyticsStrategy;
        _parallelAnalyticsStrategy = parallelAnalyticsStrategy;
        _asyncAnalyticsStrategy = asyncAnalyticsStrategy;

        _consoleOutputStrategy = consoleOutputStrategy;
        _jsonFileOutputStrategy = jsonFileOutputStrategy;
        _jsonFileOutputAsyncStrategy = jsonFileOutputAsyncStrategy;
    }

    public ApplicationRunnerBuilder UseSyncInput()
    {
        _inputStrategy = _syncInputStrategy;
        return this;
    }

    public ApplicationRunnerBuilder UseAsyncInput()
    {
        _inputStrategy = _asyncInputStrategy;
        return this;
    }

    public ApplicationRunnerBuilder UseSequentialAnalytics()
    {
        _analyticsStrategy = _sequentialAnalyticsStrategy;
        return this;
    }

    public ApplicationRunnerBuilder UseParallelAnalytics()
    {
        _analyticsStrategy = _parallelAnalyticsStrategy;
        return this;
    }

    public ApplicationRunnerBuilder UseAsyncAnalytics()
    {
        _analyticsStrategy = _asyncAnalyticsStrategy;
        return this;
    }

    public ApplicationRunnerBuilder WriteToConsole()
    {
        _outputStrategy = _consoleOutputStrategy;
        return this;
    }

    public ApplicationRunnerBuilder WriteJson()
    {
        _outputStrategy = _jsonFileOutputStrategy;
        return this;
    }

    public ApplicationRunnerBuilder WriteJsonAsync()
    {
        _outputStrategy = _jsonFileOutputAsyncStrategy;
        return this;
    }

    public ApplicationRunner Build()
    {
        if (_inputStrategy is null)
        {
            throw new InvalidOperationException("Input strategy is not configured");
        }

        if (_analyticsStrategy is null)
        {
            throw new InvalidOperationException("Analytics strategy is not configured");
        }

        if (_outputStrategy is null)
        {
            throw new InvalidOperationException("Output strategy is not configured");
        }

        return new ApplicationRunner(
            _inputStrategy,
            _analyticsStrategy,
            _outputStrategy);
    }
}