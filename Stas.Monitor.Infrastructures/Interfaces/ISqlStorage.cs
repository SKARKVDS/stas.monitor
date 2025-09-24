using Stas.Monitor.Domains;

namespace Stas.Monitor.Infrastructures.Interfaces;

public interface ISqlStorage : IDisposable
{
    string[] GetTypeOfViews(string? name);

    IEnumerable<Measure> GetMeasures(string name, DateTime lastMeasureDate, string viewName);

    DateTime GetLastMeasureDate(string name, string viewName);
}
