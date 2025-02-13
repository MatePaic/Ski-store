using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Infrastructure.Services
{
    public class PaymentsService(
            IConfiguration configuration
        ) : IPaymentService
    {
        public async Task<PaymentIntent?> CreateOrUpdatePaymentIntent(ShoppingCart shoppingCart)
        {
            StripeConfiguration.ApiKey = configuration["StripeSettings:SecretKey"];

            var service = new PaymentIntentService();

            var intent = new PaymentIntent();
            var subtotal = shoppingCart.Items.Sum(x => x.Quantity * x.Product.Price);
            var deliveryFee = subtotal > 10000 ? 0 : 500;

            if (string.IsNullOrEmpty(shoppingCart.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(subtotal + deliveryFee),
                    Currency = "usd",
                    PaymentMethodTypes = ["card"]
                };
                intent = await service.CreateAsync(options);

                shoppingCart.PaymentIntentId ??= intent.Id;
                shoppingCart.ClientSecret ??= intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)(subtotal + deliveryFee)
                };
                await service.UpdateAsync(shoppingCart.PaymentIntentId, options);
            }

            return intent;
        }
    }
}
