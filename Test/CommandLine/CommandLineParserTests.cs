using CLI.CommandLine;

namespace Tests.CommandLine;

public class CommandLineParserTests
{
    private readonly CommandLineParser _parser = new();

    [Fact]
    public void Parse_WhenArgumentsAreValid_ShouldReturnCommandLineArguments()
    {
        string[] args =
        [
            "--mode", "parallel",
        "--input", "sales.csv",
        "--output", "result.json",
        "--start-date", "1/1/2026",
        "--end-date", "1/2/2026"
        ];

        var result = _parser.Parse(args);

        Assert.False(result.IsHelp);

        Assert.Equal(ApplicationMode.Parallel, result.Mode);
        Assert.Equal("sales.csv", result.Options.InputPath);
        Assert.Equal("result.json", result.Options.OutputPath);

        Assert.Equal(
            new DateOnly(2026, 1, 1),
            result.Options.StartDate);

        Assert.Equal(
            new DateOnly(2026, 1, 2),
            result.Options.EndDate);
    }

    [Theory]
    [InlineData("--help")]
    [InlineData("-h")]
    public void Parse_WhenHelpArgumentProvided_ShouldReturnHelp(string helpArgument)
    {
        string[] args = [helpArgument];

        var result = _parser.Parse(args);

        Assert.True(result.IsHelp);
    }

    [Fact]
    public void Parse_WhenOptionalArgumentsAreMissing_ShouldSetThemToNull()
    {
        string[] args =
        [
            "--mode", "console",
            "--input", "sales.csv"
        ];

        var result = _parser.Parse(args);

        Assert.Equal(ApplicationMode.Console, result.Mode);
        Assert.Equal("sales.csv", result.Options.InputPath);

        Assert.Null(result.Options.OutputPath);
        Assert.Null(result.Options.StartDate);
        Assert.Null(result.Options.EndDate);

        Assert.False(result.IsHelp);
    }

    [Fact]
    public void Parse_WhenModeIsMissing_ShouldThrowArgumentException()
    {
        string[] args =
        [
            "--input", "sales.csv"
        ];

        var exception = Assert.Throws<ArgumentException>(() => _parser.Parse(args));

        Assert.Contains("--mode", exception.Message);
    }

    [Fact]
    public void Parse_WhenInputIsMissing_ShouldThrowArgumentException()
    {
        string[] args =
        [
            "--mode", "console"
        ];

        var exception = Assert.Throws<ArgumentException>(() => _parser.Parse(args));

        Assert.Contains("--input", exception.Message);
    }

    [Fact]
    public void Parse_WhenModeIsUnknown_ShouldThrowArgumentException()
    {
        string[] args =
        [
            "--mode", "super-fast",
            "--input", "sales.csv"
        ];

        var exception = Assert.Throws<ArgumentException>(() => _parser.Parse(args));

        Assert.Contains("Unknown mode", exception.Message);
    }

    [Theory]
    [InlineData("01-01-2026")]
    [InlineData("2026/01/01")]
    [InlineData("hello")]
    public void Parse_WhenStartDateHasInvalidFormat_ShouldThrowArgumentException(string date)
    {
        string[] args =
        [
            "--mode", "console",
            "--input", "sales.csv",
            "--start-date", date
        ];

        var exception = Assert.Throws<ArgumentException>(() => _parser.Parse(args));

        Assert.Contains("--start-date", exception.Message);
    }

    [Fact]
    public void Parse_WhenStartDateIsAfterEndDate_ShouldThrowArgumentException()
    {
        string[] args =
        [
            "--mode", "console",
            "--input", "sales.csv",
            "--start-date", "9/10/20026",
            "--end-date", "9/1/20026"
        ];

        var exception = Assert.Throws<ArgumentException>(() => _parser.Parse(args));

        Assert.Contains("--start-date", exception.Message);
    }

    [Fact]
    public void Parse_WhenStartDateEqualsEndDate_ShouldThrowArgumentException()
    {
        string[] args =
        [
            "--mode", "console",
            "--input", "sales.csv",
            "--start-date", "9/10/20026",
            "--end-date", "9/10/20026"
        ];

        Assert.Throws<ArgumentException>(() => _parser.Parse(args));
    }

    [Fact]
    public void Parse_WhenArgumentIsUnknown_ShouldThrowArgumentException()
    {
        string[] args =
        [
            "--mode", "console",
            "--input", "sales.csv",
            "--unknown", "value"
        ];

        var exception = Assert.Throws<ArgumentException>(() => _parser.Parse(args));

        Assert.Contains("Unknown argument", exception.Message);
    }

    [Fact]
    public void Parse_WhenArgumentDoesNotHaveValue_ShouldThrowArgumentException()
    {
        string[] args =
        [
            "--mode", "console",
            "--input"
        ];

        var exception = Assert.Throws<ArgumentException>(() => _parser.Parse(args));

        Assert.Contains("requires a value", exception.Message);
    }

    [Fact]
    public void Parse_WhenArgumentSpecifiedTwice_ShouldThrowArgumentException()
    {
        string[] args =
        [
            "--mode", "console",
            "--mode", "parallel",
            "--input", "sales.csv"
        ];

        var exception = Assert.Throws<ArgumentException>(() => _parser.Parse(args));

        Assert.Contains("specified more than once", exception.Message);
    }

    [Fact]
    public void Parse_WhenModeHasDifferentCase_ShouldParseIgnoringCase()
    {
        string[] args =
        [
            "--mode", "PaRaLlEl",
            "--input", "sales.csv"
        ];

        var result = _parser.Parse(args);

        Assert.Equal(ApplicationMode.Parallel, result.Mode);
    }
}