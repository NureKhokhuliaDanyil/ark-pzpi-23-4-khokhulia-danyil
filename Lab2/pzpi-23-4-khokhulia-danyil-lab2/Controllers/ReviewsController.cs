using Microsoft.AspNetCore.Mvc;
using Washing.DTOs;
using Washing.Entities;
using Washing.Interfaces;

namespace Washing.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly IGenericRepository<Review> _repository;

    public ReviewsController(IGenericRepository<Review> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReviewResponseDto>>> GetAll()
    {
        var reviews = await _repository.GetAllAsync();
        var response = reviews.Select(r => new ReviewResponseDto(r.Id, r.UserId, r.LaundryId, r.Rating, r.Comment));
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReviewResponseDto>> GetById(int id)
    {
        var review = await _repository.GetByIdAsync(id);
        if (review == null)
            return NotFound();

        var response = new ReviewResponseDto(review.Id, review.UserId, review.LaundryId, review.Rating, review.Comment);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<ReviewResponseDto>> Create(CreateReviewDto dto)
    {
        var review = new Review
        {
            UserId = dto.UserId,
            LaundryId = dto.LaundryId,
            Rating = dto.Rating,
            Comment = dto.Comment
        };

        var created = await _repository.AddAsync(review);
        var response = new ReviewResponseDto(created.Id, created.UserId, created.LaundryId, created.Rating, created.Comment);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateReviewDto dto)
    {
        var review = await _repository.GetByIdAsync(id);
        if (review == null)
            return NotFound();

        review.UserId = dto.UserId;
        review.LaundryId = dto.LaundryId;
        review.Rating = dto.Rating;
        review.Comment = dto.Comment;

        await _repository.UpdateAsync(review);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var review = await _repository.GetByIdAsync(id);
        if (review == null)
            return NotFound();

        await _repository.DeleteAsync(id);
        return NoContent();
    }
}
