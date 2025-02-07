namespace API.DTOs
{
    public class ShoppingCartDto
    {
        public required string ShoppingCartId { get; set; }
        public List<ShoppingCartItemDto> Items { get; set; } = [];
    }
}
