namespace Stas.Monitor.Domains.Interfaces;

public interface IConfigurationReader
{
    IEnumerable<string>? Thermometers { get; }

    IDbConfig? DbConfig { get; }
}
