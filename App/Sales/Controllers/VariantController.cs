using Microsoft.AspNetCore.Mvc;
using Pos.App.BaseModule.Models;
using Pos.App.Sales.Interfaces.Shared;
using Pos.App.Sales.Models.Variant;

namespace Pos.App.Sales.Controllers;

[ApiController]
[Route("api/variant")]
public class VariantController(
        IVariantService variantService
    ) : ControllerBase
{
    private readonly IVariantService _variantService = variantService;

    [HttpGet]
    [Produces("application/json")]
    public async Task<ActionResult<PaginatedResponse<VariantResponse>>> GetPaginatedVariant([FromQuery] VariantFilter filter)
    {
        PaginatedResponse<VariantResponse> variants = await _variantService.GetPaginatedVariant(filter);
        return Ok(variants);
    }

    [HttpGet("{id}")]
    [Produces("application/json")]
    public async Task<ActionResult<BaseResponse<VariantResponseSingle>>> GetVariantById(Guid id)
    {
        BaseResponse<VariantResponseSingle> variant = await _variantService.GetVariantById(id);
        return Ok(variant);
    }

    [HttpPost]
    [Produces("application/json")]
    public async Task<ActionResult<BaseResponse<VariantResponseSingle>>> CreateVariant(CreateVariantRequest model)
    {
        BaseResponse<VariantResponseSingle> variant = await _variantService.CreateVariant(model);
        return Ok(variant);
    }

    [HttpPut("{id}")]
    [Produces("application/json")]
    public async Task<ActionResult<BaseResponse<VariantResponseSingle>>> UpdateVariant(Guid id, UpdateVariantRequest model)
    {
        BaseResponse<VariantResponseSingle> variant = await _variantService.UpdateVariant(id, model);
        return Ok(variant);
    }

    [HttpDelete("{id}")]
    [Produces("application/json")]
    public async Task<IActionResult> DeleteVariant(Guid id)
    {
        await _variantService.DeleteVariant(id);
        return Ok(new { message = "success" });
    }
}