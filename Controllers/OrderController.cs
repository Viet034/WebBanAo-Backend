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

namespace WebBanAoo.Controllers;

[ApiController]
[Route("/api/[controller]")]

public class OrderController : ControllerBase
{
    private readonly IOrderService _service;

    public OrderController(IOrderService service)
    {
        _service = service;
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
    [Authorize(Roles = "Admin,Employee")]
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
    [Authorize(Roles = "Admin,Employee")]
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
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> CheckoutFromCart([FromBody] CartCheckoutRequest request)
    {
        try
        {
            var response = await _service.CheckoutFromCartAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
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

}
