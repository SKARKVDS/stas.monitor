namespace Stas.Monitor.Infrastructures.Except;

public class RepositoryException : Exception
{
  public RepositoryException(Exception exception) : base("unable to read data", exception)
  {
  }
}
