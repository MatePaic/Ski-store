using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Controllers
{
    public class ShoppingCartController(
        IGenericRepository<ShoppingCart> shoppingCartRepository, 
        IShoppingCartService shoppingCartService,
        IGenericRepository<Product> productRepository,
        IMapper mapper) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<ShoppingCartDto>> GetShoppingCart()
        {
            var shoppingCart = await RetrieveShoppingCart();

            if (shoppingCart == null) return NoContent();

            return mapper.Map<ShoppingCartDto>(shoppingCart);
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCartDto>> AddItemToShoppingCart(int productId, int quantity)
        {
            var shoppingCart = await RetrieveShoppingCart();

            shoppingCart ??= CreateShoppingCart();

            var product = await productRepository.GetByIdAsync(productId);
            if (product == null) return BadRequest("Problem adding item to shoppingCart");

            shoppingCartService.AddItem(product, quantity, shoppingCart.Items);

            var result = await shoppingCartRepository.SaveChangesAsync();

            if (result) return CreatedAtAction(nameof(GetShoppingCart), 
                mapper.Map<ShoppingCartDto>(shoppingCart));

            return BadRequest("Problem updating shoppingCart");
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveShoppingCartItem(int productId, int quantity)
        {
            var shoppingCart = await RetrieveShoppingCart();
            if (shoppingCart == null)
            {
                return BadRequest("Unable to retrieve shoppingCart");
            }

            shoppingCartService.RemoveItem(productId, quantity, shoppingCart.Items);

            var result = await shoppingCartRepository.SaveChangesAsync();
            if (result) return Ok();

            return BadRequest("Product does not exist in the shoppingCart");
        }

        private async Task<ShoppingCart?> RetrieveShoppingCart()
        {
            var specification = new ShoppingCartIncludeSpecification();
            Expression<Func<ShoppingCart, bool>> predicate = p => p.ShoppingCartId == Request.Cookies["shoppingCartId"];

            var shoppingCart = await shoppingCartRepository.GetFirstOrDefaultWithSpecAsync(specification, predicate);

            return shoppingCart;
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
            shoppingCartRepository.Add(shoppingCart);

            return shoppingCart;
        }
    }
}
