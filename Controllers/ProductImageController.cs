using Microsoft.AspNetCore.Mvc;
using WebBanAoo.Service;
using WebBanAoo.Models.DTO.Request.ProductImage;
using WebBanAoo.Models;
using System.Net;
using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Controllers;

[ApiController]
[Route("/api/[controller]")]

public class ProductImageController : ControllerBase
{
    private readonly IProductImageService _service;

    public ProductImageController(IProductImageService service)
    {
        _service = service;
    }

    [HttpPost("AddProductImage")]
    [ProducesResponseType(typeof(IEnumerable<ProductImage>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> AddProductImage([FromBody] ProductImageCreate create)
    {
        try
        {
            var response = await _service.CreateProductImageAsync(create);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    
    [HttpGet("GetProductImage")]
    [ProducesResponseType(typeof(IEnumerable<ProductImage>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<IEnumerable<ProductImage>>> GetAllProductImage()
    {
        try
        {
            var response = await _service.GetAllProductImageAsync();
            return Ok(response);
        }catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }


    [HttpGet("productDetailId/{id}")]
    [ProducesResponseType(typeof(IEnumerable<ProductImage>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> FindProductImageByProductDetailId(int id)
    {
        try
        {
            var response = await _service.FindProductImageByProductDetailIdAsync(id);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }



    [HttpPut("Update/{id}")]
    [ProducesResponseType(typeof(IEnumerable<ProductImage>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateProductImage([FromBody] ProductImageUpdate update, int id)
    {
        try
        {
            var response = await _service.UpdateProductImageAsync(id, update);
            return Ok(response);
        }catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }




    



    [HttpDelete("DeletePermanent/{id}")]
    [ProducesResponseType(typeof(IEnumerable<ProductImage>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> HardDeleteProductImage(int id)
    {
        try
        {
            var response = await _service.HardDeleteProductImageAsync(id);
            return Ok(response);
        }catch(Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

}
