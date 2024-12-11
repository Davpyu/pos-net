using Pos.App.Sales.Models.Variant;

namespace Pos.App.Sales.Models.Brand;

public class BrandResponseSingle
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public List<VariantResponse> Variants { get; set; }
}