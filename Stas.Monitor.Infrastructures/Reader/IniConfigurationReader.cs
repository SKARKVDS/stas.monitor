using Stas.Monitor.Domains.Interfaces;

namespace Stas.Monitor.Infrastructures.Reader;

public class IniConfigurationReader : IConfigurationReader
{
  private readonly IThermometersIniReader? _thermometersIniReader;
  private readonly IDbConfigIniReader? _dbConfigIniReader;

  public IniConfigurationReader(IReadOnlyDictionary<string, string> args)
  {
        var pathMaker = new PathMaker(args);
        var path = pathMaker.GetThermometersPath();
        _thermometersIniReader = new ThermometersIniReader(path);
        _dbConfigIniReader = new DbConfigIniReader(path);
  }

  public IEnumerable<string>? Thermometers => _thermometersIniReader?.Thermometers;

  public IDbConfig? DbConfig => _dbConfigIniReader?.DbConfig;
}
