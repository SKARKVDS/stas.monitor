using System.Data;
using Stas.Monitor.Infrastructures.Dtos;
using Stas.Monitor.Infrastructures.Interfaces;

namespace Stas.Monitor.Infrastructures;

public class CommandExecuter : ICommandExecuter
{
    public string[] ExecuteTypeOfViews(IDbCommand command)
    {
        using var reader = command.ExecuteReader();
        List<string> views = new();
        while (reader.Read())
        {
            views.Add(reader.GetString(0));
        }

        return views.ToArray();
    }

    public IEnumerable<MeasureDto> ExecuteMeasures(IDbCommand command)
    {
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            yield return new MeasureDto(
                reader.GetDateTime(0),
                reader.GetString(1),
                reader.GetDouble(2),
                SafeGetDouble(reader, 3));
        }
    }

    public DateTime ExecuteLastMeasureDate(IDbCommand command)
    {
        using var reader = command.ExecuteReader();
        return reader.Read() ? reader.GetDateTime(0) : DateTime.MinValue;
    }

    private static double? SafeGetDouble(IDataRecord reader, int colIndex) => reader.IsDBNull(colIndex) ? null : reader.GetDouble(colIndex);
}
