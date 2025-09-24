namespace Stas.Monitor.Domains.Interfaces;

public interface IThermometerRepository
{
    string?[] AllThermometers { get; }

    string[] GetAvailableViews(string name);

    string? GetThermometer(string name);

    IEnumerable<Measure> GetMeasures(string name, DateTime lastMeasureDate, string viewName);

    DateTime GetLastMeasureDate(string currentThermometerName, string viewName);
}
