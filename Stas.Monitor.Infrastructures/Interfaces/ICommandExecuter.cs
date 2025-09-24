using System.Data;
using Stas.Monitor.Infrastructures.Dtos;

namespace Stas.Monitor.Infrastructures.Interfaces;

public interface ICommandExecuter
{
    string[] ExecuteTypeOfViews(IDbCommand reader);

    IEnumerable<MeasureDto> ExecuteMeasures(IDbCommand reader);

    DateTime ExecuteLastMeasureDate(IDbCommand reader);
}
