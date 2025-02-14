using API.DTOs;
using API.Extensions;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Controllers
{
    [Authorize]
    public class OrdersController(IUnitOfWork unit) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrders()
        {
            var orderSpecification = new OrderSpecification(User.GetEmail());

            var orders = await unit.Repository<Order>().GetWithSpecificationAsync(orderSpecification);

            var ordersToReturn = orders.Select(o => o.ToDto()).ToList();

            return Ok(ordersToReturn);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDto>> GetOrderDetails(int id)
        {
            var orderDetailSpecification = new OrderSpecification(User.GetEmail(), id);

            var order = await unit.Repository<Order>().GetFirstOrDefaultWithSpecAsync(orderDetailSpecification);

            if (order == null) return NotFound();

            return Ok(order.ToDto());
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(CreateOrderDto orderDto)
        {
            var email = User.GetEmail();

            var shoppingCart = await unit.Repository<ShoppingCart>()
                .GetShoppingCartWithItems(Request.Cookies["shoppingCartId"]);

            if (shoppingCart == null || shoppingCart.Items.Count == 0)
                return BadRequest("Shopping Cart is empty or not found");

            if (shoppingCart.PaymentIntentId == null) return BadRequest("No payment intent for this order");

            var items = CreateOrderItems(shoppingCart.Items);

            if (items == null) return BadRequest("Some items out of stock");

            var subtotal = items.Sum(x => x.Price * x.Quantity);
            var deliveryFee = CalculateDeliveryFee(subtotal);

            Expression<Func<Order, bool>> predicate = p => p.PaymentIntentId == shoppingCart.PaymentIntentId;
            var orderSpecification = new OrderSpecification(User.GetEmail());

            var order = await unit.Repository<Order>().GetFirstOrDefaultWithSpecAsync(orderSpecification, predicate);

            if (order == null)
            {
                order = new Order
                {
                    OrderItems = items,
                    BuyerEmail = email,
                    ShippingAddress = orderDto.ShippingAddress,
                    DeliveryFee = deliveryFee,
                    Subtotal = subtotal,
                    PaymentSummary = orderDto.PaymentSummary,
                    PaymentIntentId = shoppingCart.PaymentIntentId
                };
                unit.Repository<Order>().Add(order);
            }
            else
            {
                order.OrderItems = items;
            }

            if (await unit.Complete())
            {
                return CreatedAtAction(nameof(GetOrderDetails), new { id = order.Id }, order.ToDto());
            }

            return BadRequest("Problem creating order");
        }

        private List<OrderItem>? CreateOrderItems(List<ShoppingCartItem> items)
        {
            var orderItems = new List<OrderItem>();

            foreach (var item in items)
            {
                if (item.Product.QuantityInStock < item.Quantity)
                    return null;

                var orderItem = new OrderItem
                {
                    ItemOrdered = new ProductItemOrdered
                    {
                        ProductId = item.ProductId,
                        PictureUrl = item.Product.PictureUrl,
                        Name = item.Product.Name,
                    },
                    Price = item.Product.Price,
                    Quantity = item.Quantity,
                    Brand = item.Product.Brand,
                    Type = item.Product.Type
                };
                orderItems.Add(orderItem);

                item.Product.QuantityInStock -= item.Quantity;
            }

            return orderItems;
        }

        private long CalculateDeliveryFee(decimal subtotal)
        {
            return subtotal > 10000 ? 0 : 500;
        }
    }
}
