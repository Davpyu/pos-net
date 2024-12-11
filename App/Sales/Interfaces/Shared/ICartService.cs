using Pos.App.BaseModule.Models;
using Pos.App.Sales.Models.Cart;

namespace Pos.App.Sales.Interfaces.Shared;

public interface ICartService
{
    Task<BaseResponse<CartResponse>> GetCartByKey(string key);
    Task<BaseResponse<CartResponse>> AddToNewCart(CartRequest model);
    Task<BaseResponse<CartResponse>> AddToExistingCart(string key, CartRequest model);
    Task<BaseResponse<CartResponse>> UpdateQuantityCart(string key, CartRequest model);
    Task DeleteCart(string key);
}