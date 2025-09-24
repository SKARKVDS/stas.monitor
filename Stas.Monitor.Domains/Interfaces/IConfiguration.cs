namespace Stas.Monitor.Domains.Interfaces;

public interface IConfiguration
{
    public string?[] AllThermometers { get; }

    public string? ConnectionString { get; }
}
