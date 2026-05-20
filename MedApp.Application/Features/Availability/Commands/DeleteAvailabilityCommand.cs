using MediatR;

namespace MedApp.Application.Features.Availability.Commands;

public record DeleteAvailabilityCommand(Guid AvailabilityId) : IRequest<bool>;