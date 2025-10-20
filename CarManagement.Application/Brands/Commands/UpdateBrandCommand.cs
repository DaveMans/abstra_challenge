using AutoMapper;
using CarManagement.Application.Brands.Dtos;
using CarManagement.Domain.Interfaces;
using CarManagement.Domain.Interfaces.Repositories;
using MediatR;

namespace CarManagement.Application.Brands.Commands;

public sealed record UpdateBrandCommand(Guid Id, string Name, string Country, int FoundedYear, bool IsActive) : IRequest<BrandDto?>;

public sealed class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, BrandDto?>
{
    private readonly IBrandRepository _brands;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public UpdateBrandCommandHandler(IBrandRepository brands, IUnitOfWork uow, IMapper mapper)
    {
        _brands = brands;
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<BrandDto?> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
    {
        var existing = await _brands.GetByIdAsync(request.Id, cancellationToken);
        if (existing is null) return null;

        existing.Name = request.Name.Trim();
        existing.Country = request.Country.Trim();
        existing.FoundedYear = request.FoundedYear;
        existing.IsActive = request.IsActive;

        _brands.Update(existing);
        await _uow.SaveChangesAsync(cancellationToken);
        return _mapper.Map<BrandDto>(existing);
    }
}
