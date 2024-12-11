using Pos.App.Sales.Models.Brand;

namespace Pos.App.Sales.Models.Variant;

public class VariantResponseSingle
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public BrandResponse Brand { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}