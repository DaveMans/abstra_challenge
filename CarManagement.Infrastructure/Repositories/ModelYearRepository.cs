using CarManagement.Domain.Entities;
using CarManagement.Domain.Interfaces.Repositories;
using CarManagement.Infrastructure.Persistence;

namespace CarManagement.Infrastructure.Repositories;

public sealed class ModelYearRepository : RepositoryBase<ModelYear>, IModelYearRepository
{
    public ModelYearRepository(AppDbContext db) : base(db)
    {
    }
}
