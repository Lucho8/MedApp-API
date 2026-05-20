using MedApp.Application.Common.DTOs;
using MediatR;

namespace MedApp.Application.Features.Auth.Commands;

public record RegisterCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string Role
) : IRequest<AuthResponse>;