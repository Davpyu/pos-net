namespace Pos.App.Sales.Models.Cart;

public class CartResponse
{
    public string Key { get; set; }
    public List<CartResponseSingle> Carts { get; set; }
}