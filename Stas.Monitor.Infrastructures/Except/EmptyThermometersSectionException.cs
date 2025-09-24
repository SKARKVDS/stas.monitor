namespace Stas.Monitor.Infrastructures.Except;

public class EmptyThermometersSectionException : Exception
{
    public EmptyThermometersSectionException(string message) : base(message)
    {
    }
}
