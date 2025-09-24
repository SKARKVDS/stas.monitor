using System.Data.Common;
using MySql.Data.MySqlClient;
using Stas.Monitor.Infrastructures.Except;
using Stas.Monitor.Infrastructures.Interfaces;

namespace Stas.Monitor.Infrastructures.SqlStorages;

public class SqlStorageFactory
{
  private readonly DbProviderFactory _factory;
  private readonly string? _connectionString;

  public SqlStorageFactory(string providerName, string? connectionString, DbProviderFactory? factory = null)
  {
    try
    {
        _connectionString = connectionString;
        if (factory != null)
        {
            DbProviderFactories.RegisterFactory(providerName, factory);
        }
        else
        {
            DbProviderFactories.RegisterFactory("MySql.Data.MySqlClient", MySqlClientFactory.Instance);
        }

        _factory = DbProviderFactories.GetFactory(providerName);
    }
    catch (ArgumentException e)
    {
      throw new UnableToConnectException(e);
    }
  }

  public ISqlStorage NewStorage()
  {
    try
    {
        return new SqlStorage(_factory,_connectionString);
    }
    catch (Exception e)
    {
        throw new UnableToConnectException(e);
    }
  }
}
