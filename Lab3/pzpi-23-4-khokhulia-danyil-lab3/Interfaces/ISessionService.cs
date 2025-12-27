using Washing.DTOs;

namespace Washing.Interfaces;

public interface ISessionService
{
    Task<SessionResponseDto> StartSessionAsync(StartSessionDto dto);
    Task CompleteSessionAsync(int sessionId);
    Task CancelSessionAsync(int sessionId, int userId);
}
