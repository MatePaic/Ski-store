using Core.Entities;
using Stripe;

namespace Core.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentIntent?> CreateOrUpdatePaymentIntent(ShoppingCart shoppingCart);
    }
}
