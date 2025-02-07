namespace Core.Entities;

public class ShoppingCart : BaseEntity
{
    public required string ShoppingCartId { get; set; }
    public List<ShoppingCartItem> Items { get; set; } = [];
}