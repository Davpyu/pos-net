using AutoMapper;
using Pos.App.Sales.Models.Item;
using Pos.Entities;

namespace Pos.App.Sales.Accounts.Profiles;

public class ItemProfile : Profile
{
    public ItemProfile()
    {
        CreateMap<Item, ItemResponse>()
            .ForMember(dest =>
                dest.Name,
                opt => opt.MapFrom( src => src.Variant.Brand.Name + " " + src.Variant.Name ));

        CreateMap<Item, ItemResponseSingle>()
            .ForMember(dest =>
                dest.Brand,
                opt => opt.MapFrom( src => src.Variant.Brand ));

        CreateMap<CreateItemRequest, Item>();
    }
}