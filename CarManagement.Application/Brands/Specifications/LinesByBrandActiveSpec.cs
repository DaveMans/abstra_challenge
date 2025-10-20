using CarManagement.Domain.Specifications;

namespace CarManagement.Application.Brands.Specifications;

public sealed class LinesByBrandActiveSpec : BaseSpecification<CarManagement.Domain.Entities.Line>
{
    public LinesByBrandActiveSpec(Guid brandId)
    {
        Criteria = l => !l.IsDeleted && l.BrandId == brandId && l.IsActive;
        SetTracking(true);
    }
}
