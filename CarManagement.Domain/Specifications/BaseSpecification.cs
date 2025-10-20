using System.Linq.Expressions;

namespace CarManagement.Domain.Specifications;

public abstract class BaseSpecification<T> : ISpecification<T>
{
    public Expression<Func<T, bool>>? Criteria { get; protected set; }
    public List<Expression<Func<T, object?>>> MutIncludes { get; } = new();
    public List<string> MutIncludeStrings { get; } = new();
    public Expression<Func<T, object?>>? OrderBy { get; protected set; }
    public Expression<Func<T, object?>>? OrderByDescending { get; protected set; }
    public int? Take { get; protected set; }
    public int? Skip { get; protected set; }
    public bool AsNoTracking { get; protected set; } = true;

    IReadOnlyList<Expression<Func<T, object?>>> ISpecification<T>.Includes => MutIncludes;
    IReadOnlyList<string> ISpecification<T>.IncludeStrings => MutIncludeStrings;

    protected void AddInclude(Expression<Func<T, object?>> include) => MutIncludes.Add(include);
    protected void AddInclude(string include) => MutIncludeStrings.Add(include);
    protected void ApplyPaging(int skip, int take) { Skip = skip; Take = take; }
    protected void ApplyOrderBy(Expression<Func<T, object?>> orderBy) { OrderBy = orderBy; }
    protected void ApplyOrderByDescending(Expression<Func<T, object?>> orderByDesc) { OrderByDescending = orderByDesc; }
    protected void SetTracking(bool asNoTracking) { AsNoTracking = asNoTracking; }
}
