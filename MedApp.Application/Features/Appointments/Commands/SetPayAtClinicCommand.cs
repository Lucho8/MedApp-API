using MediatR;

namespace MedApp.Application.Features.Appointments.Commands;

public record SetPayAtClinicCommand(Guid AppointmentId) : IRequest<bool>;