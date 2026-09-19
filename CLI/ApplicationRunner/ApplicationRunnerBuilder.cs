using CLI.ApplicationRunner.Strategy.Analytics;
using CLI.ApplicationRunner.Strategy.Input;
using CLI.ApplicationRunner.Strategy.Output;
using Core.Service.Analytics;
using Core.Service.Csv;
using Core.Service.Writer;

namespace CLI.ApplicationRunner;

public class ApplicationRunnerBuilder
{
    private readonly ICsvReader _csvReader;
    private readonly IAnalyticsService _analyticsService;
    private readonly IResultWriter _consoleWriter;
    private readonly IResultWriter _jsonWriter;

    private IInputStrategy? _inputStrategy;
    private IAnalyticsStrategy? _analyticsStrategy;
    private IOutputStrategy? _outputStrategy;

    public ApplicationRunnerBuilder(
        ICsvReader csvReader,
        IAnalyticsService analyticsService,
        TextResultWriter consoleWriter,
        JsonResultWriter jsonWriter)
    {
        _csvReader = csvReader;
        _analyticsService = analyticsService;
        _consoleWriter = consoleWriter;
        _jsonWriter = jsonWriter;
    }

    public ApplicationRunnerBuilder UseSyncInput()
    {
        _inputStrategy = new SyncInputStrategy(_csvReader);
        return this;
    }

    public ApplicationRunnerBuilder UseAsyncInput()
    {
        _inputStrategy = new AsyncInputStrategy(_csvReader);
        return this;
    }

    public ApplicationRunnerBuilder UseSequentialAnalytics()
    {
        _analyticsStrategy = new SequentialAnalyticsStrategy(_analyticsService);

        return this;
    }

    public ApplicationRunnerBuilder UseAsyncAnalytics()
    {
        _analyticsStrategy = new AsyncAnalyticsStrategy(_analyticsService);

        return this;
    }

    public ApplicationRunnerBuilder UseParallelAnalytics()
    {
        _analyticsStrategy = new ParallelAnalyticsStrategy(_analyticsService);

        return this;
    }

    public ApplicationRunnerBuilder WriteToConsole()
    {
        _outputStrategy = new ConsoleOutputStrategy(_consoleWriter);

        return this;
    }

    public ApplicationRunnerBuilder WriteJson()
    {
        _outputStrategy = new JsonFileOutputStrategy(_jsonWriter);

        return this;
    }

    public ApplicationRunnerBuilder WriteJsonAsync()
    {
        _outputStrategy = new JsonFileOutputAsyncStrategy(_jsonWriter);

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