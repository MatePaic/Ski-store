using API.DTOs;
using API.Extensions;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ShoppingCartController(
        IShoppingCartService shoppingCartService,
        IUnitOfWork unit,
        IMapper mapper,
        IDiscountService discountService,
        IPaymentService paymentService
    ) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<ShoppingCartDto>> GetShoppingCart()
        {
            var shoppingCart = await unit.Repository<ShoppingCart>()
                .GetShoppingCartWithItems(Request.Cookies["shoppingCartId"]);

            if (shoppingCart == null) return NoContent();

            return mapper.Map<ShoppingCartDto>(shoppingCart);
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCartDto>> AddItemToShoppingCart(int productId, int quantity)
        {
            var shoppingCart = await unit.Repository<ShoppingCart>()
                .GetShoppingCartWithItems(Request.Cookies["shoppingCartId"]);

            shoppingCart ??= CreateShoppingCart();

            var product = await unit.Repository<Product>().GetByIdAsync(productId);
            if (product == null) return BadRequest("Problem adding item to shoppingCart");

            shoppingCartService.AddItem(product, quantity, shoppingCart.Items);

            var result = await unit.Complete();

            if (result) return CreatedAtAction(nameof(GetShoppingCart),
                mapper.Map<ShoppingCartDto>(shoppingCart));

            return BadRequest("Problem updating shoppingCart");
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveShoppingCartItem(int productId, int quantity)
        {
            var shoppingCart = await unit.Repository<ShoppingCart>()
                .GetShoppingCartWithItems(Request.Cookies["shoppingCartId"]);

            if (shoppingCart == null)
            {
                return BadRequest("Unable to retrieve shoppingCart");
            }

            shoppingCartService.RemoveItem(productId, quantity, shoppingCart.Items);

            var result = await unit.Complete();
            if (result) return Ok();

            return BadRequest("Product does not exist in the shoppingCart");
        }

        [HttpPost("{code}")]
        public async Task<ActionResult<ShoppingCartDto>> AddCouponCode(string code)
        {
            var shoppingCart = await unit.Repository<ShoppingCart>()
                .GetShoppingCartWithItems(Request.Cookies["shoppingCartId"]);
            if (shoppingCart == null || string.IsNullOrEmpty(shoppingCart.ClientSecret))
                return BadRequest("Unable to apply voucher");

            var coupon = await discountService.GetCouponFromPromoCode(code);
            if (coupon == null) return BadRequest("Invalid coupon");

            shoppingCart.Coupon = coupon;

            var intent = await paymentService.CreateOrUpdatePaymentIntent(shoppingCart);
            if (intent == null) return BadRequest("Problem applying coupon to basket");

            var result = await unit.Complete();

            if (result) return CreatedAtAction(nameof(GetShoppingCart), mapper.Map<ShoppingCartDto>(shoppingCart));

            return BadRequest("Problem updating shopping cart");
        }

        [HttpDelete("remove-coupon")]
        public async Task<ActionResult> RemoveCouponFromBasket()
        {
            var shoppingCart = await unit.Repository<ShoppingCart>()
               .GetShoppingCartWithItems(Request.Cookies["shoppingCartId"]);
            if (shoppingCart == null || string.IsNullOrEmpty(shoppingCart.ClientSecret))
                return BadRequest("Unable to update basket with coupon");

            var intent = await paymentService.CreateOrUpdatePaymentIntent(shoppingCart, true);
            if (intent == null) return BadRequest("Problem removing coupon from basket");

            shoppingCart.Coupon = null;

            var result = await unit.Complete();

            if (result) return Ok();

            return BadRequest("Problem updating basket");
        }

        private ShoppingCart CreateShoppingCart()
        {
            var shoppingCartId = Guid.NewGuid().ToString();
            var cookieOptions = new CookieOptions
            {
                IsEssential = true,
                Expires = DateTime.UtcNow.AddDays(30)
            };

            Response.Cookies.Append("shoppingCartId", shoppingCartId, cookieOptions);

            var shoppingCart = new ShoppingCart { ShoppingCartId = shoppingCartId };
            unit.Repository<ShoppingCart>().Add(shoppingCart);

            return shoppingCart;
        }
    }
}