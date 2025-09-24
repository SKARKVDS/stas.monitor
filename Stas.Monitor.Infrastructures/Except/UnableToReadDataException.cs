namespace Stas.Monitor.Infrastructures.Except;

public class UnableToReadDataException : Exception
{
    public UnableToReadDataException(Exception exception) : base("unable to read data",exception)
    {
    }
}
