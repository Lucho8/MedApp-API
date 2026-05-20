using MedApp.Application.Common.DTOs;
using MedApp.Application.Common.Exceptions;
using MedApp.Domain.Enums;
using MedApp.Domain.Interfaces;
using MediatR;

namespace MedApp.Application.Features.Appointments.Commands;

public class CompleteAppointmentCommandHandler : IRequestHandler<CompleteAppointmentCommand, AppointmentDto>
{
    private readonly IUnitOfWork _uow;

    public CompleteAppointmentCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<AppointmentDto> Handle(CompleteAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointments = await _uow.Appointments.FindAsync(a => a.Id == request.AppointmentId);
        var appointment = appointments.FirstOrDefault()
            ?? throw new AppException("Turno no encontrado.", 404);

        if (appointment.Status == AppointmentStatus.Cancelled)
            throw new AppException("No se puede completar un turno cancelado.");

        appointment.Status = AppointmentStatus.Completed;
        _uow.Appointments.Update(appointment);

        await _uow.AuditLogs.AddAsync(new Domain.Entities.AuditLog
        {
            UserId = request.RequestedByUserId,
            Action = "Completed",
            EntityType = "Appointment",
            EntityId = appointment.Id,
            Details = "{}"
        });

        await _uow.SaveChangesAsync();

        var doctors = await _uow.Doctors.FindAsync(d => d.Id == appointment.DoctorId);
        var doctor = doctors.First();
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