using System.ComponentModel.DataAnnotations;

namespace Pos.App.Sales.Models.Variant;

public class CreateVariantRequest
{
    [Required]
    public string Name { get; set; }
    [Required]
    public Guid BrandId { get; set; }
}