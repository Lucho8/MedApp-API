using MedApp.Application.Common.DTOs;
using MedApp.Application.Common.Exceptions;
using MedApp.Domain.Enums;
using MedApp.Domain.Interfaces;
using MediatR;

namespace MedApp.Application.Features.Appointments.Commands;

public class RescheduleAppointmentCommandHandler : IRequestHandler<RescheduleAppointmentCommand, AppointmentDto>
{
    private readonly IUnitOfWork _uow;

    public RescheduleAppointmentCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<AppointmentDto> Handle(RescheduleAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointments = await _uow.Appointments.FindAsync(a => a.Id == request.AppointmentId);
        var appointment = appointments.FirstOrDefault()
            ?? throw new AppException("Turno no encontrado.", 404);

        if (appointment.Status == AppointmentStatus.Cancelled)
            throw new AppException("No se puede reprogramar un turno cancelado.");

        if (appointment.Status == AppointmentStatus.Completed)
            throw new AppException("No se puede reprogramar un turno completado.");

        // Verificar que el nuevo slot esté libre
        var doctors = await _uow.Doctors.FindAsync(d => d.Id == appointment.DoctorId);
        var doctor = doctors.First();
        var slotEnd = request.NewDateTime.AddMinutes(doctor.AppointmentDurationMinutes);

        var conflict = await _uow.Appointments.FindAsync(a =>
            a.Id != request.AppointmentId &&
            a.DoctorId == appointment.DoctorId &&
            a.ClinicId == appointment.ClinicId &&
            a.Status != AppointmentStatus.Cancelled &&
            a.DateTime < slotEnd &&
            a.DateTime.AddMinutes(doctor.AppointmentDurationMinutes) > request.NewDateTime);

        if (conflict.Any())
            throw new AppException("El nuevo horario ya está ocupado.");

        var oldDateTime = appointment.DateTime;
        appointment.DateTime = request.NewDateTime;
        appointment.Status = AppointmentStatus.Confirmed;
        _uow.Appointments.Update(appointment);

        await _uow.AuditLogs.AddAsync(new Domain.Entities.AuditLog
        {
            UserId = request.RequestedByUserId,
            Action = "Rescheduled",
            EntityType = "Appointment",
            EntityId = appointment.Id,
            Details = $"{{\"from\": \"{oldDateTime:o}\", \"to\": \"{request.NewDateTime:o}\"}}"
        });

        await _uow.SaveChangesAsync();

        var patients = await _uow.Patients.FindAsync(p => p.Id == appointment.PatientId);
        var patient = patients.First();
        var clinics = await _uow.Clinics.FindAsync(c => c.Id == appointment.ClinicId);
        var clinic = clinics.First();

        return new AppointmentDto(
            appointment.Id, doctor.Id, $"{doctor.FirstName} {doctor.LastName}",
            patient.Id, $"{patient.FirstName} {patient.LastName}",
            clinic.Id, clinic.Name, appointment.DateTime, appointment.DurationMinutes,
            appointment.Status.ToString(), appointment.PaymentStatus.ToString(),
            appointment.Notes, appointment.CancellationReason);
    }
}