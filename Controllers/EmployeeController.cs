using Microsoft.AspNetCore.Mvc;
using WebBanAoo.Service;
using WebBanAoo.Models.DTO.Request.Employee;
using WebBanAoo.Models;
using System.Net;
using static WebBanAoo.Models.Status.Status;

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
    public async Task<IActionResult> UpdateProduct([FromBody] EmployeeUpdate update, int id)
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

}
