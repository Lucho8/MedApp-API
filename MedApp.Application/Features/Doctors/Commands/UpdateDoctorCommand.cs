using MedApp.Application.Common.DTOs;
using MediatR;

namespace MedApp.Application.Features.Doctors.Commands;

public record UpdateDoctorCommand(
    Guid DoctorId,
    string FirstName,
    string LastName,
    string? Bio,
    string? PhotoUrl,
    int AppointmentDurationMinutes,
    decimal ConsultationPrice
) : IRequest<DoctorDto>;