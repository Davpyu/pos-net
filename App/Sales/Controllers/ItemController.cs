using Microsoft.AspNetCore.Mvc;
using Pos.App.BaseModule.Models;
using Pos.App.Sales.Interfaces.Shared;
using Pos.App.Sales.Models.Item;

namespace Pos.App.Sales.Controllers;

[ApiController]
[Route("api/item")]
public class ItemController(
        IItemService itemService
    ) : ControllerBase
{
    private readonly IItemService _itemService = itemService;

    [HttpGet]
    [Produces("application/json")]
    public async Task<ActionResult<PaginatedResponse<ItemResponse>>> GetPaginatedItem([FromQuery] ItemFilter filter)
    {
        PaginatedResponse<ItemResponse> items = await _itemService.GetPaginatedItem(filter);
        return Ok(items);
    }

    [HttpGet("{id}")]
    [Produces("application/json")]
    public async Task<ActionResult<BaseResponse<ItemResponseSingle>>> GetItemById(Guid id)
    {
        BaseResponse<ItemResponseSingle> item = await _itemService.GetItemById(id);
        return Ok(item);
    }

    [HttpPost]
    [Produces("application/json")]
    public async Task<ActionResult<BaseResponse<ItemResponseSingle>>> CreateItem(CreateItemRequest model)
    {
        BaseResponse<ItemResponse> item = await _itemService.CreateItem(model);
        return Ok(item);
    }

    [HttpPut("{id}")]
    [Produces("application/json")]
    public async Task<ActionResult<BaseResponse<ItemResponseSingle>>> UpdateItem(Guid id, UpdateItemRequest model)
    {
        BaseResponse<ItemResponse> item = await _itemService.UpdateItem(id, model);
        return Ok(item);
    }

    [HttpDelete("{id}")]
    [Produces("application/json")]
    public async Task<IActionResult> DeleteItem(Guid id)
    {
        await _itemService.DeleteItem(id);
        return Ok(new { message = "success" });
    }
}