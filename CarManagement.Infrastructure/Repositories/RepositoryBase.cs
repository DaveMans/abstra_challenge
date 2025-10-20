using CarManagement.Domain.Interfaces.Repositories;
using CarManagement.Domain.Specifications;
using CarManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CarManagement.Infrastructure.Repositories;

public class RepositoryBase<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _db;

    public RepositoryBase(AppDbContext db)
    {
        _db = db;
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _db.Set<T>().FindAsync([id], cancellationToken);
    }

    public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        var query = SpecificationEvaluator.GetQuery(_db.Set<T>().AsQueryable(), specification);
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        var query = SpecificationEvaluator.GetQuery(_db.Set<T>().AsQueryable(), specification);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        var query = SpecificationEvaluator.GetQuery(_db.Set<T>().AsQueryable(), specification);
        return await query.AnyAsync(cancellationToken);
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _db.Set<T>().AddAsync(entity, cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        await _db.Set<T>().AddRangeAsync(entities, cancellationToken);
    }

    public void Update(T entity)
    {
        _db.Set<T>().Update(entity);
    }

    public void UpdateRange(IEnumerable<T> entities)
    {
        _db.Set<T>().UpdateRange(entities);
    }

    public void Remove(T entity)
    {
        _db.Set<T>().Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        _db.Set<T>().RemoveRange(entities);
    }
}
