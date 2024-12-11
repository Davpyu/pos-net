using Pos.App.BaseModule.Models;
using Pos.App.Sales.Models.Item;

namespace Pos.App.Sales.Interfaces.Shared;

public interface IItemService
{
    Task<PaginatedResponse<ItemResponse>> GetPaginatedItem(ItemFilter filter);
    Task<BaseResponse<ItemResponseSingle>> GetItemById(Guid id);
    Task<BaseResponse<ItemResponse>> CreateItem(CreateItemRequest model);
    Task<BaseResponse<ItemResponse>> UpdateItem(Guid id, UpdateItemRequest model);
    Task DeleteItem(Guid id);
}