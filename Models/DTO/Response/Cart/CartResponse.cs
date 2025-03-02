using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models.DTO.Response.Cart;

public class CartResponse
{
    public int CartId { get; set; }
    public int CustomerId { get; set; }
    public string SessionId { get; set; }
    public List<CartItemResponse> Items { get; set; } = new List<CartItemResponse>();
    public decimal TotalAmount { get; set; }
}

public class CartItemResponse
{
    public int CartDetailId { get; set; }
    public int ProductDetailId { get; set; }
    public string ProductName { get; set; }
    public string ProductImage { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal Subtotal { get; set; }
    public CartProductDetailStatus Status { get; set; }
    public SizeCode Size { get; set; }
    public string Color { get; set; }
} 