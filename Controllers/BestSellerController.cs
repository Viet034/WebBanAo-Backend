using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Response;
using WebBanAoo.Service;

namespace WebBanAoo.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class BestSellerController : ControllerBase
{
    private readonly IBestSellerService _service;

    public BestSellerController(IBestSellerService service)
    {
        _service = service;
    }

    [HttpGet("GetAll")]
    [ProducesResponseType(typeof(IEnumerable<BestSellerResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    
    public async Task<ActionResult<IEnumerable<BestSellerResponse>>> GetBestSeller()
    {
        try
        {
            var response = await _service.GetBestSellersAsync();
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }
}
