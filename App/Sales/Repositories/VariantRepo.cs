using Microsoft.EntityFrameworkCore;
using Pos.App.Sales.Interfaces.Repositories;
using Pos.App.Sales.Models.Variant;
using Pos.Entities;
using Pos.Enums;
using Pos.Helpers;

namespace Pos.App.Sales.Repositories;

public class VariantRepo(
    DataContext context
    ) : IVariantRepo
{
    public readonly DataContext _context = context;

    public async Task<(List<Variant>, int, int)> GetPaginatedVariants(VariantFilter variantFilter, int pageIndex, int pageSize)
    {
        var query = _context.Variants.AsQueryable();

        query = query.Include(x => x.Brand);

        if (variantFilter.query != null)
        {
            query = query.Where(x => EF.Functions.Like(x.Name, $"%{variantFilter.query}%"));
        }

        query = variantFilter.sort switch
        {
            DefaultSort.Alphabetical => query.OrderBy(x => x.Name),
            DefaultSort.ReverseAlphabetical => query.OrderByDescending(x => x.Name),
            DefaultSort.Oldest => query.OrderBy(x => x.Created),
            _ => query.OrderByDescending(x => x.Created)
        };

        List<Variant> result = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        int count = await query.CountAsync();
        int totalPages = (int)Math.Ceiling(count / (double)pageSize);
        return (result, count, totalPages);
    }

    public async Task<List<Variant>> GetAllVariants()
    {
        return await _context.Variants.Include(x => x.Brand).ToListAsync();
    }

    public async Task<Variant> GetVariant(Guid id)
    {
        return await _context.Variants.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Variant> GetFullVariant(Guid id)
    {
        IQueryable<Variant> query = _context.Variants.AsQueryable();

        query = query.Include(x => x.Brand);

        return await query.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> CheckVariantIdExist(Guid id)
    {
        return await _context.Variants.AnyAsync(x => x.Id == id);
    }

    public async Task<bool> CheckVariantNameExist(string variantName, Guid brandId)
    {
        return await _context.Variants.AnyAsync(x => x.Name == variantName && x.BrandId == brandId);
    }

    public async Task<bool> CheckVariantHasItems(Guid id)
    {
        return await _context.Items.AnyAsync(x => x.VariantId == id);
    }

    public async Task CreateVariant(Variant Variant)
    {
        await _context.Variants.AddAsync(Variant);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateVariant(Variant Variant)
    {
        _context.Variants.Update(Variant);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteVariant(Variant Variant)
    {
        _context.Variants.Remove(Variant);
        await _context.SaveChangesAsync();
    }
}