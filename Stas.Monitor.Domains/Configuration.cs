using Stas.Monitor.Domains.Interfaces;

namespace Stas.Monitor.Domains;

public class Configuration : IConfiguration
{
  private readonly IEnumerable<string?> _thermometers;

  private readonly IDbConfig _dbConfig;

  public Configuration(IEnumerable<string?> thermometers, IDbConfig dbConfig)
  {
    _thermometers = thermometers;
    _dbConfig = dbConfig;
  }

  public string?[] AllThermometers => _thermometers.ToArray();

  public string ConnectionString => _dbConfig.ConnectionString;
}
