using Core.Entities;

namespace Core.Specifications
{
    public class ShoppingCartIncludeSpecification : BaseSpecification<ShoppingCart>
    {
        public ShoppingCartIncludeSpecification()
        {
            AddInclude(x => x.Items);
            AddInclude("Items.Product");
        }
    }
}
