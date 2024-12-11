using Microsoft.EntityFrameworkCore;
using Pos.App.Sales.Interfaces.Repositories;
using Pos.App.Sales.Models.Item;
using Pos.Entities;
using Pos.Enums;
using Pos.Helpers;

namespace Pos.App.Sales.Repositories;

public class ItemRepo(
    DataContext context
    ) : IItemRepo
{
    public readonly DataContext _context = context;

    public async Task<(List<Item>, int, int)> GetPaginatedItems(ItemFilter itemFilter, int pageIndex, int pageSize)
    {
        var query = _context.Items.AsQueryable();

        query = query.Include( x => x.Variant ).ThenInclude( x => x.Brand );

        if (itemFilter.query != null)
        {
            query = query.Where(x => EF.Functions.Like(x.Variant.Brand.Name, $"%{itemFilter.query}%") || EF.Functions.Like(x.Variant.Name, $"%{itemFilter.query}%"));
        }

        query = itemFilter.sort switch
        {
            ItemSort.Alphabetical => query.OrderBy( x => x.Variant.Brand.Name ).ThenBy( x => x.Variant.Name ),
            ItemSort.ReverseAlphabetical => query.OrderByDescending( x => x.Variant.Brand.Name ).ThenByDescending( x => x.Variant.Name ),
            ItemSort.Oldest => query.OrderBy(x => x.Created),
            ItemSort.LowestPrice => query.OrderBy( x => x.Price ),
            ItemSort.HighestPrice => query.OrderByDescending( x => x.Price ),
            ItemSort.LowestQuantity => query.OrderBy( x => x.Quantity ),
            ItemSort.HighestQuantity => query.OrderByDescending( x => x.Quantity ),
            _ => query.OrderByDescending(x => x.Created)
        };

        List<Item> result = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        int count = await query.CountAsync();
        int totalPages = (int)Math.Ceiling(count / (double)pageSize);
        return (result, count, totalPages);
    }
    public async Task<Item> GetItem(Guid id)
    {
        return await _context.Items.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Item> GetFullItem(Guid id)
    {
        IQueryable<Item> query = _context.Items.AsQueryable();

        query = query.Include(x => x.Variant).ThenInclude(x => x.Brand);

        return await query.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> CheckItemIdExist(Guid id)
    {
        return await _context.Items.AnyAsync(x => x.Id == id);
    }

    public async Task<bool> CheckItemWithVariantExist(Guid variantId)
    {
        return await _context.Items.AnyAsync(x => x.VariantId == variantId);
    }

    public async Task CreateItem(Item Item)
    {
        await _context.Items.AddAsync(Item);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateItem(Item Item)
    {
        _context.Items.Update(Item);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteItem(Item Item)
    {
        _context.Items.Remove(Item);
        await _context.SaveChangesAsync();
    }
}