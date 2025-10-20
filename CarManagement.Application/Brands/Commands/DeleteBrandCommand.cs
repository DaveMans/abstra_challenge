using CarManagement.Domain.Interfaces;
using CarManagement.Domain.Interfaces.Repositories;
using MediatR;
using CarManagement.Application.Brands.Specifications;

namespace CarManagement.Application.Brands.Commands;

public sealed record DeleteBrandCommand(Guid Id) : IRequest<bool>;

public sealed class DeleteBrandCommandHandler : IRequestHandler<DeleteBrandCommand, bool>
{
    private readonly IBrandRepository _brands;
    private readonly ILineRepository _lines;
    private readonly IUnitOfWork _uow;

    public DeleteBrandCommandHandler(IBrandRepository brands, ILineRepository lines, IUnitOfWork uow)
    {
        _brands = brands;
        _lines = lines;
        _uow = uow;
    }

    public async Task<bool> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
    {
        var brand = await _brands.GetByIdAsync(request.Id, cancellationToken);
        if (brand is null) return false;

        var hasActiveLines = await _lines.AnyAsync(new LinesByBrandActiveSpec(request.Id), cancellationToken);
        if (hasActiveLines) throw new InvalidOperationException("Cannot delete a brand with active lines.");

        _brands.Remove(brand);
        await _uow.SaveChangesAsync(cancellationToken);
        return true;
    }
}
