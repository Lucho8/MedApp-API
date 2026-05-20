namespace MedApp.Application.Common.DTOs;

public record RegisterRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string Role
);

public record LoginRequest(
    string Email,
    string Password
);

public record AuthResponse(
    string AccessToken,
    string RefreshToken,
    string Role,
    Guid UserId
);