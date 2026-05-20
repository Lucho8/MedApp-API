using MedApp.Application.Common.DTOs;
using MedApp.Domain.Interfaces;
using MediatR;

namespace MedApp.Application.Features.Availability.Queries;

public class GetAvailabilityQueryHandler : IRequestHandler<GetAvailabilityQuery, List<AvailabilityDto>>
{
    private readonly IUnitOfWork _uow;

    public GetAvailabilityQueryHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<List<AvailabilityDto>> Handle(GetAvailabilityQuery request, CancellationToken cancellationToken)
    {
        var availabilities = await _uow.Availabilities.FindAsync(a =>
            a.DoctorId == request.DoctorId &&
            a.ClinicId == request.ClinicId &&
            a.IsActive);

        return availabilities.Select(a => new AvailabilityDto(
            a.Id,
            a.DoctorId,
            a.ClinicId,
            a.DayOfWeek.ToString(),
            a.StartTime.ToString("HH:mm"),
            a.EndTime.ToString("HH:mm"),
            a.IsActive
        )).ToList();
    }
}