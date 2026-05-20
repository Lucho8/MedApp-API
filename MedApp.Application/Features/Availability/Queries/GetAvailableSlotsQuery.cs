using MedApp.Application.Common.DTOs;
using MediatR;

namespace MedApp.Application.Features.Availability.Queries;

public record GetAvailableSlotsQuery(
    Guid DoctorId,
    Guid ClinicId,
    DateOnly Date
) : IRequest<List<AvailableSlotDto>>;