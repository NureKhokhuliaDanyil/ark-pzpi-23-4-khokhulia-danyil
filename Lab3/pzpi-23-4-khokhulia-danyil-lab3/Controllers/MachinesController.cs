using Microsoft.AspNetCore.Mvc;
using Washing.DTOs;
using Washing.Entities;
using Washing.Interfaces;

namespace Washing.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MachinesController : ControllerBase
{
    private readonly IGenericRepository<WashingMachine> _repository;

    public MachinesController(IGenericRepository<WashingMachine> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MachineResponseDto>>> GetAll()
    {
        var machines = await _repository.GetAllAsync();
        var response = machines.Select(m => new MachineResponseDto(m.Id, m.LaundryId, m.SerialNumber, m.Model, m.Status));
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MachineResponseDto>> GetById(int id)
    {
        var machine = await _repository.GetByIdAsync(id);
        if (machine == null)
            return NotFound();

        var response = new MachineResponseDto(machine.Id, machine.LaundryId, machine.SerialNumber, machine.Model, machine.Status);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<MachineResponseDto>> Create(CreateMachineDto dto)
    {
        var machine = new WashingMachine
        {
            LaundryId = dto.LaundryId,
            SerialNumber = dto.SerialNumber,
            Model = dto.Model,
            Status = dto.Status
        };

        var created = await _repository.AddAsync(machine);
        var response = new MachineResponseDto(created.Id, created.LaundryId, created.SerialNumber, created.Model, created.Status);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateMachineDto dto)
    {
        var machine = await _repository.GetByIdAsync(id);
        if (machine == null)
            return NotFound();

        machine.LaundryId = dto.LaundryId;
        machine.SerialNumber = dto.SerialNumber;
        machine.Model = dto.Model;
        machine.Status = dto.Status;

        await _repository.UpdateAsync(machine);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var machine = await _repository.GetByIdAsync(id);
        if (machine == null)
            return NotFound();

        await _repository.DeleteAsync(id);
        return NoContent();
    }
}
