using Stas.Monitor.Domains.Interfaces;

namespace Stas.Monitor.Domains.Factory;

public class ConfigurationFactory
{
    private readonly IConfigurationReader _configurationReader;

    public ConfigurationFactory(IConfigurationReader configurationReader)
    {
        _configurationReader = configurationReader;
    }

    public IConfiguration Create()
    {
        var thermometers = _configurationReader.Thermometers;
        var dbConfig = _configurationReader.DbConfig;
        return new Configuration(thermometers!, dbConfig!);
    }
}
