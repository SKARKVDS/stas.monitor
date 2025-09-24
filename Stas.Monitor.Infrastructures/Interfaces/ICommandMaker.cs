using System.Data;

namespace Stas.Monitor.Infrastructures.Interfaces;

public interface ICommandMaker
{
    IDbCommand GetTypeOfViews(string name);

    IDbCommand GetMeasures(string name, DateTime lastMeasureDate, string viewName);

    IDbCommand GetLastMeasureDate(string name);
}
