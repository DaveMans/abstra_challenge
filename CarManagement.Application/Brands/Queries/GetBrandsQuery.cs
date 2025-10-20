using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarManagement.Application.Brands.Dtos;
using CarManagement.Application.Common.Pagination;
using CarManagement.Application.Brands.Specifications;
using CarManagement.Domain.Entities;
using CarManagement.Domain.Interfaces.Repositories;
using CarManagement.Domain.Specifications;
using MediatR;

namespace CarManagement.Application.Brands.Queries;

public sealed record GetBrandsQuery(int Page = 1, int PageSize = 20, string? Search = null, string? Country = null, string? SortBy = null, bool Desc = false) : IRequest<PagedResult<BrandDto>>;

public sealed class GetBrandsQueryHandler : IRequestHandler<GetBrandsQuery, PagedResult<BrandDto>>
{
    private readonly IBrandRepository _repo;
    private readonly IMapper _mapper;

    public GetBrandsQueryHandler(IBrandRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<PagedResult<BrandDto>> Handle(GetBrandsQuery request, CancellationToken cancellationToken)
    {
        var spec = new BrandSearchSpecification(request.Search, request.Country, request.SortBy, request.Desc, request.Page, request.PageSize);
        var items = await _repo.ListAsync(spec, cancellationToken);
        var count = await _repo.CountAsync(new BrandSearchSpecification(request.Search, request.Country, request.SortBy, request.Desc, null, null), cancellationToken);
        var dtos = items.Select(x => _mapper.Map<BrandDto>(x)).ToList();
        return new PagedResult<BrandDto>
        {
            Items = dtos,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = count
        };
    }
}
