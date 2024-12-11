using AutoMapper;
using Pos.App.BaseModule.Models;
using Pos.App.Sales.Models.Variant;
using Pos.Entities;

namespace Pos.App.Sales.Accounts.Profiles;

public class VariantProfile : Profile
{
    public VariantProfile()
    {
        CreateMap<Variant, VariantFrontResponse>()
            .ForMember(dest =>
                dest.Brand,
                opt => opt.MapFrom( src => src.Brand.Name ));

        CreateMap<Variant, SelectDataResponse>()
            .ForMember(dest =>
                dest.Label,
                opt => opt.MapFrom( src => src.Brand.Name + " " + src.Name ))
            .ForMember(dest =>
                dest.Value,
                opt => opt.MapFrom( src => src.Id ));

        CreateMap<Variant, VariantResponse>();

        CreateMap<Variant, VariantResponseSingle>();

        CreateMap<CreateVariantRequest, Variant>();
    }
}