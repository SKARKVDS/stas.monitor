namespace Stas.Monitor.Infrastructures.Dtos;

public record MeasureDto(DateTime Date, string Type, double Observed, double? Expected);
