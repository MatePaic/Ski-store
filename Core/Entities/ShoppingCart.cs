namespace Core.Entities;

public class ShoppingCart : BaseEntity
{
    public required string ShoppingCartId { get; set; }
    public List<ShoppingCartItem> Items { get; set; } = [];
    public string? ClientSecret { get; set; }
    public string? PaymentIntentId { get; set; }
    public AppCoupon? Coupon { get; set; }
}