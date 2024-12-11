using AutoMapper;
using Pos.App.BaseModule.Models;
using Pos.App.Sales.Interfaces.Repositories;
using Pos.App.Sales.Interfaces.Shared;
using Pos.App.Sales.Models.Brand;
using Pos.Entities;
using Microsoft.Extensions.Localization;
using Pos.Exceptions;
using Pos.Helpers;

namespace Pos.App.Sales.Services;

public class BrandService(
    IMapper mapper,
    IBrandRepo brandRepo,
    IStringLocalizer<BrandService> localizer
    ) : IBrandService
{
    private readonly IMapper _mapper = mapper;
    private readonly IBrandRepo _brandRepo = brandRepo;
    private readonly IStringLocalizer<BrandService> _localizer = localizer;

    public async Task<BaseResponse<List<SelectDataResponse>>> GetListBrand()
    {
        List<Brand> brands = await _brandRepo.GetAllBrands();

        return GlobalHelpers.CreateBaseResponse("success", _mapper.Map<List<SelectDataResponse>>(brands));
    }

    public async Task<PaginatedResponse<BrandResponse>> GetPaginatedBrand(BrandFilter filter)
    {
        int pageSize = GlobalHelpers.ValidatePageSize(filter.pageSize);
        int pageIndex = filter.page > 1 ? filter.page : 1;

        (List<Brand> result, int count, int totalPages) = await _brandRepo.GetPaginatedBrands(filter, pageIndex, pageSize);

        return GlobalHelpers.CreatePaginatedResponse("success", _mapper.Map<IEnumerable<BrandResponse>>(result), pageIndex, totalPages, count);
    }

    public async Task<BaseResponse<BrandResponseSingle>> GetBrandById(Guid id)
    {
        Brand brand = await getFullBrandById(id);
        return GlobalHelpers.CreateBaseResponse("success", _mapper.Map<BrandResponseSingle>(brand));
    }

    public async Task<BaseResponse<BrandResponse>> CreateBrand(CreateBrandRequest model)
    {
        // Check if Brand Name Already exist on database
        if (await _brandRepo.CheckBrandNameExist(model.Name))
        {
            throw new AppException(_localizer["duplicate_brand_name"], "duplicate_brand_name");
        }

        Brand brand = _mapper.Map<Brand>(model);

        await _brandRepo.CreateBrand(brand);

        return GlobalHelpers.CreateBaseResponse("success", _mapper.Map<BrandResponse>(brand));
    }

    public async Task<BaseResponse<BrandResponse>> UpdateBrand(Guid id, UpdateBrandRequest model)
    {
        Brand brand = await getBrand(id);

        // check if Brand Name Already exist on database (in another id)
        if (brand.Name != model.Name)
        {
            if (await _brandRepo.CheckBrandNameExist(model.Name))
            {
                throw new AppException(_localizer["duplicate_brand_name"], "duplicate_brand_name");
            }
        }

        brand.Name = model.Name;

        await _brandRepo.UpdateBrand(brand);

        return GlobalHelpers.CreateBaseResponse("success", _mapper.Map<BrandResponse>(brand));
    }

    public async Task DeleteBrand(Guid id)
    {
        if (await _brandRepo.CheckBrandHasVariant(id))
        {
            throw new AppException(_localizer["cant_delete_brand_has_data"], "cant_delete_brand_has_data");
        }

        Brand brand = await getBrand(id);

        await _brandRepo.DeleteBrand(brand);
    }

    /**
    * private methods
    */

    private async Task<Brand> getBrand(Guid id)
    {
        Brand brand = await _brandRepo.GetBrand(id) ?? throw new KeyNotFoundException(_localizer["brand_not_found"]);
        return brand;
    }

    private async Task<Brand> getFullBrandById(Guid id)
    {
        Brand brand = await _brandRepo.GetFullBrand(id) ?? throw new KeyNotFoundException(_localizer["brand_not_found"]);
        return brand;
    }
}