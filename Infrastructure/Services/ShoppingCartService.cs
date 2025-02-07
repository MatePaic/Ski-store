using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        public void AddItem(Product product, int quantity, List<ShoppingCartItem> items)
        {
            if (product == null) ArgumentNullException.ThrowIfNull(product);

            if (quantity <= 0) throw new ArgumentException("Quantity should be greater than zero",
                nameof(quantity));

            var existingItem = FindItem(product.Id, items);

            if (existingItem == null)
            {
                items.Add(new ShoppingCartItem
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                existingItem.Quantity += quantity;
            }
        }

        public void RemoveItem(int productId, int quantity, List<ShoppingCartItem> items)
        {
            if (quantity <= 0) throw new ArgumentException("Quantity should be greater than zero",
            nameof(quantity));

            var item = FindItem(productId, items);
            if (item == null) return;

            item.Quantity -= quantity;
            if (item.Quantity <= 0) items.Remove(item);
        }

        private ShoppingCartItem? FindItem(int productId, List<ShoppingCartItem> items)
        {
            return items.FirstOrDefault(item => item.ProductId == productId);
        }
    }
}
