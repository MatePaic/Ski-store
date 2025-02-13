using Core.Entities.OrderAggregate;

namespace API.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public required string BuyerEmail { get; set; }
        public required ShippingAddress ShippingAddress { get; set; }
        public DateTime OrderDate { get; set; }
        public required List<OrderItemDto> OrderItems { get; set; }
        public decimal Subtotal { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public required string OrderStatus { get; set; }
        public required PaymentSummary PaymentSummary { get; set; }
        public required string PaymentIntentId { get; set; }
    }
}
