using Stas.Monitor.Domains;

namespace Stas.Monitor.Presentations;

public class MeasureViewModel
{
  private readonly Measure? _measure;

  public MeasureViewModel(Measure? measure)
  {
    _measure = measure;
  }

  public DateTime? Date => _measure?.Date;

  public string? MeasureValue =>
      Type switch
      {
          "temperature" => _measure?.Observed+Unit,
          "humidity" => Math.Round((double)(_measure?.Observed * 100)!,0)+Unit,
          _ => null,
      };

  public string? Type => _measure?.Type;

  public string Unit =>
      Type switch
      {
          "temperature" => UnitSymbol.Temperature,
          "humidity" => UnitSymbol.Humidity,
          _ => string.Empty,
      };

  public double? Difference
  {
      get
      {
          if (_measure?.Expected != null)
          {
            return Type switch
              {
                  "temperature" => Math.Round((double)(_measure?.Observed - _measure?.Expected)!,2),
                  "humidity" => Math.Round((double)(_measure?.Observed - _measure?.Expected)! * 100),
                  _ => null,
              };
          }

          return null;
      }
  }
}
