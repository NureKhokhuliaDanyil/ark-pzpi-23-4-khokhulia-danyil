using Microsoft.AspNetCore.Mvc;
using Washing.DTOs;
using Washing.Entities;
using Washing.Interfaces;

namespace Washing.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly IGenericRepository<Notification> _repository;

    public NotificationsController(IGenericRepository<Notification> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<NotificationResponseDto>>> GetAll()
    {
        var notifications = await _repository.GetAllAsync();
        var response = notifications.Select(n => new NotificationResponseDto(n.Id, n.UserId, n.Title, n.Message, n.IsRead));
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<NotificationResponseDto>> GetById(int id)
    {
        var notification = await _repository.GetByIdAsync(id);
        if (notification == null)
            return NotFound();

        var response = new NotificationResponseDto(notification.Id, notification.UserId, notification.Title, notification.Message, notification.IsRead);
        return Ok(response);
    }
}
