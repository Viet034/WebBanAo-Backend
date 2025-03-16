namespace WebBanAoo.Models.DTO.Request.Cart;

public class CartCheckoutRequest
{
    public int CustomerId { get; set; }
    
    public int? VoucherId { get; set; }
    public string? Note { get; set; }
}
