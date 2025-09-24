using Stas.Monitor.Domains;
using Stas.Monitor.Domains.Interfaces;
using Stas.Monitor.Infrastructures.Except;
using Stas.Monitor.Infrastructures.Interfaces;
using Stas.Monitor.Infrastructures.SqlStorages;

namespace Stas.Monitor.Infrastructures;

public class Repository : IThermometerRepository
{
  // fausse alerte
  private readonly ISqlStorage? _storage;

  public Repository(IConfiguration configuration)
  {
      AllThermometers = configuration.AllThermometers;
      var factory = new SqlStorageFactory("MySql.Data.MySqlClient", configuration.ConnectionString!);
      _storage = factory.NewStorage();
  }

  public string?[] AllThermometers { get; }

  public string[] GetAvailableViews(string name)
  {
    try
    {
      return _storage!.GetTypeOfViews(name);
    }
    catch (Exception e)
    {
      throw new RepositoryException(e);
    }
  }

  public string? GetThermometer(string name) => AllThermometers.FirstOrDefault(t => t == name);

  public IEnumerable<Measure> GetMeasures(string name, DateTime lastMeasureDate, string viewName)
  {
    try
    {
        return _storage!.GetMeasures(name, lastMeasureDate, viewName);
    }
    catch (Exception e)
    {
      throw new RepositoryException(e);
    }
  }

  public DateTime GetLastMeasureDate(string name, string viewName)
  {
    try
    {
      return _storage!.GetLastMeasureDate(name, viewName);
    }
    catch ( Exception e )
    {
      throw new RepositoryException(e);
    }
  }
}
