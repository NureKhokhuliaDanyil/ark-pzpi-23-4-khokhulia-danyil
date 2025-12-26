using Microsoft.AspNetCore.Mvc;
using Washing.DTOs;
using Washing.Entities;
using Washing.Interfaces;

namespace Washing.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LaundriesController : ControllerBase
{
    private readonly IGenericRepository<Laundry> _repository;

    public LaundriesController(IGenericRepository<Laundry> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LaundryResponseDto>>> GetAll()
    {
        var laundries = await _repository.GetAllAsync();
        var response = laundries.Select(l => new LaundryResponseDto(l.Id, l.OwnerId, l.Name, l.Address, l.WorkingHours));
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LaundryResponseDto>> GetById(int id)
    {
        var laundry = await _repository.GetByIdAsync(id);
        if (laundry == null)
            return NotFound();

        var response = new LaundryResponseDto(laundry.Id, laundry.OwnerId, laundry.Name, laundry.Address, laundry.WorkingHours);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<LaundryResponseDto>> Create(CreateLaundryDto dto)
    {
        var laundry = new Laundry
        {
            OwnerId = dto.OwnerId,
            Name = dto.Name,
            Address = dto.Address,
            WorkingHours = dto.WorkingHours
        };

        var created = await _repository.AddAsync(laundry);
        var response = new LaundryResponseDto(created.Id, created.OwnerId, created.Name, created.Address, created.WorkingHours);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateLaundryDto dto)
    {
        var laundry = await _repository.GetByIdAsync(id);
        if (laundry == null)
            return NotFound();

        laundry.OwnerId = dto.OwnerId;
        laundry.Name = dto.Name;
        laundry.Address = dto.Address;
        laundry.WorkingHours = dto.WorkingHours;

        await _repository.UpdateAsync(laundry);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var laundry = await _repository.GetByIdAsync(id);
        if (laundry == null)
            return NotFound();

        await _repository.DeleteAsync(id);
        return NoContent();
    }
}
