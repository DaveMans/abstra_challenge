using AutoMapper;
using CarManagement.Application.Lines.Dtos;
using CarManagement.Application.Lines.Specifications;
using CarManagement.Domain.Entities;
using CarManagement.Domain.Interfaces.Repositories;
using CarManagement.Domain.Specifications;
using MediatR;

namespace CarManagement.Application.Lines.Queries;

public sealed record GetLinesByBrandQuery(Guid BrandId) : IRequest<IReadOnlyList<LineDto>>;

public sealed class GetLinesByBrandQueryHandler : IRequestHandler<GetLinesByBrandQuery, IReadOnlyList<LineDto>>
{
    private readonly ILineRepository _lines;
    private readonly IMapper _mapper;

    public GetLinesByBrandQueryHandler(ILineRepository lines, IMapper mapper)
    {
        _lines = lines;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<LineDto>> Handle(GetLinesByBrandQuery request, CancellationToken cancellationToken)
    {
        var spec = new LinesByBrandWithYearsSpec(request.BrandId);
        var entities = await _lines.ListAsync(spec, cancellationToken);
        return entities.Select(_mapper.Map<LineDto>).ToList();
    }
}
