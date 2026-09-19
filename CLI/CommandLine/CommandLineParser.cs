using System.Globalization;
using CLI.ApplicationRunner;

namespace CLI.CommandLine;

public sealed class CommandLineParser
{
    private const string DateFormat = "M/d/yyyy";

    public CommandLineArguments Parse(string[] args)
    {
        ArgumentNullException.ThrowIfNull(args);

        if (args.Contains("--help") || args.Contains("-h"))
        {
            return new CommandLineArguments(
                Mode: default,
                Options: new ApplicationRunOptions(
                    InputPath: string.Empty,
                    OutputPath: null,
                    StartDate: null,
                    EndDate: null),
                IsHelp: true);
        }

        var arguments = ParseArguments(args);

        var mode = ParseMode(GetRequired(arguments, "mode"));

        var inputPath = GetRequired(arguments, "input");

        var outputPath = GetOptional(arguments, "output");

        var startDate = ParseDate(GetOptional(arguments, "start-date"), "start-date");

        var endDate = ParseDate(GetOptional(arguments, "end-date"), "end-date");

        ValidateOutputPath(mode, outputPath);

        ValidateDates(startDate, endDate);

        return new CommandLineArguments(
            mode,
            new ApplicationRunOptions(
                inputPath,
                outputPath,
                startDate,
                endDate),
            IsHelp: false);
    }

    private static Dictionary<string, string> ParseArguments(string[] args)
    {
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        for (var i = 0; i < args.Length; i++)
        {
            var current = args[i];

            if (!current.StartsWith("--"))
            {
                throw new ArgumentException($"Unexpected argument '{current}'");
            }

            var name = current[2..];

            if (!IsKnownArgument(name))
            {
                throw new ArgumentException($"Unknown argument '--{name}'");
            }

            if (i + 1 >= args.Length || args[i + 1].StartsWith("--"))
            {
                throw new ArgumentException($"Argument '--{name}' requires a value");
            }

            var value = args[++i];

            if (!result.TryAdd(name, value))
            {
                throw new ArgumentException($"Argument '--{name}' was specified more than once");
            }
        }

        return result;
    }

    private static bool IsKnownArgument(string name)
    {
        return name is
            "mode" or
            "input" or
            "output" or
            "start-date" or
            "end-date";
    }

    private static string GetRequired(IReadOnlyDictionary<string, string> arguments, string name)
    {
        if (!arguments.TryGetValue(name, out var value) || string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"Required argument '--{name}' was not specified.");
        }

        return value;
    }

    private static string? GetOptional(IReadOnlyDictionary<string, string> arguments, string name)
    {
        return arguments.TryGetValue(name, out var value) ? value : null;
    }

    private static ApplicationMode ParseMode(string value)
    {
        if (!Enum.TryParse<ApplicationMode>(value, ignoreCase: true, out var mode))
        {
            throw new ArgumentException(
                $"Unknown mode '{value}'. Available modes: {string.Join(", ", Enum.GetNames<ApplicationMode>())}.");
        }

        return mode;
    }

    private static DateOnly? ParseDate(string? value, string argumentName)
    {
        if (value is null)
        {
            return null;
        }

        if (!DateOnly.TryParseExact(
                value,
                DateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var date))
        {
            throw new ArgumentException(
                $"Argument '--{argumentName}' must have format {DateFormat}.");
        }

        return date;
    }

    private static void ValidateDates(DateOnly? startDate, DateOnly? endDate)
    {
        if (startDate.HasValue &&
            endDate.HasValue &&
            startDate >= endDate)
        {
            throw new ArgumentException(
                "'--start-date' must be earlier than '--end-date'.");
        }
    }

    private static void ValidateOutputPath(ApplicationMode mode, string? outputPath)
    {
        var requiresOutput = mode is
            ApplicationMode.File or
            ApplicationMode.Async or
            ApplicationMode.Full;

        if (requiresOutput && string.IsNullOrWhiteSpace(outputPath))
        {
            throw new ArgumentException(
                $"Argument '--output' is required for '{mode.ToString().ToLowerInvariant()}' mode.");
        }
    }
}