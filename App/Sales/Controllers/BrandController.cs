using Microsoft.AspNetCore.Mvc;
using Pos.App.BaseModule.Models;
using Pos.App.Sales.Interfaces.Shared;
using Pos.App.Sales.Models.Brand;

namespace Pos.App.Sales.Controllers;

[ApiController]
[Route("api/brand")]
public class BrandController(
        IBrandService brandService
    ) : ControllerBase
{
    private readonly IBrandService _brandService = brandService;

    [HttpGet]
    [Produces("application/json")]
    public async Task<ActionResult<PaginatedResponse<BrandResponse>>> GetPaginatedBrand([FromQuery] BrandFilter filter)
    {
        PaginatedResponse<BrandResponse> brands = await _brandService.GetPaginatedBrand(filter);
        return Ok(brands);
    }

    [HttpGet("select-data")]
    [Produces("application/json")]
    public async Task<ActionResult<BaseResponse<List<SelectDataResponse>>>> GetBrandSelectData()
    {
        BaseResponse<List<SelectDataResponse>> brand = await _brandService.GetListBrand();
        return Ok(brand);
    }

    [HttpGet("{id}")]
    [Produces("application/json")]
    public async Task<ActionResult<BaseResponse<BrandResponseSingle>>> GetBrandById(Guid id)
    {
        BaseResponse<BrandResponseSingle> brand = await _brandService.GetBrandById(id);
        return Ok(brand);
    }

    [HttpPost]
    [Produces("application/json")]
    public async Task<ActionResult<BaseResponse<BrandResponse>>> CreateBrand(CreateBrandRequest model)
    {
        BaseResponse<BrandResponse> brand = await _brandService.CreateBrand(model);
        return Ok(brand);
    }

    [HttpPut("{id}")]
    [Produces("application/json")]
    public async Task<ActionResult<BaseResponse<BrandResponse>>> UpdateBrand(Guid id, UpdateBrandRequest model)
    {
        BaseResponse<BrandResponse> brand = await _brandService.UpdateBrand(id, model);
        return Ok(brand);
    }

    [HttpDelete("{id}")]
    [Produces("application/json")]
    public async Task<IActionResult> DeleteBrand(Guid id)
    {
        await _brandService.DeleteBrand(id);
        return Ok(new { message = "success" });
    }
}