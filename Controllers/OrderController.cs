using Microsoft.AspNetCore.Mvc;
using WebBanAoo.Service;
using WebBanAoo.Models.DTO.Request.Order;
using WebBanAoo.Models;
using System.Net;
using static WebBanAoo.Models.Status.Status;
using Microsoft.AspNetCore.Authorization;
using WebBanAoo.Models.DTO.Request.Cart;
using WebBanAoo.Models.DTO.Response;
using WebBanAoo.Service.impl;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using WebBanAoo.Data;

namespace WebBanAoo.Controllers;

[ApiController]
[Route("/api/[controller]")]

public class OrderController : ControllerBase
{
    private readonly IOrderService _service;
    private readonly ApplicationDbContext _context;

    public OrderController(IOrderService service, ApplicationDbContext context)
    {
        _service = service;
        _context = context;
    }

    [HttpPost("AddOrder")]
    [ProducesResponseType(typeof(IEnumerable<Order>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<IActionResult> AddOrder([FromBody] OrderCreate create)
    {
        try
        {
            var response = await _service.CreateOrderAsync(create);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    
    [HttpGet("GetAll")]
    [ProducesResponseType(typeof(IEnumerable<Order>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<ActionResult<IEnumerable<Order>>> GetAllOrder()
    {
        try
        {
            var response = await _service.GetAllOrderAsync();
            return Ok(response);
        }catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpGet("FindByName/{name}")]
    [ProducesResponseType(typeof(IEnumerable<Order>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<IActionResult> FindByName(string name)
    {
        try
        {
            var response = await _service.SearchOrderByKeyAsync(name);
            return Ok(response);
        }
        catch (Exception ex) 
        { 
            return BadRequest(ex.ToString());
        }
    }

    [HttpGet("findId/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Order>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [Authorize(Roles = "Admin,Employee,Customer")]
    public async Task<IActionResult> FindById(int id)
    {
        try
        {
            var response = await _service.FindOrderByIdAsync(id);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("customerId/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Order>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [AllowAnonymous]  
    public async Task<IActionResult> FindOrderByCustomerId(int id)
    {
        try
        {
            var response = await _service.GetOrderByCustomerIdAsync(id);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpPut("Update/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Order>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<IActionResult> UpdateOrder([FromBody] OrderUpdate update, int id)
    {
        try
        {
            var response = await _service.UpdateOrderAsync(id, update);
            return Ok(response);
        }catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }




    [HttpPut("ChangeSstatus/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Order>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    //[Authorize(Roles = "Admin,Employee")]
    public async Task<IActionResult> SoftDeleteOrder(int id, OrderStatus newStatus)
    {
        try
        {
            var response = await _service.SoftDeleteOrderAsync(id, newStatus);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }



    [HttpDelete("DeletePermanent/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Order>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> HardDeleteOrder(int id)
    {
        try
        {
            var response = await _service.HardDeleteOrderAsync(id);
            return Ok(response);
        }catch(Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpPost("checkout")]
    [ProducesResponseType(typeof(OrderResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    //[Authorize(Roles = "Customer")]
    public async Task<IActionResult> CheckoutFromCart([FromBody] CartCheckoutRequest request)
    {
        try
        {
            var response = await _service.CheckoutFromCartAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("assign-order/{orderId}")]
    [Authorize(Roles = "Admin,Employee")]  // Chỉ employee mới được assign
    public async Task<IActionResult> AssignOrderToCurrentEmployee(int orderId)
    {
        try
        {
            // Lấy employeeId từ token của người đang đăng nhập
            //var employeeId = User.FindFirst("EmployeeId")?.Value;
            //var employeeIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //var userTypeClaim = User.FindFirst("UserType")?.Value;
            //if (string.IsNullOrEmpty(userTypeClaim))
            //    return Unauthorized("Employee information not found");

            //var response = await _service.AssignEmployeeToOrderAsync(orderId, int.Parse(userTypeClaim));
            //return Ok(response);
            var employeeIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(employeeIdClaim))
                return Unauthorized("Employee ID not found");

            if (!int.TryParse(employeeIdClaim, out int employeeId))
                return BadRequest("Invalid Employee ID format");

            var response = await _service.AssignEmployeeToOrderAsync(orderId, employeeId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("pending-orders")]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<IActionResult> GetPendingOrders()
    {
        try
        {
            var orders = await _service.GetPendingOrdersAsync();
            return Ok(orders);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("my-orders")]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<IActionResult> GetMyOrders()
    {
        try
        {
            //var employeeId = int.Parse(User.FindFirst("EmployeeId").Value);
            //var orders = await _service.GetOrdersByEmployeeIdAsync(employeeId);
            //return Ok(orders);
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(idClaim))
                return Unauthorized("Employee ID not found in token.");

            if (!int.TryParse(idClaim, out int employeeId))
                return BadRequest("Invalid Employee ID format.");

            var orders = await _service.GetOrdersByEmployeeIdAsync(employeeId);
            return Ok(orders);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpGet("calculate-with-voucher")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> CalculateWithVoucher(int customerId, int voucherId)
    {
        try
        {
            // Lấy giỏ hàng hiện tại
            var cart = await _context.Carts
                .Include(c => c.Cart_ProductDetails)
                    .ThenInclude(cpd => cpd.ProductDetail)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            if (cart == null)
                return BadRequest("Cart not found");

            // Tính tổng tiền giỏ hàng
            decimal totalAmount = cart.Cart_ProductDetails.Sum(item => 
                item.ProductDetail.Price * item.Quantity);

            // Lấy thông tin voucher
            var voucher = await _context.Vouchers
                .FirstOrDefaultAsync(v => v.Id == voucherId);
            
            if (voucher == null)
                return BadRequest("Voucher not found");

            // Kiểm tra các điều kiện voucher
            if (voucher.Status != VoucherStatus.Active)
                return BadRequest("Voucher is not active");
            
            if (voucher.StartDate > DateTime.Now || voucher.EndDate < DateTime.Now)
                return BadRequest("Voucher is expired or not yet active");
            
            if (totalAmount < voucher.MinimumOrderValue)
                return BadRequest($"Order total must be at least {voucher.MinimumOrderValue}");

            if (voucher.Quantity <= 0)
                return BadRequest("Voucher is out of stock");

            // Kiểm tra xem khách hàng đã sử dụng voucher này chưa
            var customerVoucher = await _context.Customer_Vouchers
                .FirstOrDefaultAsync(cv => 
                    cv.CustomerId == customerId && 
                    cv.VoucherId == voucherId);

            if (customerVoucher?.Status == CustomerVoucherStatus.Used)
                return BadRequest("This voucher has already been used by the customer");

            // Tính toán giảm giá
            decimal discount = Math.Min(
                totalAmount * voucher.DiscountValue / 100,
                voucher.MaxDiscount
            );

            // Trả về kết quả
            return Ok(new {
                OriginalAmount = totalAmount,
                DiscountAmount = discount,
                FinalAmount = totalAmount - discount
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}
