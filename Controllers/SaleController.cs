using Microsoft.AspNetCore.Mvc;
using WebBanAoo.Service;
using WebBanAoo.Models.DTO.Request.Sale;
using WebBanAoo.Models;
using System.Net;
using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Controllers;

[ApiController]
[Route("/api/[controller]")]

public class SaleController : ControllerBase
{
    private readonly ISaleService _service;

    public SaleController(ISaleService service)
    {
        _service = service;
    }

    [HttpPost("AddSale")]
    [ProducesResponseType(typeof(IEnumerable<Sale>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> AddSale([FromBody] SaleCreate create)
    {
        try
        {
            var response = await _service.CreateSaleAsync(create);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    
    [HttpGet("GetAll")]
    [ProducesResponseType(typeof(IEnumerable<Sale>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<IEnumerable<Sale>>> GetAllSale()
    {
        try
        {
            var response = await _service.GetAllSaleAsync();
            return Ok(response);
        }catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpGet("FindByName/{name}")]
    [ProducesResponseType(typeof(IEnumerable<Sale>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> FindByName(string name)
    {
        try
        {
            var response = await _service.SearchSaleByKeyAsync(name);
            return Ok(response);
        }
        catch (Exception ex) 
        { 
            return BadRequest(ex.ToString());
        }
    }

    [HttpGet("findId/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Sale>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> FindById(int id)
    {
        try
        {
            var response = await _service.FindSaleByIdAsync(id);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }



    [HttpPut("Update/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Sale>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateSale([FromBody] SaleUpdate update, int id)
    {
        try
        {
            var response = await _service.UpdateSaleAsync(id, update);
            return Ok(response);
        }catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }




    [HttpPut("ChangeSstatus/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Sale>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> SoftDeleteSale(int id, SaleStatus newStatus)
    {
        try
        {
            var response = await _service.SoftDeleteSaleAsync(id, newStatus);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }



    [HttpDelete("DeletePermanent/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Sale>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> HardDeleteSale(int id)
    {
        try
        {
            var response = await _service.HardDeleteSaleAsync(id);
            return Ok(response);
        }catch(Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

}
