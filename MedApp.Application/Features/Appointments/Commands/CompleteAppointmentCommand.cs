using MedApp.Application.Common.DTOs;
using MediatR;

namespace MedApp.Application.Features.Appointments.Commands;

public record CompleteAppointmentCommand(Guid AppointmentId, Guid RequestedByUserId) : IRequest<AppointmentDto>;