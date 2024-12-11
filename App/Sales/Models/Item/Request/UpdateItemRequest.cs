using System.ComponentModel.DataAnnotations;

namespace Pos.App.Sales.Models.Item;

public class UpdateItemRequest
{
    [Required]
    public Guid VariantId { get; set; }
    [Required]
    public decimal Price { get; set; }
    [Required]
    public int Quantity { get; set; }
}