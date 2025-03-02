using System.ComponentModel.DataAnnotations;

namespace WebBanAoo.Models.DTO.Request.Cart;

public class AddToCartRequest
{
    [Required]
    public int ProductDetailId { get; set; }
    
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
    public int Quantity { get; set; }
    
    [Required]
    public int CustomerId { get; set; }
}

public class UpdateCartQuantityRequest
{
    [Required]
    public int CartDetailId { get; set; }
    
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
    public int Quantity { get; set; }
}

public class CartItemRequest
{
    public int CartId { get; set; }
    public int ProductDetailId { get; set; }
    public int Quantity { get; set; }
} 