using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Washing.DTOs;
using Washing.Interfaces;

namespace Washing.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SessionsController : ControllerBase
{
    private readonly ISessionService _sessionService;
    private readonly IPricingService _pricingService;

    public SessionsController(ISessionService sessionService, IPricingService pricingService)
    {
        _sessionService = sessionService;
        _pricingService = pricingService;
    }

    [HttpPost("preview-price")]
    public async Task<ActionResult<PricingDetailDto>> PreviewPrice(int laundryId, int modeId, int userId)
    {
        try
        {
            var pricingDetail = await _pricingService.CalculateFinalPriceAsync(laundryId, modeId, userId);
            return Ok(pricingDetail);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("calculate-price")]
    public async Task<ActionResult<decimal>> CalculatePrice(int machineId, int modeId)
    {
        try
        {
            var price = await _pricingService.CalculatePriceAsync(machineId, modeId);
            return Ok(new { machineId, modeId, price });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("start")]
    public async Task<ActionResult<SessionResponseDto>> StartSession(StartSessionDto dto)
    {
        try
        {
            var session = await _sessionService.StartSessionAsync(dto);
            return CreatedAtAction(nameof(StartSession), new { id = session.Id }, session);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{id}/complete")]
    public async Task<IActionResult> CompleteSession(int id)
    {
        try
        {
            await _sessionService.CompleteSessionAsync(id);
            return Ok(new { message = "Session completed successfully" });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> CancelSession(int id, [FromBody] int userId)
    {
        try
        {
            await _sessionService.CancelSessionAsync(id, userId);
            return Ok(new { message = "Session cancelled successfully" });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
