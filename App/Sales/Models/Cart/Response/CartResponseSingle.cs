namespace Pos.App.Sales.Models.Cart;

public class CartResponseSingle
{
    public Guid ItemId { get; set; }
    public string Brand { get; set; }
    public string Variant { get; set; }
    public int Quantity { get; set; }
}