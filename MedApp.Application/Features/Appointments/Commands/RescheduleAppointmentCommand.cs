using MedApp.Application.Common.DTOs;
using MediatR;

namespace MedApp.Application.Features.Appointments.Commands;

public record RescheduleAppointmentCommand(
    Guid AppointmentId,
    DateTime NewDateTime,
    Guid RequestedByUserId
) : IRequest<AppointmentDto>;