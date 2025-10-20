using CarManagement.Domain.Enums;

namespace CarManagement.Application.Lines.Dtos;

public sealed class LineDto
{
    public Guid Id { get; init; }
    public Guid BrandId { get; init; }
    public string Name { get; init; } = string.Empty;
    public CarCategory Category { get; init; }
    public bool IsActive { get; init; }
    public IReadOnlyList<ModelYearDto> ModelYears { get; init; } = Array.Empty<ModelYearDto>();
}

public sealed class ModelYearDto
{
    public Guid Id { get; init; }
    public Guid LineId { get; init; }
    public int Year { get; init; }
    public decimal BasePrice { get; init; }
    public string Features { get; init; } = "{}";
    public bool IsActive { get; init; }
}
