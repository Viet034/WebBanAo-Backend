using Microsoft.AspNetCore.Mvc;
using WebBanAoo.Service;
using WebBanAoo.Models;
using System.Net;
using WebBanAoo.Models.DTO;
using static WebBanAoo.Models.Status.Status;
using Microsoft.AspNetCore.Authorization;
using WebBanAoo.Models.DTO.Request.Cart;

namespace WebBanAoo.Controllers;

[ApiController]
[Route("/api/[controller]")]
//[Authorize] // Yêu cầu xác thực để truy cập giỏ hàng
public class CartController : ControllerBase
{
    private readonly ICartService _service;

    public CartController(ICartService service)
    {
        _service = service;
    }

    [HttpPost("add-to-cart")]
    [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
    {
        try
        {
            var response = await _service.AddToCartAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpGet("get-cart/{customerId}")]
    [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetCart(int customerId)
    {
        try
        {
            var response = await _service.GetCartByCustomerIdAsync(customerId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpPut("update-quantity")]
    [ProducesResponseType(typeof(Cart_ProductDetail), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateQuantity([FromBody] UpdateCartQuantityRequest request)
    {
        try
        {
            var response = await _service.UpdateCartItemQuantityAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpDelete("remove-item/{cartDetailId}")]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> RemoveFromCart(int cartDetailId)
    {
        try
        {
            var response = await _service.RemoveFromCartAsync(cartDetailId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpDelete("clear-cart/{customerId}")]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> ClearCart(int customerId)
    {
        try
        {
            var response = await _service.ClearCartAsync(customerId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpGet("get-cart-total/{customerId}")]
    [ProducesResponseType(typeof(decimal), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetCartTotal(int customerId)
    {
        try
        {
            var response = await _service.GetCartTotalAsync(customerId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }
} 