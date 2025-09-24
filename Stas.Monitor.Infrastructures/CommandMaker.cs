using System.Data;
using Stas.Monitor.Infrastructures.Except;
using Stas.Monitor.Infrastructures.Interfaces;

namespace Stas.Monitor.Infrastructures;

public class CommandMaker : ICommandMaker
{
    private readonly IDbConnection _connection;

    public CommandMaker(IDbConnection connection)
    {
        _connection = connection;
    }

    public IDbCommand GetTypeOfViews(string name)
    {
        try
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT DISTINCT type " +
                                  "FROM Measures " +
                                  "WHERE thermometerName = @name";
            command.Parameters.Add(CreateParameter(command.CreateParameter(), "@name", name, DbType.String));
            return command;
        }
        catch (Exception e)
        {
            throw new UnableToReadDataException(e);
        }
    }

    public IDbCommand GetMeasures(string name, DateTime lastMeasureDate, string viewName)
    {
        try
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT m.date, m.type, ROUND(m.observed,2), a.expected " +
                                  "FROM Measures m " +
                                  "LEFT JOIN Alerts a ON m.id=a.measureId " +
                                  "WHERE m.thermometerName = @name AND m.date >= @lastMeasureDate AND m.type = @type ";
            command.Parameters.Add(CreateParameter(command.CreateParameter(),"@name", name, DbType.String));
            command.Parameters.Add(CreateParameter(command.CreateParameter(),"@lastMeasureDate", lastMeasureDate, DbType.DateTime));
            command.Parameters.Add(CreateParameter(command.CreateParameter(),"@type", viewName, DbType.String));
            return command;
        }
        catch (Exception e)
        {
            throw new UnableToReadDataException(e);
        }
    }

    public IDbCommand GetLastMeasureDate(string name)
    {
        try
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT date " +
                                  "FROM Measures " +
                                  "WHERE thermometerName = @name " +
                                  "ORDER BY date DESC " +
                                  "LIMIT 1";
            command.Parameters.Add(CreateParameter(command.CreateParameter(),"@name", name, DbType.String));
            return command;
        }
        catch (Exception e)
        {
            throw new UnableToReadDataException(e);
        }
    }

    private static IDbDataParameter CreateParameter(IDbDataParameter parameter, string parameterName, object value, DbType type)
    {
        parameter.ParameterName = parameterName;
        parameter.Value = value;
        parameter.DbType = type;

        return parameter;
    }
}
