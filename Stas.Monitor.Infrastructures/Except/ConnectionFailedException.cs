namespace Stas.Monitor.Infrastructures.Except;

public class ConnectionFailedException : Exception
{
  public ConnectionFailedException(string message, Exception innerException) : base(message, innerException)
  {
  }
}
