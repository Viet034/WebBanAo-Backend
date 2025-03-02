using Microsoft.EntityFrameworkCore;
using WebBanAoo.Data;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Cart;
using WebBanAoo.Models.DTO.Response.Cart;
using WebBanAoo.Models.Mapper;

namespace WebBanAoo.Service;

public class CartService : ICartService
{
    private readonly ApplicationDbContext _context;
    private readonly ICartMapper _mapper;

    public CartService(ApplicationDbContext context, ICartMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CartResponse> AddToCartAsync(AddToCartRequest request)
    {
        var cart = await GetOrCreateCartAsync(request.CustomerId);
        
        var existingItem = await _context.Cart_ProductDetails
            .FirstOrDefaultAsync(cp => cp.CartId == cart.CartId && 
                                     cp.ProductDetailId == request.ProductDetailId);

        if (existingItem != null)
        {
            existingItem.Quantity += request.Quantity;
        }
        else
        {
            var newItem = new Cart_ProductDetail
            {
                CartId = cart.CartId,
                ProductDetailId = request.ProductDetailId,
                Quantity = request.Quantity
            };
            await _context.Cart_ProductDetails.AddAsync(newItem);
        }

        await _context.SaveChangesAsync();
        return await GetCartByCustomerIdAsync(request.CustomerId);
    }

    public async Task<CartResponse> GetCartByCustomerIdAsync(int customerId)
    {
        var cart = await _context.Carts
            .Include(c => c.Cart_ProductDetails)
                .ThenInclude(cp => cp.ProductDetail)
                    .ThenInclude(pd => pd.Product)
            .Include(c => c.Cart_ProductDetails)
                .ThenInclude(cp => cp.ProductDetail)
                    .ThenInclude(pd => pd.Size)
            .Include(c => c.Cart_ProductDetails)
                .ThenInclude(cp => cp.ProductDetail)
                    .ThenInclude(pd => pd.Color)
            .Include(c => c.Cart_ProductDetails)
                .ThenInclude(cp => cp.ProductDetail)
                    .ThenInclude(pd => pd.ProductImages)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);

        if (cart == null)
            throw new Exception("Không tìm thấy giỏ hàng");

        return _mapper.ToCartResponse(cart);
    }

    public async Task<CartItemResponse> UpdateCartItemQuantityAsync(UpdateCartQuantityRequest request)
    {
        var cartItem = await _context.Cart_ProductDetails
            .Include(cp => cp.ProductDetail)
                .ThenInclude(pd => pd.Product)
            .Include(cp => cp.ProductDetail)
                .ThenInclude(pd => pd.Size)
            .Include(cp => cp.ProductDetail)
                .ThenInclude(pd => pd.Color)
            .Include(cp => cp.ProductDetail)
                .ThenInclude(pd => pd.ProductImages)
            .FirstOrDefaultAsync(cp => cp.Id == request.CartDetailId);

        if (cartItem == null)
            throw new Exception("Không tìm thấy sản phẩm trong giỏ hàng");

        cartItem.Quantity = request.Quantity;
        await _context.SaveChangesAsync();

        return _mapper.ToCartItemResponse(cartItem);
    }

    public async Task<bool> RemoveFromCartAsync(int cartDetailId)
    {
        var cartItem = await _context.Cart_ProductDetails
            .FirstOrDefaultAsync(cp => cp.Id == cartDetailId);

        if (cartItem == null)
            return false;

        _context.Cart_ProductDetails.Remove(cartItem);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ClearCartAsync(int customerId)
    {
        var cart = await _context.Carts
            .Include(c => c.Cart_ProductDetails)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);

        if (cart == null)
            return false;

        _context.Cart_ProductDetails.RemoveRange(cart.Cart_ProductDetails);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<decimal> GetCartTotalAsync(int customerId)
    {
        var cart = await _context.Carts
            .Include(c => c.Cart_ProductDetails)
                .ThenInclude(cp => cp.ProductDetail)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);

        if (cart == null)
            return 0;

        return cart.Cart_ProductDetails.Sum(item => 
            (item.ProductDetail?.Price ?? 0) * item.Quantity);
    }

    public async Task<CartResponse> GetOrCreateCartAsync(int customerId)
    {
        var cart = await _context.Carts
            .Include(c => c.Cart_ProductDetails)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);

        if (cart == null)
        {
            cart = new Cart
            {
                CustomerId = customerId,
                SessionId = Guid.NewGuid().ToString()
            };
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        return _mapper.ToCartResponse(cart);
    }
} 