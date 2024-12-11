using AutoMapper;
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

        CreateMap<Variant, VariantResponse>();

        CreateMap<Variant, VariantResponseSingle>();

        CreateMap<CreateVariantRequest, Variant>();
    }
}