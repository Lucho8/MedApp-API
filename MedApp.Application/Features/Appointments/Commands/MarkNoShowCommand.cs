using MedApp.Application.Common.DTOs;
using MediatR;

namespace MedApp.Application.Features.Appointments.Commands;

public record MarkNoShowCommand(Guid AppointmentId, Guid RequestedByUserId) : IRequest<AppointmentDto>;