using Microsoft.AspNetCore.Mvc;
using WebBanAoo.Models;
using WebBanAoo.Service;
using System.Net;
using WebBanAoo.Models.DTO.Request.Customer;
using static WebBanAoo.Models.Status.Status;
using Microsoft.AspNetCore.Authorization;
namespace WebBanAoo.Controllers;

[ApiController]
[Route("/api/[controller]")]

public class CustomerController : ControllerBase
{
    private readonly ICustomerService _service;

    public CustomerController(ICustomerService service)
    {
        _service = service;
    }

    [HttpPost("AddCustomer")]
    [ProducesResponseType(typeof(IEnumerable<Customer>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [Authorize(Roles = "Admin, Employee")]
    public async Task<IActionResult> AddCustomer([FromBody] CustomerCreate create)
    {
        try
        {
            var response = await _service.CreateCustomerAsync(create);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpGet("GetAll")]
    [ProducesResponseType(typeof(IEnumerable<Customer>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [Authorize(Roles = "Admin, Employee")]
    public async Task<ActionResult<IEnumerable<Customer>>> GetAllCustomer()
    {
        try
        {
            var response = await _service.GetAllCustomerAsync();
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpGet("FindByName/{name}")]
    [ProducesResponseType(typeof(IEnumerable<Customer>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [Authorize(Roles = "Admin, Employee")]
    public async Task<IActionResult> FindByName(string name)
    {
        try
        {
            var response = await _service.SearchCustomerByKeyAsync(name);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpGet("findId/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Customer>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [AllowAnonymous]
    
    public async Task<IActionResult> FindById(int id)
    {
        try
        {
            var response = await _service.FindCustomerByIdAsync(id);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("Update/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Customer>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [AllowAnonymous]
    public async Task<IActionResult> UpdateCustomer([FromBody] CustomerUpdate update, int id)
    {
        try
        {
            var response = await _service.UpdateCustomerAsync(id, update);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpPut("ChangeSstatus/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Customer>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [Authorize(Roles = "Admin, Employee")]
    public async Task<IActionResult> SoftDeleteCustomer(int id, CustomerStatus newStatus)
    {
        try
        {
            var response = await _service.SoftDeleteCustomerAsync(id, newStatus);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpDelete("DeletePermanent/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Customer>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [AllowAnonymous]
    public async Task<IActionResult> HardDeleteCustomer(int id)
    {
        try
        {
            var response = await _service.HardDeleteCustomerAsync(id);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }
}
