using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Washing.DTOs;
using Washing.Interfaces;

namespace Washing.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WalletController : ControllerBase
{
    private readonly IFinanceService _financeService;

    public WalletController(IFinanceService financeService)
    {
        _financeService = financeService;
    }

    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit(DepositDto dto)
    {
        try
        {
            await _financeService.DepositAsync(dto.UserId, dto.Amount);
            return Ok(new { message = "Deposit successful" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
