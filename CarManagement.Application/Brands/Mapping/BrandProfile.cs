using AutoMapper;
using CarManagement.Application.Brands.Dtos;
using CarManagement.Domain.Entities;

namespace CarManagement.Application.Brands.Mapping;

public sealed class BrandProfile : Profile
{
    public BrandProfile()
    {
        CreateMap<Brand, BrandDto>();
        CreateMap<CreateBrandRequest, Brand>();
        CreateMap<UpdateBrandRequest, Brand>();
    }
}
