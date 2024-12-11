using AutoMapper;
using Pos.App.BaseModule.Models;
using Pos.App.Sales.Models.Brand;
using Pos.Entities;

namespace Pos.App.Sales.Accounts.Profiles;

public class BrandProfile : Profile
{
    public BrandProfile()
    {
        CreateMap<Brand, BrandResponse>();

        CreateMap<Brand, BrandResponseSingle>();

        CreateMap<CreateBrandRequest, Brand>();

        CreateMap<Brand, SelectDataResponse>()
            .ForMember(dest =>
                dest.Label,
                opt => opt.MapFrom( src => src.Name ))
            .ForMember(dest =>
                dest.Value,
                opt => opt.MapFrom( src => src.Id ));
    }
}