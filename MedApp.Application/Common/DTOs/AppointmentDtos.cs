using MedApp.Domain.Enums;

namespace MedApp.Application.Common.DTOs;

public record AppointmentDto(
    Guid Id,
    Guid DoctorId,
    string DoctorName,
    Guid PatientId,
    string PatientName,
    Guid ClinicId,
    string ClinicName,
    DateTime DateTime,
    int DurationMinutes,
    string Status,
    string PaymentStatus,
    string? Notes,
    string? CancellationReason
);

public record CreateAppointmentRequest(
    Guid DoctorId,
    Guid ClinicId,
    DateTime DateTime,
    // Datos del paciente — si tiene cuenta se ignoran
    string? FirstName,
    string? LastName,
    string? Email,
    string? Phone,
    string? DNI
);

public record CancelAppointmentRequest(
    string Reason
);