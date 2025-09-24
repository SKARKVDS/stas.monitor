namespace Stas.Monitor.Domains;

public record Measure(DateTime? Date, double Observed, double? Expected, string? Type);
