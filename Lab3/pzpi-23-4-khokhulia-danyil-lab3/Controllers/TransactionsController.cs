using Microsoft.AspNetCore.Mvc;
using Washing.DTOs;
using Washing.Entities;
using Washing.Interfaces;

namespace Washing.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly IGenericRepository<Transaction> _repository;

    public TransactionsController(IGenericRepository<Transaction> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransactionResponseDto>>> GetAll()
    {
        var transactions = await _repository.GetAllAsync();
        var response = transactions.Select(t => new TransactionResponseDto(t.Id, t.UserId, t.Amount, t.Type, t.Timestamp));
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TransactionResponseDto>> GetById(int id)
    {
        var transaction = await _repository.GetByIdAsync(id);
        if (transaction == null)
            return NotFound();

        var response = new TransactionResponseDto(transaction.Id, transaction.UserId, transaction.Amount, transaction.Type, transaction.Timestamp);
        return Ok(response);
    }
}
