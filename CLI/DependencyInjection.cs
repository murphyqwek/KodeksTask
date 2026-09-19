using CLI.ApplicationRunner;
using CLI.ApplicationRunner.Strategy.Analytics;
using CLI.ApplicationRunner.Strategy.Input;
using CLI.ApplicationRunner.Strategy.Output;
using Core.Service.Analytics;
using Core.Service.Csv;
using Core.Service.Writer;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IAnalyticsService, AnalyticsService>();

        services.AddSingleton<ICsvReader, CsvReader>();

        //Стратегии input'а
        services.AddSingleton<AsyncInputStrategy>();
        services.AddSingleton<SyncInputStrategy>();

        //Стратегии аналитики
        services.AddSingleton<SequentialAnalyticsStrategy>();
        services.AddSingleton<ParallelAnalyticsStrategy>();
        services.AddSingleton<AsyncAnalyticsStrategy>();

        //Стратегии output'а
        services.AddSingleton<ConsoleOutputStrategy>();
        services.AddSingleton<JsonFileOutputStrategy>();
        services.AddSingleton<JsonFileOutputAsyncStrategy>();

        services.AddSingleton<JsonResultWriter>();
        services.AddSingleton<TextResultWriter>();

        services.AddTransient<ApplicationRunnerBuilder>();

        return services;
    }
}