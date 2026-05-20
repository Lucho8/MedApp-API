using MedApp.Application.Common.DTOs;
using MediatR;

namespace MedApp.Application.Features.Appointments.Commands;

public record CancelAppointmentCommand(
    Guid AppointmentId,
    Guid RequestedByUserId,
    string Reason
) : IRequest<AppointmentDto>;