using Microsoft.AspNetCore.Mvc;
using WebBanAoo.Models;
using WebBanAoo.Service;
using System.Net;
using WebBanAoo.Models.DTO.Request.Category;
using static WebBanAoo.Models.Status.Status;
namespace WebBanAoo.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _service;

    public CategoryController(ICategoryService service)
    {
        _service = service;
    }

    [HttpPost("AddCategory")]
    [ProducesResponseType(typeof(IEnumerable<Category>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> AddCategory([FromBody] CategoryCreate create)
    {
        try
        {
            var response = await _service.CreateCategoryAsync(create);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpGet("GetAll")]
    [ProducesResponseType(typeof(IEnumerable<Category>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<IEnumerable<Category>>> GetAllCategory()
    {
        try
        {
            var response = await _service.GetAllCategoryAsync();
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpGet("FindByName/{name}")]
    [ProducesResponseType(typeof(IEnumerable<Category>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> FindByName(string name)
    {
        try
        {
            var response = await _service.SearchCategoryByKeyAsync(name);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpGet("findId/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Category>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> FindById(int id)
    {
        try
        {
            var response = await _service.FindCategoryByIdAsync(id);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("Update/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Category>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateCategory([FromBody] CategoryUpdate update, int id)
    {
        try
        {
            var response = await _service.UpdateCategoryAsync(id, update);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpPut("ChangeSstatus/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Category>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> SoftDeleteCategory(int id, CategoryStatus newStatus)
    {
        try
        {
            var response = await _service.SoftDeleteCategoryAsync(id, newStatus);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpDelete("DeletePermanent/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Category>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> HardDeleteCategory(int id)
    {
        try
        {
            var response = await _service.HardDeleteCategoryAsync(id);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }
}
