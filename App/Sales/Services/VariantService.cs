using AutoMapper;
using Pos.App.BaseModule.Models;
using Pos.App.Sales.Interfaces.Repositories;
using Pos.App.Sales.Interfaces.Shared;
using Pos.App.Sales.Models.Variant;
using Pos.Entities;
using Microsoft.Extensions.Localization;
using Pos.Helpers;
using Pos.Exceptions;

namespace Pos.App.Sales.Services;

public class VariantService : IVariantService
{
    private readonly IMapper _mapper;
    private readonly IVariantRepo _variantRepo;
    private readonly IStringLocalizer<VariantService> _localizer;

    public VariantService(
        IMapper mapper,
        IVariantRepo variantRepo,
        IStringLocalizer<VariantService> localizer
    )
    {
        _mapper = mapper;
        _variantRepo = variantRepo;
        _localizer = localizer;
    }


    public async Task<PaginatedResponse<VariantResponse>> GetPaginatedVariant(VariantFilter filter)
    {
        int pageSize = GlobalHelpers.ValidatePageSize(filter.pageSize);
        int pageIndex = filter.page > 1 ? filter.page : 1;

        (List<Variant> result, int count, int totalPages) = await _variantRepo.GetPaginatedVariants(filter, pageIndex, pageSize);

        return GlobalHelpers.CreatePaginatedResponse("success", _mapper.Map<IEnumerable<VariantResponse>>(result), pageIndex, totalPages, count);
    }

    public async Task<BaseResponse<VariantResponseSingle>> GetVariantById(Guid id)
    {
        Variant variant = await getFullVariantById(id);
        return GlobalHelpers.CreateBaseResponse("success", _mapper.Map<VariantResponseSingle>(variant));
    }

    public async Task<BaseResponse<VariantResponse>> CreateVariant(CreateVariantRequest model)
    {
        // Check if Variant Name Already exist on database
        if (await _variantRepo.CheckVariantNameExist(model.Name, model.BrandId))
        {
            throw new AppException(_localizer["duplicate_variant_name"], "duplicate_variant_name");
        }

        Variant variant = _mapper.Map<Variant>(model);

        await _variantRepo.CreateVariant(variant);

        return GlobalHelpers.CreateBaseResponse("success", _mapper.Map<VariantResponse>(variant));
    }

    public async Task<BaseResponse<VariantResponse>> UpdateVariant(Guid id, UpdateVariantRequest model)
    {
        Variant variant = await getVariant(id);

        // check if Variant Name Already exist on database (in another id)
        if (variant.Name != model.Name)
        {
            if (await _variantRepo.CheckVariantNameExist(model.Name, model.BrandId))
            {
                throw new AppException(_localizer["duplicate_variant_name"], "duplicate_variant_name");
            }
        }

        variant.Name = model.Name;
        variant.BrandId = model.BrandId;

        await _variantRepo.UpdateVariant(variant);

        return GlobalHelpers.CreateBaseResponse("success", _mapper.Map<VariantResponse>(variant));

    }

    public async Task DeleteVariant(Guid id)
    {
        Variant variant = await getVariant(id);

        await _variantRepo.DeleteVariant(variant);
    }

    /**
    * private methods
    */

    private async Task<Variant> getVariant(Guid id)
    {
        Variant variant = await _variantRepo.GetVariant(id) ?? throw new KeyNotFoundException(_localizer["variant_not_found"]);
        return variant;
    }

    private async Task<Variant> getFullVariantById(Guid id)
    {
        Variant variant = await _variantRepo.GetFullVariant(id) ?? throw new KeyNotFoundException(_localizer["variant_not_found"]);
        return variant;
    }
}