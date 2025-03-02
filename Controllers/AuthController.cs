using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WebBanAoo.Models.DTO.Request.Customer;
using WebBanAoo.Models.DTO.Request.Employee;


namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]

public class AuthController : ControllerBase
{
    private readonly IAuthService _service;
    private readonly ILogger<AuthController> _logger;
    
public AuthController(IAuthService service, ILogger<AuthController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var response = await _service.LoginAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return Unauthorized(ex.ToString());
        }
    }

    [HttpPost("register/customer")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterCustomer([FromBody] CustomerRegisterRequest request)
    {
        try
        {
            var response = await _service.RegisterCustomerAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpPost("register/employee")]
    //[Authorize(Roles = "Admin")] // Chỉ Admin mới có thể tạo nhân viên mới
    [Authorize(Roles = "Admin, Employee")]
    public async Task<IActionResult> RegisterEmployee([FromBody] EmployeeRegisterRequest request)
    {
        try
        {
            var response = await _service.RegisterEmployeeAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }
}
