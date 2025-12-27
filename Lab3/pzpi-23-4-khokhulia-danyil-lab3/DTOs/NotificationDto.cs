namespace Washing.DTOs;

public record NotificationResponseDto(int Id, int UserId, string Title, string Message, bool IsRead);
