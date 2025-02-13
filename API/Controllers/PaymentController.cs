using API.DTOs;
using API.Extensions;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PaymentsController(
        IPaymentService paymentService, 
        IGenericRepository<ShoppingCart> shoppingCartRepository,
        IMapper mapper
    ) : BaseApiController
    {
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ShoppingCartDto>> CreateOrUpdatePaymentIntent()
        {
            var shoppingCart = await shoppingCartRepository
                .GetShoppingCartWithItems(Request.Cookies["shoppingCartId"]);
            if (shoppingCart == null) return BadRequest("Problem with the basket");

            var intent = await paymentService.CreateOrUpdatePaymentIntent(shoppingCart);
            if (intent == null) return BadRequest("Problem creating payment intent");

            if (shoppingCartRepository.HasChanges())
            {
                var result = await shoppingCartRepository.SaveChangesAsync();
                if (!result) return BadRequest("Problem updating shoppingCart with intent");
            }

            return mapper.Map<ShoppingCartDto>(shoppingCart);
        }
    }
}
