using Stas.Monitor.Infrastructures.Except;

namespace Stas.Monitor.Infrastructures;

public class PathMaker
{
    private readonly string _path;

    public PathMaker(IReadOnlyDictionary<string, string> args)
    {
        _path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\ressources\"+GetPath(args));
    }

    private string GetPath(IReadOnlyDictionary<string, string> args) => args.Count > 0 ? args["config-file"] : throw new NoArgumentPassedException("missing configuration file argument");

    public string GetThermometersPath()
    {
        if (!File.Exists(_path))
        {
            throw new NoConfigurationFileFoundException("configuration file not found");
        }

        return _path;
    }
}
