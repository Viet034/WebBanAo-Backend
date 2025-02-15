using Microsoft.AspNetCore.Mvc;
using WebBanAoo.Models;
using WebBanAoo.Service;
using System.Net;
using WebBanAoo.Models.DTO.Request.Color;
using static WebBanAoo.Models.Status.Status;
namespace WebBanAoo.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ColorController : ControllerBase
{
    private readonly IColorService _service;

    public ColorController(IColorService service)
    {
        _service = service;
    }

    [HttpPost("AddColor")]
    [ProducesResponseType(typeof(IEnumerable<Color>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> AddColor([FromBody] ColorCreate create)
    {
        try
        {
            var response = await _service.CreateProductColorAsync(create);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpGet("GetAll")]
    [ProducesResponseType(typeof(IEnumerable<Color>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<IEnumerable<Color>>> GetAllColor()
    {
        try
        {
            var response = await _service.GetAllColorProductsAsync();
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpGet("FindByName/{name}")]
    [ProducesResponseType(typeof(IEnumerable<Color>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> FindByName(string name)
    {
        try
        {
            var response = await _service.SearchColorByKeyAsync(name);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpGet("findId/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Color>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> FindById(int id)
    {
        try
        {
            var response = await _service.FindProductColorByIdAsync(id);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("Update/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Color>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateColor([FromBody] ColorUpdate update, int id)
    {
        try
        {
            var response = await _service.UpdateProductColorAsync(id, update);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpPut("ChangeSstatus/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Color>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> SoftDeleteColor(int id, ColorStatus newStatus)
    {
        try
        {
            var response = await _service.SoftDeleteProductColorAsync(id, newStatus);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpDelete("DeletePermanent/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Color>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> HardDeleteColor(int id)
    {
        try
        {
            var response = await _service.HardDeleteProductColorAsync(id);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }
}
