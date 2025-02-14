namespace API.DTOs
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public required string Name { get; set; }
        public required string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public required string Type { get; set; }
        public required string Brand { get; set; }

    }
}
