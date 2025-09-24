namespace Stas.Monitor.Infrastructures.Except;

public class NoArgumentPassedException : Exception
{
    public NoArgumentPassedException(string message) : base(message)
    {
    }
}
