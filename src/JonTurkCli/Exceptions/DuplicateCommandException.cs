namespace JonTurkCli.Exceptions;

public class DuplicateCommandException : Exception
{
    public DuplicateCommandException(string message) : base(message)
    {
    }
}