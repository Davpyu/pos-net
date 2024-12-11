using Pos.App.Sales.Models.Brand;
using Pos.App.Sales.Models.Variant;

namespace Pos.App.Sales.Models.Item;

public class ItemResponseSingle
{
    public Guid Id { get; set; }
    public BrandResponse Brand { get; set; }
    public VariantResponse Variant { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}