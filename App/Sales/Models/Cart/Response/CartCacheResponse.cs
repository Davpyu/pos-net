namespace Pos.App.Sales.Models.Cart;

public class CartCacheResponse
{
    public string Key { get; set; }
    public List<CartRequest> Carts { get; set; }
}