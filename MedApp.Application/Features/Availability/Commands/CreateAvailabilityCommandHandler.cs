using MedApp.Application.Common.DTOs;
using MedApp.Application.Common.Exceptions;
using MedApp.Domain.Interfaces;
using MediatR;

namespace MedApp.Application.Features.Availability.Commands;

public class CreateAvailabilityCommandHandler : IRequestHandler<CreateAvailabilityCommand, AvailabilityDto>
{
    private readonly IUnitOfWork _uow;

    public CreateAvailabilityCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<AvailabilityDto> Handle(CreateAvailabilityCommand request, CancellationToken cancellationToken)
    {
        var doctors = await _uow.Doctors.FindAsync(d => d.Id == request.DoctorId);
        if (!doctors.Any())
            throw new AppException("Médico no encontrado.", 404);

        var clinics = await _uow.Clinics.FindAsync(c => c.Id == request.ClinicId);
        if (!clinics.Any())
            throw new AppException("Clínica no encontrada.", 404);

        var existing = await _uow.Availabilities.FindAsync(a =>
            a.DoctorId == request.DoctorId &&
            a.ClinicId == request.ClinicId &&
            a.DayOfWeek == request.DayOfWeek &&
            a.IsActive);

        if (existing.Any())
            throw new AppException("Ya existe disponibilidad para ese día en esa clínica.");

        var availability = new Domain.Entities.Availability
        {
            DoctorId = request.DoctorId,
            ClinicId = request.ClinicId,
            DayOfWeek = request.DayOfWeek,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            IsActive = true
        };

        await _uow.Availabilities.AddAsync(availability);
        await _uow.SaveChangesAsync();

        return new AvailabilityDto(
            availability.Id,
            availability.DoctorId,
            availability.ClinicId,
            availability.DayOfWeek.ToString(),
            availability.StartTime.ToString("HH:mm"),
            availability.EndTime.ToString("HH:mm"),
            availability.IsActive
        );
    }
}