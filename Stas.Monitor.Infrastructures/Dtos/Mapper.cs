using Stas.Monitor.Domains;

namespace Stas.Monitor.Infrastructures.Dtos;

public static class Mapper
{
    public static Measure DtoToMeasure(MeasureDto dto) => new(dto.Date, dto.Observed, dto.Expected, dto.Type);
}
