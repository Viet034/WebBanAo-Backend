using Microsoft.AspNetCore.Mvc;
using WebBanAoo.Service;
using WebBanAoo.Models.DTO.Request.Role;
using WebBanAoo.Models;
using System.Net;
using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Controllers;

[ApiController]
[Route("/api/[controller]")]

public class RoleController : ControllerBase
{
    private readonly IRoleService _service;

    public RoleController(IRoleService service)
    {
        _service = service;
    }

    [HttpPost("AddRole")]
    [ProducesResponseType(typeof(IEnumerable<Role>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> AddRole([FromBody] RoleCreate create)
    {
        try
        {
            var response = await _service.CreateRoleAsync(create);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    
    [HttpGet("GetAll")]
    [ProducesResponseType(typeof(IEnumerable<Role>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<IEnumerable<Role>>> GetAllEmployee()
    {
        try
        {
            var response = await _service.GetAllRoleAsync();
            return Ok(response);
        }catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpGet("FindByName/{name}")]
    [ProducesResponseType(typeof(IEnumerable<Role>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> FindByName(string name)
    {
        try
        {
            var response = await _service.SearchRoleByKeyAsync(name);
            return Ok(response);
        }
        catch (Exception ex) 
        { 
            return BadRequest(ex.ToString());
        }
    }

    [HttpGet("findId/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Role>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> FindById(int id)
    {
        try
        {
            var response = await _service.FindRoleByIdAsync(id);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }



    [HttpPut("Update/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Role>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateRole([FromBody] RoleUpdate update, int id)
    {
        try
        {
            var response = await _service.UpdateRoleAsync(id, update);
            return Ok(response);
        }catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }




    [HttpPut("ChangeStatus/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Role>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> SoftDeleteRole(int id, RoleStatus newStatus)
    {
        try
        {
            var response = await _service.SoftDeleteRoleAsync(id, newStatus);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }



    [HttpDelete("DeletePermanent/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Role>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> HardDeleteRole(int id)
    {
        try
        {
            var response = await _service.HardDeleteRoleAsync(id);
            return Ok(response);
        }catch(Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

}
