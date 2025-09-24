using Stas.Monitor.Domains.Interfaces;
using Stas.Monitor.Infrastructures.Except;

namespace Stas.Monitor.Infrastructures.Reader;

public class ThermometersIniReader : IThermometersIniReader
{
    private readonly string? _iniPath;

    private readonly IList<string>? _thermometers;

    public ThermometersIniReader(string? iniPath)
    {
        _iniPath = iniPath;
        _thermometers = new List<string>();
        LoadThermometers();

        if (_thermometers?.Count == 0)
        {
            throw new EmptyThermometersSectionException("no thermometer found in configuration file");
        }
    }

    private void LoadThermometers()
    {
        try
        {
            using var reader = new StreamReader(_iniPath!);
            var line = reader.ReadLine();
            while (line != null)
            {
                if (line.Contains("[Database]"))
                {
                    break;
                }

                if (line.Contains(';') || line.Contains("[Thermometers]") || string.IsNullOrWhiteSpace(line))
                {
                    line = reader.ReadLine();
                    continue;
                }

                _thermometers?.Add(line);
                line = reader.ReadLine();
            }
        }
        catch (Exception)
        {
            throw new EmptyThermometersSectionException("missing required section thermometers");
        }
    }

    public IEnumerable<string>? Thermometers => _thermometers;
}
