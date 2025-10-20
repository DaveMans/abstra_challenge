using AutoMapper;
using CarManagement.Application.Brands.Dtos;
using CarManagement.Domain.Entities;
using CarManagement.Domain.Interfaces;
using CarManagement.Domain.Interfaces.Repositories;
using MediatR;

namespace CarManagement.Application.Brands.Commands;

public sealed record CreateBrandCommand(string Name, string Country, int FoundedYear) : IRequest<BrandDto>;

public sealed class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, BrandDto>
{
    private readonly IBrandRepository _brands;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CreateBrandCommandHandler(IBrandRepository brands, IUnitOfWork uow, IMapper mapper)
    {
        _brands = brands;
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<BrandDto> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
    {
        var entity = new Brand
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Country = request.Country.Trim(),
            FoundedYear = request.FoundedYear,
            IsActive = true
        };
        await _brands.AddAsync(entity, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        return _mapper.Map<BrandDto>(entity);
    }
}
