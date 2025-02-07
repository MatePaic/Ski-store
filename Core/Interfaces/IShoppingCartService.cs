using Core.Entities;

namespace Core.Interfaces
{
    public interface IShoppingCartService
    {
        public void AddItem(Product product, int quantity, List<ShoppingCartItem> items);
        public void RemoveItem(int productId, int quantity, List<ShoppingCartItem> items);
    }
}
