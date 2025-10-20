using CarManagement.Domain.Entities;
using CarManagement.Domain.Interfaces.Repositories;
using CarManagement.Infrastructure.Persistence;

namespace CarManagement.Infrastructure.Repositories;

public sealed class LineRepository : RepositoryBase<Line>, ILineRepository
{
    public LineRepository(AppDbContext db) : base(db)
    {
    }
}
