using Pos.App.BaseModule.Models;
using Pos.App.Sales.Models.Brand;

namespace Pos.App.Sales.Interfaces.Shared;

public interface IBrandService
{
    Task<BaseResponse<List<SelectDataResponse>>> GetListBrand();
    Task<PaginatedResponse<BrandResponse>> GetPaginatedBrand(BrandFilter filter);
    Task<BaseResponse<BrandResponseSingle>> GetBrandById(Guid id);
    Task<BaseResponse<BrandResponse>> CreateBrand(CreateBrandRequest model);
    Task<BaseResponse<BrandResponse>> UpdateBrand(Guid id, UpdateBrandRequest model);
    Task DeleteBrand(Guid id);
}