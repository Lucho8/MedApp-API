using MedApp.Application.Common.DTOs;
using MedApp.Application.Common.Exceptions;
using MedApp.Application.Common.Interfaces;
using MedApp.Domain.Entities;
using MedApp.Domain.Enums;
using MedApp.Domain.Interfaces;

using MediatR;

namespace MedApp.Application.Features.Appointments.Commands;

public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, AppointmentDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IEmailService _emailService;
    private readonly INotificationService _notificationService;

    public CreateAppointmentCommandHandler(IUnitOfWork uow, IEmailService emailService, INotificationService notificationService)
        {
            _uow = uow;
            _emailService = emailService;
            _notificationService = notificationService;
        }


    public async Task<AppointmentDto> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        // 1. Verificar que el médico existe
        var doctors = await _uow.Doctors.FindAsync(d => d.Id == request.DoctorId && d.IsActive);
        var doctor = doctors.FirstOrDefault()
            ?? throw new AppException("Médico no encontrado.", 404);

        // 2. Verificar que la clínica existe
        var clinics = await _uow.Clinics.FindAsync(c => c.Id == request.ClinicId && c.IsActive);
        var clinic = clinics.FirstOrDefault()
            ?? throw new AppException("Clínica no encontrada.", 404);

        // 3. Verificar que el slot está disponible
        var slotEnd = request.DateTime.AddMinutes(doctor.AppointmentDurationMinutes);
        var existing = await _uow.Appointments.FindAsync(a =>
            a.DoctorId == request.DoctorId &&
            a.ClinicId == request.ClinicId &&
            a.Status != AppointmentStatus.Cancelled &&
            a.DateTime < slotEnd &&
            a.DateTime.AddMinutes(doctor.AppointmentDurationMinutes) > request.DateTime);

        if (existing.Any())
            throw new AppException("Ese horario ya está ocupado.");

        // 4. Resolver el paciente — registrado o guest
        Patient patient;

        if (request.RegisteredPatientUserId.HasValue)
        {
            var patients = await _uow.Patients.FindAsync(p => p.UserId == request.RegisteredPatientUserId);
            patient = patients.FirstOrDefault()
                ?? throw new AppException("Paciente no encontrado.", 404);
        }
        else
        {
            if (string.IsNullOrEmpty(request.Email))
                throw new AppException("El email es requerido para reservar sin cuenta.");

            
            var existingPatients = await _uow.Patients.FindAsync(p =>
                p.Email == request.Email && p.UserId == null);

            patient = existingPatients.FirstOrDefault()!;

            if (patient == null)
            {
                patient = new Patient
                {
                    FirstName = request.FirstName ?? "",
                    LastName = request.LastName ?? "",
                    Email = request.Email,
                    Phone = request.Phone,
                    DNI = request.DNI
                };
                await _uow.Patients.AddAsync(patient);
            }
        }

        // 5. Crear el turno
        var appointment = new Appointment
        {
            DoctorId = request.DoctorId,
            PatientId = patient.Id,
            ClinicId = request.ClinicId,
            DateTime = request.DateTime,
            DurationMinutes = doctor.AppointmentDurationMinutes,
            Status = request.RegisteredPatientUserId.HasValue
                ? AppointmentStatus.Confirmed
                : AppointmentStatus.Pending,
            PaymentStatus = PaymentStatus.Unpaid,
            CreatedByUserId = request.RegisteredPatientUserId ?? Guid.Empty
        };

        await _uow.Appointments.AddAsync(appointment);
        await _uow.SaveChangesAsync();

        await _emailService.SendAppointmentConfirmationAsync(
            patient.Email,
            $"{patient.FirstName} {patient.LastName}",
            $"{doctor.FirstName} {doctor.LastName}",
            clinic.Name,
            appointment.DateTime);

        await _notificationService.NotifyNewAppointmentAsync(
            doctor.Id.ToString(),
            new { appointment.Id, appointment.DateTime, PatientName = $"{patient.FirstName} {patient.LastName}" });


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