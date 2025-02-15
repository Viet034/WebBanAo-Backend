using Microsoft.AspNetCore.Mvc;
using WebBanAoo.Service;
using WebBanAoo.Models.DTO.Request.Size;
using WebBanAoo.Models;
using System.Net;
using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Controllers;

[ApiController]
[Route("/api/[controller]")]

public class SizeController : ControllerBase
{
    private readonly ISizeService _service;

    public SizeController(ISizeService service)
    {
        _service = service;
    }

    [HttpPost("AddSize")]
    [ProducesResponseType(typeof(IEnumerable<Sale>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> AddSize([FromBody] SizeCreate create)
    {
        try
        {
            var response = await _service.CreateSizeAsync(create);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    
    [HttpGet("GetAll")]
    [ProducesResponseType(typeof(IEnumerable<Size>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<IEnumerable<Size>>> GetAllSale()
    {
        try
        {
            var response = await _service.GetAllSizeAsync();
            return Ok(response);
        }catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpGet("FindByName/{name}")]
    [ProducesResponseType(typeof(IEnumerable<Size>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> FindByName(string name)
    {
        try
        {
            var response = await _service.SearchSizeByKeyAsync(name);
            return Ok(response);
        }
        catch (Exception ex) 
        { 
            return BadRequest(ex.ToString());
        }
    }

    [HttpGet("findId/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Size>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> FindById(int id)
    {
        try
        {
            var response = await _service.FindSizeByIdAsync(id);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }



    [HttpPut("Update/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Size>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateSize([FromBody] SizeUpdate update, int id)
    {
        try
        {
            var response = await _service.UpdateSizeAsync(id, update);
            return Ok(response);
        }catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }




    [HttpPut("ChangeSstatus/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Size>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> SoftDeleteSize(int id, SizeStatus newStatus)
    {
        try
        {
            var response = await _service.SoftDeleteSizeAsync(id, newStatus);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }



    [HttpDelete("DeletePermanent/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Size>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> HardDeleteSize(int id)
    {
        try
        {
            var response = await _service.HardDeleteSizeAsync(id);
            return Ok(response);
        }catch(Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

}
