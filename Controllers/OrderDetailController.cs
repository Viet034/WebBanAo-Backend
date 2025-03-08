using Microsoft.AspNetCore.Mvc;
using WebBanAoo.Models.DTO.Request.OrderDetail;
using WebBanAoo.Models;
using WebBanAoo.Service;
using System.Net;
using WebBanAoo.Models.DTO.Request.Employee;
using static WebBanAoo.Models.Status.Status;


namespace WebBanAoo.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _service;

        public OrderDetailController(IOrderDetailService service)
        {
            _service = service;
        }

        [HttpPost("AddOrderDetail")]
        [ProducesResponseType(typeof(IEnumerable<OrderDetail>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddOrderDetail([FromBody] OrderDetailCreate create)
        {
            try
            {
                var response = await _service.CreateOrderDetailAsync(create);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpGet("orderDetailId/{id}")]
        [ProducesResponseType(typeof(IEnumerable<OrderDetail>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> FindOrderDetailByOrderId(int id)
        {
            try
            {
                var response = await _service.GetOrderDetailByOrderIdAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(IEnumerable<OrderDetail>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<IEnumerable<OrderDetail>>> GetAllOrderDetail()
        {
            try
            {
                var response = await _service.GetAllOrderDetailAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("FindByName/{name}")]
        [ProducesResponseType(typeof(IEnumerable<OrderDetail>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> FindByName(string name)
        {
            try
            {
                var response = await _service.SearchOrderDetailByCodeAsync(name);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("findId/{id}")]
        [ProducesResponseType(typeof(IEnumerable<OrderDetail>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> FindById(int id)
        {
            try
            {
                var response = await _service.FindOrderDetailByIdAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Update/{id}")]
        [ProducesResponseType(typeof(IEnumerable<OrderDetail>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateOrderDetail([FromBody] OrderDetailUpdate update, int id)
        {
            try
            {
                var response = await _service.UpdateOrderDetailAsync(id, update);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPut("ChangeSstatus/{id}")]
        [ProducesResponseType(typeof(IEnumerable<OrderDetail>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SoftDeleteOrderDetail(int id, OrderDetailStatus newStatus)
        {
            try
            {
                var response = await _service.SoftDeleteOrderDetailAsync(id, newStatus);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpDelete("DeletePermanent/{id}")]
        [ProducesResponseType(typeof(IEnumerable<OrderDetail>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> HardDeleteOrderDetail(int id)
        {
            try
            {
                var response = await _service.HardDeleteOrderDetailAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
