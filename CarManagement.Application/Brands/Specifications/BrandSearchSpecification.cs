using CarManagement.Domain.Entities;
using CarManagement.Domain.Specifications;

namespace CarManagement.Application.Brands.Specifications;

public sealed class BrandSearchSpecification : BaseSpecification<Brand>
{
    public BrandSearchSpecification(string? search, string? country, string? sortBy, bool desc, int? page, int? pageSize)
    {
        Criteria = x => !x.IsDeleted && (string.IsNullOrEmpty(search) || x.Name.Contains(search)) && (string.IsNullOrEmpty(country) || x.Country == country);
        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            if (string.Equals(sortBy, nameof(Brand.Name), StringComparison.OrdinalIgnoreCase))
            {
                if (desc) ApplyOrderByDescending(x => x.Name); else ApplyOrderBy(x => x.Name);
            }
            else if (string.Equals(sortBy, nameof(Brand.FoundedYear), StringComparison.OrdinalIgnoreCase))
            {
                if (desc) ApplyOrderByDescending(x => x.FoundedYear); else ApplyOrderBy(x => x.FoundedYear);
            }
            else
            {
                if (desc) ApplyOrderByDescending(x => x.CreatedAt); else ApplyOrderBy(x => x.CreatedAt);
            }
        }
        else
        {
            if (desc) ApplyOrderByDescending(x => x.CreatedAt); else ApplyOrderBy(x => x.CreatedAt);
        }
        if (page.HasValue && pageSize.HasValue && page > 0 && pageSize > 0)
        {
            ApplyPaging((page.Value - 1) * pageSize.Value, pageSize.Value);
        }
        SetTracking(true);
    }
}
