using Microsoft.AspNetCore.Mvc;
using Pos.App.BaseModule.Models;
using Pos.App.Sales.Interfaces.Shared;
using Pos.App.Sales.Models.Cart;

namespace Pos.App.Sales.Controllers;

[ApiController]
[Route("api/cart")]
public class CartController(
        ICartService cartService
    ) : ControllerBase
{
    private readonly ICartService _cartService = cartService;

    [HttpGet("{key}")]
    [Produces("application/json")]
    public async Task<ActionResult<BaseResponse<CartResponse>>> GetCartById(string key)
    {
        BaseResponse<CartResponse> cart = await _cartService.GetCartByKey(key);
        return Ok(cart);
    }

    [HttpPost]
    [Produces("application/json")]
    public async Task<ActionResult<BaseResponse<CartResponse>>> CreateNewCart(CartRequest model)
    {
        BaseResponse<CartResponse> cart = await _cartService.AddToNewCart(model);
        return Ok(cart);
    }

    [HttpPost("{key}")]
    [Produces("application/json")]
    public async Task<ActionResult<BaseResponse<CartResponse>>> AddToCart(string key, CartRequest model)
    {
        BaseResponse<CartResponse> cart = await _cartService.AddToExistingCart(key, model);
        return Ok(cart);
    }

    [HttpPut("{key}")]
    [Produces("application/json")]
    public async Task<ActionResult<BaseResponse<CartResponse>>> UpdateCart(string key, CartRequest model)
    {
        BaseResponse<CartResponse> cart = await _cartService.UpdateQuantityCart(key, model);
        return Ok(cart);
    }

    [HttpDelete("{key}")]
    [Produces("application/json")]
    public async Task<IActionResult> DeleteCart(string key)
    {
        await _cartService.DeleteCart(key);
        return Ok(new { message = "success" });
    }
}