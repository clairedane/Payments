using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payments.Application.DTOs;
using Payments.Application.Services;
using Payments.Domain.Enums;

namespace Payments.Api.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // JWT-protected
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _service;

        public PaymentsController(IPaymentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentDetails>>> GetAllPayments()
        {
            var payments = await _service.GetAllPaymentsAsync();
            return Ok(payments);
        }

        [HttpGet("{referenceID}")]
        public async Task<IActionResult> GetPayment(string referenceID)
        {
            var payment = await _service.GetPaymentByReferenceIDAsync(referenceID);

            if (payment == null)
                return NotFound(new { Message = "Payment not found" });

            return Ok(payment);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PaymentRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _service.ProcessPaymentAsync(request);
            if (result.Status == PaymentStatus.Success)
                return Ok(result);

            if (result.Status == PaymentStatus.Pending)
                return Accepted(result);

            if (result.Status == PaymentStatus.Failed)
                return UnprocessableEntity(result);

            return Conflict(result);

        }

    }

}
