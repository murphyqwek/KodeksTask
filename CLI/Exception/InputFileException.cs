namespace CLI.Exceptions;

public sealed class InputFileException : Exception
{
    public string Path { get; }

    public InputFileException(string path, Exception innerException)
        : base($"Failed to read input file '{path}'", innerException)
    {
        Path = path;
    }
}