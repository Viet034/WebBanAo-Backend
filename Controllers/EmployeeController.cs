using Microsoft.AspNetCore.Mvc;
using WebBanAoo.Service;
using WebBanAoo.Models.DTO.Request.Employee;
using WebBanAoo.Models;
using System.Net;
using static WebBanAoo.Models.Status.Status;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WebBanAoo.Controllers;

[ApiController]
[Route("/api/[controller]")]


public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _service;

    public EmployeeController(IEmployeeService service)
    {
        _service = service;
    }

    [HttpPost("AddEmployee")]
    [ProducesResponseType(typeof(IEnumerable<Employee>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddEmployee([FromBody] EmployeeCreate create)
    {
        try
        {
            var response = await _service.CreateEmployeeAsync(create);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    
    [HttpGet("GetAll")]
    [ProducesResponseType(typeof(IEnumerable<Employee>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployee()
    {
        try
        {
            var response = await _service.GetAllEmployeeAsync();
            return Ok(response);
        }catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpGet("FindByName/{name}")]
    [ProducesResponseType(typeof(IEnumerable<Employee>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<IActionResult> FindByName(string name)
    {
        try
        {
            var response = await _service.SearchEmployeeByKeyAsync(name);
            return Ok(response);
        }
        catch (Exception ex) 
        { 
            return BadRequest(ex.ToString());
        }
    }

    [HttpGet("findId/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Employee>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<IActionResult> FindById(int id)
    {
        try
        {
            var response = await _service.FindEmployeeByIdAsync(id);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }



    [HttpPut("Update/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Employee>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeUpdate update, int id)
    {
        try
        {
            var response = await _service.UpdateEmployeeAsync(id, update);
            return Ok(response);
        }catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }




    [HttpPut("ChangeSstatus/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Employee>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SoftDeleteEmployee(int id, EmployeeStatus newStatus)
    {
        try
        {
            var response = await _service.SoftDeleteEmployeeAsync(id, newStatus);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }



    [HttpDelete("DeletePermanent/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Employee>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> HardDeleteEmployee(int id)
    {
        try
        {
            var response = await _service.HardDeleteEmployeeAsync(id);
            return Ok(response);
        }catch(Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    //[HttpGet("test-auth")]
    //[Authorize] // Giữ lại Authorize cho endpoint này
    //public IActionResult TestAuth()
    //{
    //    var identity = HttpContext.User.Identity as ClaimsIdentity;
    //    var claims = identity?.Claims.Select(c => new { c.Type, c.Value }).ToList();
    //    return Ok(new { 
    //        IsAuthenticated = identity?.IsAuthenticated ?? false,
    //        Claims = claims
    //    });
    //}

}
