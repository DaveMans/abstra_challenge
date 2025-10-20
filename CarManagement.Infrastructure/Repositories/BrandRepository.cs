using CarManagement.Domain.Entities;
using CarManagement.Domain.Interfaces.Repositories;
using CarManagement.Infrastructure.Persistence;

namespace CarManagement.Infrastructure.Repositories;

public sealed class BrandRepository : RepositoryBase<Brand>, IBrandRepository
{
    public BrandRepository(AppDbContext db) : base(db)
    {
    }
}
