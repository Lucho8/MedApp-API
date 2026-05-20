using MediatR;

namespace MedApp.Application.Features.Clinics.Commands;

public record DeleteClinicCommand(Guid ClinicId) : IRequest<bool>;