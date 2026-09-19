using Core.Model;

namespace CLI.ApplicationRunner.Strategy.Input;

public interface IInputStrategy
{
    Task<IReadOnlyList<Sale>> ReadAsync(string inputPath, CancellationToken cancellationToken = default);
}