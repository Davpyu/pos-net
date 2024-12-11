using AutoMapper;
using Pos.App.Sales.Models.Item;
using Pos.Entities;

namespace Pos.App.Sales.Accounts.Profiles;

public class ItemProfile : Profile
{
    public ItemProfile()
    {
        CreateMap<Item, ItemResponse>();

        CreateMap<Item, ItemResponseSingle>();

        CreateMap<CreateItemRequest, Item>();
    }
}