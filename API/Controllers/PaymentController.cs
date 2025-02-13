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
        IUnitOfWork unit,
        IMapper mapper
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
    }
}
