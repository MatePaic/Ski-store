using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using System.Linq.Expressions;

namespace API.Extensions
{
    public static class ShoppingCartExtensions
    {
        public static async Task<ShoppingCart?> GetShoppingCartWithItems(
            this IGenericRepository<ShoppingCart> shoppingCartRepository, string? shoppingCartId)
        {
            var specification = new ShoppingCartIncludeSpecification();
            Expression<Func<ShoppingCart, bool>> predicate = p => p.ShoppingCartId == shoppingCartId;

            var shoppingCart = await shoppingCartRepository.GetFirstOrDefaultWithSpecAsync(specification, predicate);

            if (shoppingCart == null) return null;

            return shoppingCart;
        }
    }
}