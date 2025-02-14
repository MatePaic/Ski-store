using API.DTOs;
using API.Extensions;
using AutoMapper;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Linq.Expressions;

namespace API.Controllers
{
    public class PaymentsController(
        IPaymentService paymentService, 
        IUnitOfWork unit,
        IMapper mapper,
        IConfiguration configuration,
        ILogger<PaymentsController> logger
    ) : BaseApiController
    {
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ShoppingCartDto>> CreateOrUpdatePaymentIntent()
        {
            var shoppingCart = await unit.Repository<ShoppingCart>()
                .GetShoppingCartWithItems(Request.Cookies["shoppingCartId"]);
            if (shoppingCart == null) return BadRequest("Problem with the shoppingCart");

            var intent = await paymentService.CreateOrUpdatePaymentIntent(shoppingCart);
            if (intent == null) return BadRequest("Problem creating payment intent");

            if (unit.Repository<ShoppingCart>().HasChanges())
            {
                var result = await unit.Complete();
                if (!result) return BadRequest("Problem updating shoppingCart with intent");
            }

            return mapper.Map<ShoppingCartDto>(shoppingCart);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = ConstructStripeEvent(json);

                if (stripeEvent.Data.Object is not PaymentIntent intent)
                {
                    return BadRequest("Invalid event data");
                }

                if (intent.Status == "succeeded") await HandlePaymentIntentSucceeded(intent);
                else await HandlePaymentIntentFailed(intent);

                return Ok();
            }
            catch (StripeException exception)
            {
                logger.LogError(exception, "Stripe webhook error");
                return StatusCode(StatusCodes.Status500InternalServerError, "Webhook error");
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "An unexpected error has occured");
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        private async Task HandlePaymentIntentSucceeded(PaymentIntent intent)
        {
            Expression<Func<Order, bool>> predicate = p => p.PaymentIntentId == intent.Id;
            var orderSpecification = new OrderSpecification();

            var order = await unit.Repository<Order>()
                .GetFirstOrDefaultWithSpecAsync(orderSpecification, predicate)
                ?? throw new Exception("Order not found");

            if(order.GetTotal() != intent.Amount)
            {
                order.OrderStatus = OrderStatus.PaymentMismatch;
            } 
            else
            {
                order.OrderStatus = OrderStatus.PaymentReceived;
            }

            var shoppingCart = await unit.Repository<ShoppingCart>().GetFirstOrDefaultAsync(x => 
                x.PaymentIntentId == intent.Id);

            if (shoppingCart != null) unit.Repository<ShoppingCart>().Remove(shoppingCart);

            await unit.Complete();
        }

        private async Task HandlePaymentIntentFailed(PaymentIntent intent)
        {
            var orderSpecification = new OrderSpecification(User.GetEmail());
            Expression<Func<Order, bool>> predicate = p => p.PaymentIntentId == intent.Id;

            var order = await unit.Repository<Order>()
                .GetFirstOrDefaultWithSpecAsync(orderSpecification, predicate) 
                ?? throw new Exception("Order not found");

            foreach (var item in order.OrderItems)
            {
                var productItem = await unit.Repository<Core.Entities.Product>()
                    .GetByIdAsync(item.ItemOrdered.ProductId)
                    ?? throw new Exception("Problem updating order stock");

                productItem.QuantityInStock += item.Quantity;
            }

            order.OrderStatus = OrderStatus.PaymentFailed;

            await unit.Complete();
        }

        private Event ConstructStripeEvent(string json)
        {
            try
            {
                return EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"],
                    configuration["StripeSettings:WhSecret"]);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to constuct stripe event");
                throw new StripeException("Invalid signature");
            }
        }
    }
}
