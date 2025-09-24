namespace Stas.Monitor.Infrastructures.Except;

public class NoConfigurationFileFoundException : Exception
{
    public NoConfigurationFileFoundException(string message) : base(message)
    {
    }
}
