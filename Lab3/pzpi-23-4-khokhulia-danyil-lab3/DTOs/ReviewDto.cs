namespace Washing.DTOs;

public record CreateReviewDto(int UserId, int LaundryId, int Rating, string Comment);

public record UpdateReviewDto(int UserId, int LaundryId, int Rating, string Comment);

public record ReviewResponseDto(int Id, int UserId, int LaundryId, int Rating, string Comment);
