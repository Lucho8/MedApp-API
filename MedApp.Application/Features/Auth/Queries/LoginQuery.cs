using MedApp.Application.Common.DTOs;
using MediatR;

namespace MedApp.Application.Features.Auth.Queries;

public record LoginQuery(
    string Email,
    string Password
) : IRequest<AuthResponse>;