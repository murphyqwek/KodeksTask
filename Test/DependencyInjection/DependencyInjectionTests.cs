using CLI.ApplicationRunner;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.DependencyInjection;

public class DependencyInjectionTests
{
    [Fact]
    public void AddApplicationServices_ShouldBuildServiceProvider()
    {
        var services = new ServiceCollection();

        services.AddApplicationServices();

        using var provider = services.BuildServiceProvider(
            new ServiceProviderOptions
            {
                ValidateOnBuild = true,
                ValidateScopes = true
            });

        Assert.NotNull(provider);
    }

    [Fact]
    public void AddApplicationServices_ShouldResolveApplicationRunnerBuilder()
    {
        var services = new ServiceCollection();

        services.AddApplicationServices();

        using var provider = services.BuildServiceProvider(
            new ServiceProviderOptions
            {
                ValidateOnBuild = true,
                ValidateScopes = true
            });

        var builder = provider.GetRequiredService<ApplicationRunnerBuilder>();

        Assert.NotNull(builder);
    }
}