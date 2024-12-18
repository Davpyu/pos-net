using AutoMapper;
using Pos.App.BaseModule.Models;
using Pos.App.Sales.Interfaces.Repositories;
using Pos.App.Sales.Interfaces.Shared;
using Pos.App.Sales.Models.Item;
using Pos.Entities;
using Microsoft.Extensions.Localization;
using Pos.Helpers;
using Pos.Exceptions;

namespace Pos.App.Sales.Services;

public class ItemService(
    IMapper mapper,
    IItemRepo itemRepo,
    IVariantRepo variantRepo,
    IStringLocalizer<ItemService> localizer
    ) : IItemService
{
    private readonly IMapper _mapper = mapper;
    private readonly IItemRepo _itemRepo = itemRepo;
    private readonly IVariantRepo _variantRepo = variantRepo;
    private readonly IStringLocalizer<ItemService> _localizer = localizer;

    public async Task<PaginatedResponse<ItemResponse>> GetPaginatedItem(ItemFilter filter)
    {
        int pageSize = GlobalHelpers.ValidatePageSize(filter.pageSize);
        int pageIndex = filter.page > 1 ? filter.page : 1;

        (List<Item> result, int count, int totalPages) = await _itemRepo.GetPaginatedItems(filter, pageIndex, pageSize);

        return GlobalHelpers.CreatePaginatedResponse("success", _mapper.Map<IEnumerable<ItemResponse>>(result), pageIndex, totalPages, count);
    }

    public async Task<BaseResponse<ItemResponseSingle>> GetItemById(Guid id)
    {
        Item item = await getFullItemById(id);
        return GlobalHelpers.CreateBaseResponse("success", _mapper.Map<ItemResponseSingle>(item));
    }

    public async Task<BaseResponse<ItemResponse>> CreateItem(CreateItemRequest model)
    {
        // Check if Variant not exist on database
        if (! await _variantRepo.CheckVariantIdExist(model.VariantId))
        {
            throw new KeyNotFoundException(_localizer["variant_not_found"]);
        }

        // Check if Item Name Already exist on database
        if (await _itemRepo.CheckItemWithVariantExist(model.VariantId))
        {
            throw new AppException(_localizer["duplicate_item"], "duplicate_item");
        }

        Item item = _mapper.Map<Item>(model);

        await _itemRepo.CreateItem(item);

        item.Variant = await _variantRepo.GetFullVariant(item.VariantId);

        return GlobalHelpers.CreateBaseResponse("success", _mapper.Map<ItemResponse>(item));
    }

    public async Task<BaseResponse<ItemResponse>> UpdateItem(Guid id, UpdateItemRequest model)
    {
        Item item = await getItem(id);

        // Check if Variant not exist on database
        if (! await _variantRepo.CheckVariantIdExist(model.VariantId))
        {
            throw new KeyNotFoundException(_localizer["variant_not_found"]);
        }

        // check if Item Name Already exist on database (in another id)
        if (item.VariantId != model.VariantId)
        {
            if (await _itemRepo.CheckItemWithVariantExist(model.VariantId))
            {
                throw new AppException(_localizer["duplicate_item"], "duplicate_item");
            }
        }

        item.VariantId = model.VariantId;
        item.Price = model.Price;
        item.Quantity = model.Quantity;

        await _itemRepo.UpdateItem(item);

        item.Variant = await _variantRepo.GetFullVariant(item.VariantId);

        return GlobalHelpers.CreateBaseResponse("success", _mapper.Map<ItemResponse>(item));

    }

    public async Task DeleteItem(Guid id)
    {
        Item item = await getItem(id);

        await _itemRepo.DeleteItem(item);
    }

    /**
    * private methods
    */

    private async Task<Item> getItem(Guid id)
    {
        Item item = await _itemRepo.GetItem(id) ?? throw new KeyNotFoundException(_localizer["item_not_found"]);
        return item;
    }

    private async Task<Item> getFullItemById(Guid id)
    {
        Item item = await _itemRepo.GetFullItem(id) ?? throw new KeyNotFoundException(_localizer["item_not_found"]);
        return item;
    }
}