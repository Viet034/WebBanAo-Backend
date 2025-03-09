using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebBanAoo.Data;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Cart;
using WebBanAoo.Models.DTO.Response.Cart;
using WebBanAoo.Models.Mapper;

namespace WebBanAoo.Service.impl;

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
        var cart1 = await _context.Carts
                        .Include(c => c.Cart_ProductDetails)
                        .FirstOrDefaultAsync(x => x.CustomerId == customerId);
        if(cart1 != null)
        {
            
            var cartProductDetail1 = await _context.Cart_ProductDetails.Where(x => x.CartId == cart1.CartId).ToListAsync();
            cart1.Cart_ProductDetails = cartProductDetail1;
            
            var listIDProductDetail = cartProductDetail1.Select(x => x.ProductDetailId).ToHashSet();
            var count = cartProductDetail1.Count();
            var prodcutDetails = await _context.ProductDetail.Where(x => listIDProductDetail.Contains(x.Id)).ToListAsync();
            // lấy danh sách sản phẩm 

            CartResponse result = new()
            {
                CartId = cart1.CartId,
                CustomerId = customerId,
                TotalAmount = count,

            };
        }


        return _mapper.ToCartResponse(cart1);
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

        var cart1 = await _context.Carts.Where(c => c.CustomerId == customerId)
                                        .FirstOrDefaultAsync();
        if (cart1 == null) return 0;

        var cartProdcut = await _context.Cart_ProductDetails.Where(x => x.CartId == cart1.CartId).ToListAsync();
        cart1.Cart_ProductDetails = cartProdcut;
        
        
        if (cart1 == null)
            return 0;

        return cart1.Cart_ProductDetails.Sum(item => 
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