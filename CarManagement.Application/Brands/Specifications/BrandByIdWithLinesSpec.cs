using CarManagement.Domain.Entities;
using CarManagement.Domain.Specifications;

namespace CarManagement.Application.Brands.Specifications;

public sealed class BrandByIdWithLinesSpec : BaseSpecification<Brand>
{
    public BrandByIdWithLinesSpec(Guid id)
    {
        Criteria = x => !x.IsDeleted && x.Id == id;
        AddInclude(b => b.Lines);
        SetTracking(true);
    }
}
