using MedApp.Application.Common.DTOs;
using MediatR;

namespace MedApp.Application.Features.Availability.Commands;

public record CreateAvailabilityCommand(
    Guid DoctorId,
    Guid ClinicId,
    DayOfWeek DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime
) : IRequest<AvailabilityDto>;