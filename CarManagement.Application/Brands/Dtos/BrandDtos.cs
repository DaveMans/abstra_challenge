namespace CarManagement.Application.Brands.Dtos;

public sealed class BrandDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public int FoundedYear { get; init; }
    public bool IsActive { get; init; }
}

public sealed class CreateBrandRequest
{
    public string Name { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public int FoundedYear { get; init; }
}

public sealed class UpdateBrandRequest
{
    public string Name { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public int FoundedYear { get; init; }
    public bool IsActive { get; init; } = true;
}
