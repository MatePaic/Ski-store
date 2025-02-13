namespace Core.Entities.OrderAggregate
{
    public class Order : BaseEntity
    {
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public required string BuyerEmail { get; set; }
        public required ShippingAddress ShippingAddress { get; set; }
        public required PaymentSummary PaymentSummary { get; set; } 
        public List<OrderItem> OrderItems { get; set; } = [];
        public decimal Subtotal { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal Discount { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public required string PaymentIntentId { get; set; }

        public long GetTotal()
        {
            return (long)(Subtotal + DeliveryFee - Discount);
        }
    }
}
