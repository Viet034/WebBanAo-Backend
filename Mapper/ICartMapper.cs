using WebBanAoo.Models.DTO.Request.Cart;
using WebBanAoo.Models.DTO.Response.Cart;

namespace WebBanAoo.Models.Mapper;

public interface ICartMapper
{
    Cart_ProductDetail ToCartProductDetail(AddToCartRequest request);
    CartResponse ToCartResponse(Cart cart);
    CartItemResponse ToCartItemResponse(Cart_ProductDetail cartItem);
    List<CartItemResponse> ToCartItemResponses(ICollection<Cart_ProductDetail> cartItems);
} 