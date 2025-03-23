using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Employee;
using WebBanAoo.Models.DTO.Request.ProductDetail;
using WebBanAoo.Service;
using System.Net;
using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ProductDetailController : ControllerBase
    {
        private readonly IProductDetailService _service;
        

        public ProductDetailController(IProductDetailService service)
        {
            _service = service;
        }

        [HttpPost("AddProductDetail")]
        [ProducesResponseType(typeof(IEnumerable<ProductDetail>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddProductDetail([FromBody] ProductDetailCreate create)
        {
            try
            {
                var response = await _service.CreateProductDetailAsync(create);
                return Ok(response);
            }catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(IEnumerable<ProductDetail>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<IEnumerable<ProductDetail>>> GetAll()
        {
            try
            {
                var response = await _service.GetAllProductDetailAsync();
                return Ok(response);
            }catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("FindByName/{name}")]
        [ProducesResponseType(typeof(IEnumerable<ProductDetail>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> FindByName(string name)
        {
            try
            {
                var response = await _service.SearchProductDetailByKeyAsync(name);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("findId/{id}")]
        [ProducesResponseType(typeof(IEnumerable<ProductDetail>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> FindById(int id)
        {
            try
            {
                var response = await _service.FindProductDetailByIdAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("productId/{id}")]
        [ProducesResponseType(typeof(IEnumerable<ProductDetail>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> FindProductDetailByProductIdAsync(int id)
        {
            try
            {
                var response = await _service.FindProductDetailByProductIdAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Update/{id}")]
        [ProducesResponseType(typeof(IEnumerable<ProductDetail>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateProductDetail([FromBody] ProductDetailUpdate update, int id)
        {
            try
            {
                var response = await _service.UpdateProductDetailAsync(id, update);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPut("ChangeStatus/{id}")]
        [ProducesResponseType(typeof(IEnumerable<ProductDetail>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SoftDeleteProductDetail(int id, ProductDetailStatus newStatus)
        {
            try
            {
                var response = await _service.SoftDeleteProductDetailAsync(id, newStatus);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpDelete("DeletePermanent/{id}")]
        [ProducesResponseType(typeof(IEnumerable<ProductDetail>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> HardDeleteProductDetail(int id)
        {
            try
            {
                var response = await _service.HardDeleteProductDetailAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
