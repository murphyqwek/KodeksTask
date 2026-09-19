using CLI.CommandLine;

namespace CLI;

public class Program
{
    static void Main(string[] args)
    {
        var parser = new CommandLineParser();

        var commandArgs = parser.Parse(args);


    }
}
