using Pos.App.Sales.Models.Item;
using Pos.Entities;

namespace Pos.App.Sales.Interfaces.Repositories;

public interface IItemRepo
{
    Task<(List<Item>, int, int)> GetPaginatedItems(ItemFilter itemFilter, int pageIndex, int pageSize);
    Task<List<Item>> GetItems(List<Guid> ids);
    Task<Item> GetItem(Guid id);
    Task<Item> GetFullItem(Guid id);
    Task<bool> CheckItemIdExist(Guid id);
    Task<bool> CheckItemWithVariantExist(Guid variantId);
    Task CreateItem(Item item);
    Task UpdateItem(Item item);
    Task DeleteItem(Item item);
}