using MediatR;

namespace MedApp.Application.Features.Specialties.Commands;

public record DeleteSpecialtyCommand(Guid SpecialtyId) : IRequest<bool>;