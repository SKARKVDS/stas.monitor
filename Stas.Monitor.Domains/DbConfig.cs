using Stas.Monitor.Domains.Interfaces;

namespace Stas.Monitor.Domains;

public class DbConfig : IDbConfig
{
    private readonly string? _user;
    private readonly string? _password;
    private readonly string? _baseConnectionString;

    public DbConfig(string? user, string? password, string? baseConnectionString)
    {
        _user = user;
        _password = password;
        _baseConnectionString = baseConnectionString;
    }

    public string ConnectionString => $"{_baseConnectionString}User id={_user};Password={_password};";
}
