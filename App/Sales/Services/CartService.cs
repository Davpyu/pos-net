using AutoMapper;
using Pos.App.BaseModule.Models;
using Pos.App.Sales.Interfaces.Repositories;
using Pos.App.Sales.Interfaces.Shared;
using Pos.Entities;
using Microsoft.Extensions.Localization;
using Pos.Exceptions;
using Pos.Helpers;
using Pos.App.BaseModule.Interfaces.Shared;
using Pos.App.Sales.Models.Cart;
using System.Text.Json;

namespace Pos.App.Sales.Services;

public class CartService(
        IMapper mapper,
        IItemRepo itemRepo,
        ICacheService cacheService,
        IStringLocalizer<CartService> localizer
    ) : ICartService
{
    private readonly IMapper _mapper = mapper;
    private readonly IItemRepo _itemRepo = itemRepo;
    private readonly ICacheService _cacheService = cacheService;
    private readonly IStringLocalizer<CartService> _localizer = localizer;

    public async Task<BaseResponse<CartResponse>> AddToExistingCart(string key, CartRequest model)
    {
        CartCacheResponse cart = await _cacheService.GetRedisCache<CartCacheResponse>(key) ?? throw new AppException(_localizer["cart_not_found"], "cart_not_found");
        Item item = await _itemRepo.GetItem(model.ItemId);

        if (cart.Carts.Any(x => x.ItemId == model.ItemId))
        {
            CartRequest current = cart.Carts.Where(x => x.ItemId == model.ItemId).First();

            if (current.Quantity + model.Quantity > item.Quantity)
            {
                throw new AppException(_localizer["item_quantity_too_large"], "item_quantity_too_large");
            }

            current.Quantity += model.Quantity;
        }

        else
        {
            if (model.Quantity > item.Quantity)
            {
                throw new AppException(_localizer["item_quantity_too_large"], "item_quantity_too_large");
            }

            cart.Carts.Add(model);
        }

        await _cacheService.UpdateRedisCache(key, JsonSerializer.Serialize(cart));

        List<Item> items = await _itemRepo.GetItems(cart.Carts.Select(x => x.ItemId).ToList());

        CartResponse response = new()
        {
            Key = key,
            Carts = _mapper.Map<List<CartResponseSingle>>(items)
        };

        foreach (CartResponseSingle current in response.Carts)
        {
            current.Quantity = cart.Carts.Where(x => x.ItemId == current.ItemId).First().Quantity;
        }

        return GlobalHelpers.CreateBaseResponse("success", response);
    }

    public async Task<BaseResponse<CartResponse>> AddToNewCart(CartRequest model)
    {
        string key = GlobalHelpers.RandomString(6);

        CartCacheResponse cart = new()
        {
            Key = key,
            Carts = [
                model
            ]
        };

        await _cacheService.WriteRedisCache(key, JsonSerializer.Serialize(cart));

        List<Item> items = await _itemRepo.GetItems(cart.Carts.Select(x => x.ItemId).ToList());

        CartResponse response = new()
        {
            Key = key,
            Carts = _mapper.Map<List<CartResponseSingle>>(items)
        };

        foreach (CartResponseSingle current in response.Carts)
        {
            current.Quantity = cart.Carts.Where(x => x.ItemId == current.ItemId).First().Quantity;
        }

        return GlobalHelpers.CreateBaseResponse("success", response);
    }

    public async Task DeleteCart(string key)
    {
        if (await _cacheService.ExistRedisCache(key))
        {
            await _cacheService.DeleteCache(key);
        }
    }

    public async Task<BaseResponse<CartResponse>> GetCartByKey(string key)
    {
        CartCacheResponse cart = await _cacheService.GetRedisCache<CartCacheResponse>(key) ?? throw new AppException(_localizer["cart_not_found"], "cart_not_found");

        List<Item> items = await _itemRepo.GetItems(cart.Carts.Select(x => x.ItemId).ToList());

        CartResponse response = new()
        {
            Key = key,
            Carts = _mapper.Map<List<CartResponseSingle>>(items)
        };

        foreach (CartResponseSingle current in response.Carts)
        {
            current.Quantity = cart.Carts.Where(x => x.ItemId == current.ItemId).First().Quantity;
        }

        return GlobalHelpers.CreateBaseResponse("success", response);
    }

    public async Task<BaseResponse<CartResponse>> UpdateQuantityCart(string key, CartRequest model)
    {
        CartCacheResponse cart = await _cacheService.GetRedisCache<CartCacheResponse>(key) ?? throw new AppException(_localizer["cart_not_found"], "cart_not_found");

        if (! cart.Carts.Any(x => x.ItemId == model.ItemId))
        {
            throw new AppException(_localizer["item_not_found_on_cart"], "item_not_found_on_cart");
        }

        CartRequest item = cart.Carts.Where(x => x.ItemId == model.ItemId).First();

        int quantity = item.Quantity + model.Quantity;

        Item currentItem = await _itemRepo.GetItem(model.ItemId);

        if (quantity > currentItem.Quantity)
        {
            throw new AppException(_localizer["item_quantity_too_large"], "item_quantity_too_large");
        }

        item.Quantity += model.Quantity;

        if (item.Quantity <= 0)
        {
            cart.Carts.Remove(item);
        }

        await _cacheService.UpdateRedisCache(key, JsonSerializer.Serialize(cart));

        List<Item> items = await _itemRepo.GetItems(cart.Carts.Select(x => x.ItemId).ToList());

        CartResponse response = new()
        {
            Key = key,
            Carts = _mapper.Map<List<CartResponseSingle>>(items)
        };

        foreach (CartResponseSingle current in response.Carts)
        {
            current.Quantity = cart.Carts.Where(x => x.ItemId == current.ItemId).First().Quantity;
        }

        return GlobalHelpers.CreateBaseResponse("success", response);
    }
}