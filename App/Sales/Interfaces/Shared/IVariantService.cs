using Pos.App.BaseModule.Models;
using Pos.App.Sales.Models.Variant;

namespace Pos.App.Sales.Interfaces.Shared;

public interface IVariantService
{
    Task<BaseResponse<List<SelectDataResponse>>> GetListVariant();
    Task<PaginatedResponse<VariantFrontResponse>> GetPaginatedVariant(VariantFilter filter);
    Task<BaseResponse<VariantResponseSingle>> GetVariantById(Guid id);
    Task<BaseResponse<VariantResponse>> CreateVariant(CreateVariantRequest model);
    Task<BaseResponse<VariantResponse>> UpdateVariant(Guid id, UpdateVariantRequest model);
    Task DeleteVariant(Guid id);
}