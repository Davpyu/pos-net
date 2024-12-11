namespace Pos.Entities;

public class Item : BaseEntity
{
    public Variant Variant { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}