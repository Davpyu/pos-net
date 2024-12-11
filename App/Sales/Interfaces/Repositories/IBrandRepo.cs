using Pos.App.Sales.Models.Brand;
using Pos.Entities;

namespace Pos.App.Sales.Interfaces.Repositories;

public interface IBrandRepo
{
    Task<(List<Brand>, int, int)> GetPaginatedBrands(BrandFilter brandFilter, int pageIndex, int pageSize);
    Task<List<Brand>> GetAllBrands();
    Task<Brand> GetBrand(Guid id);
    Task<Brand> GetFullBrand(Guid id);
    Task<bool> CheckBrandIdExist(Guid id);
    Task<bool> CheckBrandNameExist(string brandName);
    Task<bool> CheckBrandHasVariant(Guid id);
    Task CreateBrand(Brand Brand);
    Task UpdateBrand(Brand Brand);
    Task DeleteBrand(Brand Brand);
}