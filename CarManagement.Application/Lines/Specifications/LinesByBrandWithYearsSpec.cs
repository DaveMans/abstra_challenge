using CarManagement.Domain.Entities;
using CarManagement.Domain.Specifications;

namespace CarManagement.Application.Lines.Specifications;

public sealed class LinesByBrandWithYearsSpec : BaseSpecification<Line>
{
    public LinesByBrandWithYearsSpec(Guid brandId)
    {
        Criteria = l => !l.IsDeleted && l.BrandId == brandId;
        AddInclude(l => l.ModelYears);
        SetTracking(true);
    }
}
