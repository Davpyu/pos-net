using Pos.App.BaseModule.Models;
using Pos.App.Sales.Models.Variant;
using Pos.Entities;

namespace Pos.App.Sales.Interfaces.Repositories;

public interface IVariantRepo
{
    Task<(List<Variant>, int, int)> GetPaginatedVariants(VariantFilter variantFilter, int pageIndex, int pageSize);
    Task<Variant> GetVariant(Guid id);
    Task<Variant> GetFullVariant(Guid id);
    Task<bool> CheckVariantIdExist(Guid id);
    Task<bool> CheckVariantNameExist(string variantName, Guid brandId);
    Task CreateVariant(Variant variant);
    Task UpdateVariant(Variant variant);
    Task DeleteVariant(Variant variant);
}