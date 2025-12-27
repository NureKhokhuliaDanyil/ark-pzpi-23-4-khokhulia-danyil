using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Washing.DTOs;
using Washing.Interfaces;

namespace Washing.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet("balance/{userId}")]
    public async Task<ActionResult<decimal>> GetBalance(int userId)
    {
        try
        {
            var balance = await _paymentService.GetBalanceAsync(userId);
            return Ok(new { userId, balance });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost("apply-promo")]
    public async Task<IActionResult> ApplyPromo(int userId, [FromBody] PromoCodeDto dto)
    {
        try
        {
            await _paymentService.ApplyPromoCodeAsync(userId, dto.Code);
            return Ok(new { message = "Promo code applied successfully" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("process")]
    public async Task<IActionResult> ProcessPayment(ProcessPaymentDto dto)
    {
        try
        {
            await _paymentService.ProcessPaymentAsync(dto.UserId, dto.Amount);
            return Ok(new { message = "Payment processed successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
