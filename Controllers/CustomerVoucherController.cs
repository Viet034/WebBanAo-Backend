using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebBanAoo.Models.DTO.Request.CustomerVoucher;
using WebBanAoo.Service;
using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerVoucherController : ControllerBase
    {
        private readonly ICustomerVoucherService _service;

        public CustomerVoucherController(ICustomerVoucherService service)
        {
            _service = service;
        }

        [HttpPost("assign")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AssignVoucherToCustomer([FromBody] CustomerVoucherCreate request)
        {
            try
            {
                var result = await _service.AssignVoucherToCustomerAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("customer/{customerId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetVouchersByCustomerId(int customerId)
        {
            try
            {
                var vouchers = await _service.GetVouchersByCustomerIdAsync(customerId);
                return Ok(vouchers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{customerVoucherId}/status")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateStatus(int customerVoucherId, [FromQuery] CustomerVoucherStatus newStatus)
        {
            try
            {
                var result = await _service.UpdateCustomerVoucherStatusAsync(customerVoucherId, newStatus);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{customerVoucherId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Delete(int customerVoucherId)
        {
            try
            {
                var result = await _service.DeleteCustomerVoucherAsync(customerVoucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("check-availability/{voucherId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CheckVoucherAvailability(int voucherId)
        {
            try
            {
                var isAvailable = await _service.CheckVoucherAvailabilityAsync(voucherId);
                return Ok(isAvailable);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
} 