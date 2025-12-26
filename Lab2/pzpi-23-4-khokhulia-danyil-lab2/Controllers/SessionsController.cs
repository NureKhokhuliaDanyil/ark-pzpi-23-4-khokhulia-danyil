using Microsoft.AspNetCore.Mvc;
using Washing.DTOs;
using Washing.Entities;
using Washing.Interfaces;

namespace Washing.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SessionsController : ControllerBase
{
    private readonly IGenericRepository<WashingSession> _repository;

    public SessionsController(IGenericRepository<WashingSession> repository)
    {
        _repository = repository;
    }

    [HttpPost]
    public async Task<ActionResult<SessionResponseDto>> Create(CreateSessionDto dto)
    {
        var session = new WashingSession
        {
            UserId = dto.UserId,
            MachineId = dto.MachineId,
            ModeId = dto.ModeId,
            StartTime = dto.StartTime,
            Status = dto.Status,
            ActualPrice = dto.ActualPrice
        };

        var created = await _repository.AddAsync(session);
        var response = new SessionResponseDto(created.Id, created.UserId, created.MachineId, created.ModeId, created.StartTime, created.EndTime, created.Status, created.ActualPrice);
        return CreatedAtAction(nameof(Create), new { id = created.Id }, response);
    }
}
