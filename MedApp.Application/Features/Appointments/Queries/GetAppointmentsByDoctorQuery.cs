using MedApp.Application.Common.DTOs;
using MediatR;

namespace MedApp.Application.Features.Appointments.Queries;

public record GetAppointmentsByDoctorQuery(
    Guid DoctorId,
    DateOnly? Date = null
) : IRequest<List<AppointmentDto>>;