using MedApp.Application.Common.DTOs;
using MedApp.Domain.Interfaces;
using MediatR;

namespace MedApp.Application.Features.Availability.Queries;

public class GetAvailableSlotsQueryHandler : IRequestHandler<GetAvailableSlotsQuery, List<AvailableSlotDto>>
{
    private readonly IUnitOfWork _uow;

    public GetAvailableSlotsQueryHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<List<AvailableSlotDto>> Handle(GetAvailableSlotsQuery request, CancellationToken cancellationToken)
    {
        // 1. Buscar disponibilidad del médico ese día de la semana
        var dayOfWeek = request.Date.DayOfWeek;
        var availabilities = await _uow.Availabilities.FindAsync(a =>
            a.DoctorId == request.DoctorId &&
            a.ClinicId == request.ClinicId &&
            a.DayOfWeek == dayOfWeek &&
            a.IsActive);

        if (!availabilities.Any())
            return [];

        var availability = availabilities.First();

        // 2. Verificar que no esté bloqueado ese día
        var blocked = await _uow.BlockedSlots.FindAsync(b =>
            b.DoctorId == request.DoctorId &&
            b.ClinicId == request.ClinicId &&
            b.Date == request.Date);

        // Si el día entero está bloqueado (sin StartTime/EndTime específico)
        if (blocked.Any(b => b.StartTime == null && b.EndTime == null))
            return [];

        // 3. Traer los turnos ya reservados ese día
        var dateStart = request.Date.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        var dateEnd = dateStart.AddDays(1);

        var appointments = await _uow.Appointments.FindAsync(a =>
            a.DoctorId == request.DoctorId &&
            a.ClinicId == request.ClinicId &&
            a.DateTime >= dateStart &&
            a.DateTime < dateEnd &&
            a.Status != Domain.Enums.AppointmentStatus.Cancelled);

        // 4. Traer el médico para saber la duración del turno
        var doctors = await _uow.Doctors.FindAsync(d => d.Id == request.DoctorId);
        var doctor = doctors.First();
        var duration = doctor.AppointmentDurationMinutes;

        // 5. Generar todos los slots posibles y filtrar los ocupados
        var slots = new List<AvailableSlotDto>();
        var current = request.Date.ToDateTime(availability.StartTime, DateTimeKind.Utc);
        var end = request.Date.ToDateTime(availability.EndTime, DateTimeKind.Utc);

        while (current.AddMinutes(duration) <= end)
        {
            var slotEnd = current.AddMinutes(duration);

            // Verificar que no haya un turno que se superponga
            var isTaken = appointments.Any(a =>
                a.DateTime < slotEnd && a.DateTime.AddMinutes(duration) > current);

            // Verificar que no esté en un bloqueo parcial
            var isBlocked = blocked.Any(b =>
                b.StartTime != null && b.EndTime != null &&
                current.TimeOfDay < b.EndTime.Value.ToTimeSpan() &&
                slotEnd.TimeOfDay > b.StartTime.Value.ToTimeSpan());

            if (!isTaken && !isBlocked)
                slots.Add(new AvailableSlotDto(current, slotEnd));

            current = current.AddMinutes(duration);
        }

        return slots;
    }
}