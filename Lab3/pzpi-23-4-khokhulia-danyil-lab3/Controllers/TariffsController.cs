using Microsoft.AspNetCore.Mvc;
using Washing.DTOs;
using Washing.Entities;
using Washing.Interfaces;

namespace Washing.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TariffsController : ControllerBase
{
    private readonly IGenericRepository<WashMode> _repository;

    public TariffsController(IGenericRepository<WashMode> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WashModeResponseDto>>> GetAll()
    {
        var modes = await _repository.GetAllAsync();
        var response = modes.Select(m => new WashModeResponseDto(m.Id, m.LaundryId, m.Name, m.Price, m.DurationMinutes));
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WashModeResponseDto>> GetById(int id)
    {
        var mode = await _repository.GetByIdAsync(id);
        if (mode == null)
            return NotFound();

        var response = new WashModeResponseDto(mode.Id, mode.LaundryId, mode.Name, mode.Price, mode.DurationMinutes);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<WashModeResponseDto>> Create(CreateWashModeDto dto)
    {
        var mode = new WashMode
        {
            LaundryId = dto.LaundryId,
            Name = dto.Name,
            Price = dto.Price,
            DurationMinutes = dto.DurationMinutes
        };

        var created = await _repository.AddAsync(mode);
        var response = new WashModeResponseDto(created.Id, created.LaundryId, created.Name, created.Price, created.DurationMinutes);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateWashModeDto dto)
    {
        var mode = await _repository.GetByIdAsync(id);
        if (mode == null)
            return NotFound();

        mode.LaundryId = dto.LaundryId;
        mode.Name = dto.Name;
        mode.Price = dto.Price;
        mode.DurationMinutes = dto.DurationMinutes;

        await _repository.UpdateAsync(mode);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var mode = await _repository.GetByIdAsync(id);
        if (mode == null)
            return NotFound();

        await _repository.DeleteAsync(id);
        return NoContent();
    }
}
