using MedApp.Application.Common.DTOs;
using MediatR;

namespace MedApp.Application.Features.Availability.Queries;

public record GetAvailabilityQuery(Guid DoctorId, Guid ClinicId) : IRequest<List<AvailabilityDto>>;