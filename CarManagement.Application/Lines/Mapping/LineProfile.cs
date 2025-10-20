using AutoMapper;
using CarManagement.Application.Lines.Dtos;
using CarManagement.Domain.Entities;

namespace CarManagement.Application.Lines.Mapping;

public sealed class LineProfile : Profile
{
    public LineProfile()
    {
        CreateMap<Line, LineDto>();
        CreateMap<ModelYear, ModelYearDto>();
    }
}
