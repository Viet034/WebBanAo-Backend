using WebBanAoo.Models.DTO.Request.Cart;
using WebBanAoo.Models.DTO.Response.Cart;
using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models.Mapper;

public class CartMapper : ICartMapper
{
    public Cart_ProductDetail ToCartProductDetail(AddToCartRequest request)
    {
        return new Cart_ProductDetail
        {
            ProductDetailId = request.ProductDetailId,
            Quantity = request.Quantity
        };
    }

    public CartResponse ToCartResponse(Cart cart)
    {
        return new CartResponse
        {
            CartId = cart.CartId,
            CustomerId = cart.CustomerId,
            SessionId = cart.SessionId,
            Items = ToCartItemResponses(cart.Cart_ProductDetails),
            TotalAmount = CalculateTotal(cart.Cart_ProductDetails)
        };
    }

    public CartItemResponse ToCartItemResponse(Cart_ProductDetail cartItem)
    {
        return new CartItemResponse
        {
            CartDetailId = cartItem.Id,
            ProductDetailId = cartItem.ProductDetailId,
            ProductName = cartItem.ProductDetail?.Product?.ProductName ?? "",
            ProductImage = cartItem.ProductDetail?.ProductImages?.FirstOrDefault()?.Url ?? "",
            Price = cartItem.ProductDetail?.Price ?? 0,
            Quantity = cartItem.Quantity,
            Subtotal = (cartItem.ProductDetail?.Price ?? 0) * cartItem.Quantity,
            Status = cartItem.Status,
            //Size = (SizeCode)(cartItem.ProductDetail?.Size ?? default(Size)),
            Color = cartItem.ProductDetail?.Color?.ColorName ?? ""
        };
    }

    public List<CartItemResponse> ToCartItemResponses(ICollection<Cart_ProductDetail> cartItems)
    {
        return cartItems.Select(ToCartItemResponse).ToList();
    }

    private decimal CalculateTotal(ICollection<Cart_ProductDetail> cartItems)
    {
        return cartItems.Sum(item => (item.ProductDetail?.Price ?? 0) * item.Quantity);
    }
} 