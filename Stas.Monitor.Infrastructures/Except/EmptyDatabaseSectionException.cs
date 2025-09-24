namespace Stas.Monitor.Infrastructures.Except;

public class EmptyDatabaseSectionException : Exception
{
    public EmptyDatabaseSectionException(string message) : base(message)
    {
    }
}
