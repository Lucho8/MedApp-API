using MedApp.Application.Common.DTOs;
using MedApp.Application.Common.Exceptions;
using MedApp.Application.Common.Interfaces;
using MedApp.Domain.Entities;
using MedApp.Domain.Enums;
using MedApp.Domain.Interfaces;
using MediatR;

namespace MedApp.Application.Features.Auth.Commands;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponse>
{
    private readonly IUnitOfWork _uow;
    private readonly ITokenService _tokenService;

    public RegisterCommandHandler(IUnitOfWork uow, ITokenService tokenService)
    {
        _uow = uow;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existing = await _uow.Users.FindAsync(u => u.Email == request.Email);
        if (existing.Any())
            throw new AppException("El email ya está registrado.");

        if (!Enum.TryParse<UserRole>(request.Role, true, out var role))
            throw new AppException("Rol inválido.");

        var user = new User
        {
            Email = request.Email.ToLower().Trim(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = role,
            IsActive = true
        };

        await _uow.Users.AddAsync(user);

        if (role == UserRole.Doctor)
        {
            var doctor = new Doctor
            {
                UserId = user.Id,
                FirstName = request.FirstName,
                LastName = request.LastName
            };
            await _uow.Doctors.AddAsync(doctor);
        }
        else if (role == UserRole.Patient)
        {
            var patient = new Patient
            {
                UserId = user.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = user.Email
            };
            await _uow.Patients.AddAsync(patient);
        }

        var refreshToken = _tokenService.GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

        await _uow.SaveChangesAsync();

        return new AuthResponse(
            _tokenService.GenerateAccessToken(user),
            refreshToken,
            user.Role.ToString(),
            user.Id
        );
    }
}