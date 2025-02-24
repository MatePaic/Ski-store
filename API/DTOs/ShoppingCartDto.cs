using Core.Entities;

namespace API.DTOs
{
    public class ShoppingCartDto
    {
        public required string ShoppingCartId { get; set; }
        public List<ShoppingCartItemDto> Items { get; set; } = [];
        public string? ClientSecret { get; set; }
        public AppCoupon? Coupon { get; set; }
    }
}
