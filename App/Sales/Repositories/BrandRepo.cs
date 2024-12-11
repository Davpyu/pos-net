using Microsoft.EntityFrameworkCore;
using Pos.App.Sales.Interfaces.Repositories;
using Pos.App.Sales.Models.Brand;
using Pos.Entities;
using Pos.Enums;
using Pos.Helpers;

namespace Pos.App.Sales.Repositories;

public class BrandRepo(
    DataContext context
    ) : IBrandRepo
{
    public readonly DataContext _context = context;

    public async Task<(List<Brand>, int, int)> GetPaginatedBrands(BrandFilter brandFilter, int pageIndex, int pageSize)
    {
        var query = _context.Brands.AsQueryable();

        if (brandFilter.query != null)
        {
            query = query.Where(x => EF.Functions.Like(x.Name, $"%{brandFilter.query}%"));
        }

        query = brandFilter.sort switch
        {
            DefaultSort.Alphabetical => query.OrderBy(x => x.Name),
            DefaultSort.ReverseAlphabetical => query.OrderByDescending(x => x.Name),
            DefaultSort.Oldest => query.OrderBy(x => x.Created),
            _ => query.OrderByDescending(x => x.Created)
        };

        List<Brand> result = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        int count = await query.CountAsync();
        int totalPages = (int)Math.Ceiling(count / (double)pageSize);
        return (result, count, totalPages);
    }

    public async Task<List<Brand>> GetAllBrands()
    {
        return await _context.Brands.ToListAsync();
    }

    public async Task<Brand> GetBrand(Guid id)
    {
        return await _context.Brands.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Brand> GetFullBrand(Guid id)
    {
        IQueryable<Brand> query = _context.Brands.AsQueryable();

        query = query.Include(x => x.Variants);

        return await query.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> CheckBrandIdExist(Guid id)
    {
        return await _context.Brands.AnyAsync(x => x.Id == id);
    }

    public async Task<bool> CheckBrandNameExist(string brandName)
    {
        return await _context.Brands.AnyAsync(x => x.Name == brandName);
    }

    public async Task<bool> CheckBrandHasVariant(Guid id)
    {
        return await _context.Variants.AnyAsync(x => x.BrandId == id);
    }

    public async Task CreateBrand(Brand Brand)
    {
        await _context.Brands.AddAsync(Brand);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateBrand(Brand Brand)
    {
        _context.Brands.Update(Brand);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteBrand(Brand Brand)
    {
        _context.Brands.Remove(Brand);
        await _context.SaveChangesAsync();
    }
}