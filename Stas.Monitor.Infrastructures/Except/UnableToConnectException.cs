namespace Stas.Monitor.Infrastructures.Except;

public class UnableToConnectException : Exception
{
  public UnableToConnectException(Exception exception) : base("unable to connect to the database", exception)
  {
  }
}
