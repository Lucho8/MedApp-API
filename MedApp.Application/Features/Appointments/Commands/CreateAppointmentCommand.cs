using MedApp.Application.Common.DTOs;
using MediatR;

namespace MedApp.Application.Features.Appointments.Commands;

public record CreateAppointmentCommand(
    Guid DoctorId,
    Guid ClinicId,
    DateTime DateTime,
    string? FirstName,
    string? LastName,
    string? Email,
    string? Phone,
    string? DNI,
    Guid? RegisteredPatientUserId
) : IRequest<AppointmentDto>;