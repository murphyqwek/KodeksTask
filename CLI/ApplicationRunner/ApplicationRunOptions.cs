namespace CLI.ApplicationRunner;

public record ApplicationRunOptions(
    string InputPath,
    string? OutputPath,
    DateOnly? StartDate,
    DateOnly? EndDate);