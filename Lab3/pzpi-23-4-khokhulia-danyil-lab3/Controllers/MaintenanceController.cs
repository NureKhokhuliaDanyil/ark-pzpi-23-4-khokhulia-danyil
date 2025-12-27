using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Washing.DTOs;
using Washing.Entities;
using Washing.Interfaces;

namespace Washing.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Technician,Admin")]
public class MaintenanceController : ControllerBase
{
    private readonly IGenericRepository<MaintenanceLog> _repository;
    private readonly IMaintenanceService _maintenanceService;

    public MaintenanceController(IGenericRepository<MaintenanceLog> repository, IMaintenanceService maintenanceService)
    {
        _repository = repository;
        _maintenanceService = maintenanceService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MaintenanceLogResponseDto>>> GetAll()
    {
        var logs = await _repository.GetAllAsync();
        var response = logs.Select(l => new MaintenanceLogResponseDto(l.Id, l.MachineId, l.Description, l.StartedAt, l.ResolvedAt, l.Cost));
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MaintenanceLogResponseDto>> GetById(int id)
    {
        var log = await _repository.GetByIdAsync(id);
        if (log == null)
            return NotFound();

        var response = new MaintenanceLogResponseDto(log.Id, log.MachineId, log.Description, log.StartedAt, log.ResolvedAt, log.Cost);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<MaintenanceLogResponseDto>> Create(CreateMaintenanceLogDto dto)
    {
        var log = new MaintenanceLog
        {
            MachineId = dto.MachineId,
            Description = dto.Description,
            StartedAt = dto.StartedAt
        };

        var created = await _repository.AddAsync(log);
        var response = new MaintenanceLogResponseDto(created.Id, created.MachineId, created.Description, created.StartedAt, created.ResolvedAt, created.Cost);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateMaintenanceLogDto dto)
    {
        var log = await _repository.GetByIdAsync(id);
        if (log == null)
            return NotFound();

        log.MachineId = dto.MachineId;
        log.Description = dto.Description;
        log.StartedAt = dto.StartedAt;

        await _repository.UpdateAsync(log);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var log = await _repository.GetByIdAsync(id);
        if (log == null)
            return NotFound();

        await _repository.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("report")]
    public async Task<ActionResult<int>> ReportIssue(ReportIssueDto dto)
    {
        try
        {
            var logId = await _maintenanceService.ReportIssueAsync(dto.MachineId, dto.Description);
            return Ok(new { logId, message = "Issue reported successfully" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("resolve")]
    public async Task<IActionResult> ResolveIssue(ResolveIssueDto dto)
    {
        try
        {
            await _maintenanceService.ResolveIssueAsync(dto.LogId, dto.Cost, dto.Notes);
            return Ok(new { message = "Issue resolved successfully" });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
