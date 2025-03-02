using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Cart;
using WebBanAoo.Models.DTO.Response.Cart;

namespace WebBanAoo.Service;

public interface ICartService
{
    Task<CartResponse> AddToCartAsync(AddToCartRequest request);
    Task<CartResponse> GetCartByCustomerIdAsync(int customerId);
    Task<CartItemResponse> UpdateCartItemQuantityAsync(UpdateCartQuantityRequest request);
    Task<bool> RemoveFromCartAsync(int cartDetailId);
    Task<bool> ClearCartAsync(int customerId);
    Task<decimal> GetCartTotalAsync(int customerId);
    Task<CartResponse> GetOrCreateCartAsync(int customerId);
} 