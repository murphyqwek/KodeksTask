using CLI.ApplicationRunner;

namespace CLI.CommandLine;

public record CommandLineArguments(
    ApplicationMode Mode,
    ApplicationRunOptions Options,
    bool IsHelp);