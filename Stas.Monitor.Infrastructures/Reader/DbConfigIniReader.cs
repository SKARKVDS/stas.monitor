using Stas.Monitor.Domains;
using Stas.Monitor.Domains.Interfaces;
using Stas.Monitor.Infrastructures.Except;

namespace Stas.Monitor.Infrastructures.Reader;

public class DbConfigIniReader : IDbConfigIniReader
{
    private const string ConnectionPattern = "ConnectionString=";
    private const string UserPattern = "User=";
    private const string PasswordPattern = "Password=";
    private readonly string? _iniPath;

    private string? _connectionString;

    private string? _user;

    private string? _password;

    public DbConfigIniReader(string iniFilePath)
    {
        _iniPath = iniFilePath;
        LoadDbConfig();

        if (_connectionString == null || _user == null || _password == null)
        {
            throw new EmptyDatabaseSectionException("missing required section database");
        }
    }

    private void LoadDbConfig()
    {
        try
        {
            using var reader = new StreamReader(_iniPath!);
            var line = reader.ReadLine();
            while (line != null)
            {
                if (line.Contains(ConnectionPattern))
                {
                    var connectionString = line[ConnectionPattern.Length..];
                    _connectionString = connectionString;
                }

                if (line.Contains(UserPattern))
                {
                    var user = line[UserPattern.Length..];
                    _user = user;
                }

                if (line.Contains(PasswordPattern))
                {
                    var password = line[PasswordPattern.Length..];
                    _password = password;
                }

                line = reader.ReadLine();
            }
        }
        catch (Exception)
        {
            throw new EmptyDatabaseSectionException("missing required section database");
        }
    }

    public IDbConfig DbConfig => new DbConfig(_user, _password, _connectionString);
}
