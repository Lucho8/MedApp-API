using MedApp.Application.Common.DTOs;
using MediatR;

namespace MedApp.Application.Features.Specialties.Commands;

public record CreateSpecialtyCommand(string Name, string? Description) : IRequest<SpecialtyDto>;