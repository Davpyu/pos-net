using AutoMapper;
using Pos.App.Sales.Models.Cart;
using Pos.Entities;

namespace Pos.App.Sales.Accounts.Profiles;

public class CartProfile : Profile
{
    public CartProfile()
    {
        CreateMap<Item, CartResponseSingle>()
            .ForMember(dest =>
                dest.Brand,
                opt => opt.MapFrom( src => src.Variant.Brand.Name ))
            .ForMember(dest =>
                dest.ItemId,
                opt => opt.MapFrom( src => src.Id ))
            .ForMember(dest =>
                dest.Variant,
                opt => opt.MapFrom( src => src.Variant.Name ));
    }
}