using AutoMapper;
using CarManagement.Application.Brands.Dtos;
using CarManagement.Application.Brands.Specifications;
using CarManagement.Domain.Entities;
using CarManagement.Domain.Interfaces.Repositories;
using CarManagement.Domain.Specifications;
using MediatR;

namespace CarManagement.Application.Brands.Queries;

public sealed record GetBrandByIdQuery(Guid Id) : IRequest<BrandDto?>;

public sealed class GetBrandByIdQueryHandler : IRequestHandler<GetBrandByIdQuery, BrandDto?>
{
    private readonly IBrandRepository _repo;
    private readonly IMapper _mapper;

    public GetBrandByIdQueryHandler(IBrandRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<BrandDto?> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
    {
        var spec = new BrandByIdWithLinesSpec(request.Id);
        var items = await _repo.ListAsync(spec, cancellationToken);
        var brand = items.FirstOrDefault();
        return brand is null ? null : _mapper.Map<BrandDto>(brand);
    }
}
