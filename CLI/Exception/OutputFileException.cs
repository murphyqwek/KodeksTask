namespace CLI.Exceptions;

public sealed class OutputFileException : Exception
{
    public string Path { get; }

    public OutputFileException(string path, Exception innerException)
        : base($"Failed to write output file '{path}'", innerException)
    {
        Path = path;
    }
}