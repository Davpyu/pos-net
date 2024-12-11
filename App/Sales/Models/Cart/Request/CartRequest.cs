namespace Pos.App.Sales.Models.Cart;

public class CartRequest
{
    public Guid ItemId { get; set; }
    public int Quantity { get; set; }
}