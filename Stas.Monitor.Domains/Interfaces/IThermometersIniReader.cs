namespace Stas.Monitor.Domains.Interfaces;

public interface IThermometersIniReader
{
    IEnumerable<string>? Thermometers { get; }
}
