using Pos.App.Sales.Models.Variant;

namespace Pos.App.Sales.Models.Item;

public class ItemResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public DateTime Created { get; set; }
}