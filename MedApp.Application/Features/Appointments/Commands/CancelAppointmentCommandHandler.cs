using MedApp.Application.Common.DTOs;
using MedApp.Application.Common.Exceptions;
using MedApp.Application.Common.Interfaces;
using MedApp.Domain.Enums;
using MedApp.Domain.Interfaces;
using MediatR;

namespace MedApp.Application.Features.Appointments.Commands;

public class CancelAppointmentCommandHandler : IRequestHandler<CancelAppointmentCommand, AppointmentDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IEmailService _emailService;
        private readonly INotificationService _notificationService;

        public CancelAppointmentCommandHandler(IUnitOfWork uow, IEmailService emailService, INotificationService notificationService)
        {
            _uow = uow;
            _emailService = emailService;
            _notificationService = notificationService;
        }   

    public async Task<AppointmentDto> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointments = await _uow.Appointments.FindAsync(a => a.Id == request.AppointmentId);
        var appointment = appointments.FirstOrDefault()
            ?? throw new AppException("Turno no encontrado.", 404);

        if (appointment.Status == AppointmentStatus.Cancelled)
            throw new AppException("El turno ya está cancelado.");

        if (appointment.Status == AppointmentStatus.Completed)
            throw new AppException("No se puede cancelar un turno completado.");

        appointment.Status = AppointmentStatus.Cancelled;
        appointment.CancellationReason = request.Reason;

        _uow.Appointments.Update(appointment);

        // AuditLog
        await _uow.AuditLogs.AddAsync(new Domain.Entities.AuditLog
        {
            UserId = request.RequestedByUserId,
            Action = "Cancelled",
            EntityType = "Appointment",
            EntityId = appointment.Id,
            Details = $"{{\"reason\": \"{request.Reason}\"}}"
        });

        await _uow.SaveChangesAsync();

        var doctors = await _uow.Doctors.FindAsync(d => d.Id == appointment.DoctorId);
        var doctor = doctors.First();
        var patients = await _uow.Patients.FindAsync(p => p.Id == appointment.PatientId);
        var patient = patients.First();
        var clinics = await _uow.Clinics.FindAsync(c => c.Id == appointment.ClinicId);
        var clinic = clinics.First();

        await _emailService.SendAppointmentCancellationAsync(
            patient.Email,
            $"{patient.FirstName} {patient.LastName}",
            $"{doctor.FirstName} {doctor.LastName}",
            appointment.DateTime,
            request.Reason);


        await _notificationService.NotifyAppointmentCancelledAsync(
            appointment.DoctorId.ToString(),
            new { appointment.Id, appointment.DateTime, appointment.CancellationReason });

        return new AppointmentDto(
            appointment.Id,
            doctor.Id,
            $"{doctor.FirstName} {doctor.LastName}",
            patient.Id,
            $"{patient.FirstName} {patient.LastName}",
            clinic.Id,
            clinic.Name,
            appointment.DateTime,
            appointment.DurationMinutes,
            appointment.Status.ToString(),
            appointment.PaymentStatus.ToString(),
            appointment.Notes,
            appointment.CancellationReason
        );
    }
}