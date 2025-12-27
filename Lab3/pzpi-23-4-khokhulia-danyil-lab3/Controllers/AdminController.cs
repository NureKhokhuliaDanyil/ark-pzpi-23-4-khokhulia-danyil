using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Washing.DTOs;
using Washing.Interfaces;

namespace Washing.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;
    private readonly IFinanceService _financeService;

    public AdminController(IAdminService adminService, IFinanceService financeService)
    {
        _adminService = adminService;
        _financeService = financeService;
    }

    [HttpGet("stats")]
    public async Task<ActionResult<SystemStatsDto>> GetStats()
    {
        var stats = await _adminService.GetStatsAsync();
        return Ok(stats);
    }

    [HttpGet("export-report")]
    public async Task<IActionResult> ExportReport()
    {
        var csvBytes = await _adminService.ExportLaundriesReportAsync();
        return File(csvBytes, "text/csv", "laundries_report.csv");
    }

    [HttpPost("pricing/time")]
    public async Task<ActionResult<int>> CreateTimePricing(CreateTimePricingDto dto)
    {
        var id = await _adminService.CreateTimePricingConditionAsync(dto);
        return CreatedAtAction(nameof(CreateTimePricing), new { id }, new { id, message = "Time pricing condition created" });
    }

    [HttpPost("pricing/load")]
    public async Task<ActionResult<int>> CreateLoadPricing(CreateLoadPricingDto dto)
    {
        var id = await _adminService.CreateLoadPricingConditionAsync(dto);
        return CreatedAtAction(nameof(CreateLoadPricing), new { id }, new { id, message = "Load pricing condition created" });
    }

    [HttpPost("block-user/{userId}")]
    public async Task<IActionResult> BlockUser(int userId)
    {
        try
        {
            await _adminService.BlockUserAsync(userId);
            return Ok(new { message = "User blocked successfully" });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("revenue")]
    public async Task<ActionResult<RevenueStatsDto>> GetRevenue()
    {
        var stats = await _financeService.GetRevenueStatsAsync();
        return Ok(stats);
    }

    [HttpGet("analytics/busy-hours/{laundryId}")]
    public async Task<ActionResult<List<BusyHourDto>>> GetBusyHours(int laundryId)
    {
        var analytics = await _adminService.GetBusyHoursAnalyticsAsync(laundryId);
        return Ok(analytics);
    }
}
