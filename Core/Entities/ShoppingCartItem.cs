using System.ComponentModel.DataAnnotations.Schema;
namespace Core.Entities;

[Table("ShoppingCartItems")]
public class ShoppingCartItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }

    // navigation properties
    public int ProductId { get; set; }
    public required Product Product { get; set; }

    public int ShoppingCartId { get; set; }
    public ShoppingCart ShoppingCart { get; set; } = null!;
}
