using MedApp.Application.Common.DTOs;
using MedApp.Application.Common.Exceptions;
using MedApp.Application.Common.Interfaces;
using MedApp.Domain.Interfaces;
using MediatR;

namespace MedApp.Application.Features.Auth.Queries;

public class LoginQueryHandler : IRequestHandler<LoginQuery, AuthResponse>
{
    private readonly IUnitOfWork _uow;
    private readonly ITokenService _tokenService;

    public LoginQueryHandler(IUnitOfWork uow, ITokenService tokenService)
    {
        _uow = uow;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var users = await _uow.Users.FindAsync(u => u.Email == request.Email.ToLower().Trim());
        var user = users.FirstOrDefault();

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new AppException("Email o contraseña incorrectos.", 401);

        if (!user.IsActive)
            throw new AppException("La cuenta está desactivada.", 403);

        var refreshToken = _tokenService.GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

        _uow.Users.Update(user);
        await _uow.SaveChangesAsync();

        return new AuthResponse(
            _tokenService.GenerateAccessToken(user),
            refreshToken,
            user.Role.ToString(),
            user.Id
        );
    }
}