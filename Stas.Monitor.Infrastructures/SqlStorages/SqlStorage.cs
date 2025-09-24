using System.Data;
using System.Data.Common;
using Stas.Monitor.Domains;
using Stas.Monitor.Infrastructures.Dtos;
using Stas.Monitor.Infrastructures.Except;
using Stas.Monitor.Infrastructures.Interfaces;

namespace Stas.Monitor.Infrastructures.SqlStorages;

public sealed class SqlStorage : ISqlStorage
{
  private readonly IDbConnection _connection;

  private readonly ICommandMaker _commandMaker;

  private readonly ICommandExecuter _commandExecuter;

  public SqlStorage(DbProviderFactory factory, string? connectionString)
  {
        _connection = factory.CreateConnection()!;
        _connection.ConnectionString = connectionString;
        _connection.Open();
        _commandMaker = new CommandMaker(_connection);
        _commandExecuter = new CommandExecuter();
  }

  public string[] GetTypeOfViews(string? name) => _commandExecuter.ExecuteTypeOfViews(_commandMaker.GetTypeOfViews(name!)!);

  public IEnumerable<Measure> GetMeasures(string name, DateTime lastMeasureDate, string viewName) =>
      _commandExecuter.ExecuteMeasures(_commandMaker.GetMeasures(name, lastMeasureDate, viewName)!).Select(Mapper.DtoToMeasure);

  public DateTime GetLastMeasureDate(string name, string viewName) => _commandExecuter.ExecuteLastMeasureDate(_commandMaker.GetLastMeasureDate(name)!);

  public void Dispose() => _connection.Dispose();
}
