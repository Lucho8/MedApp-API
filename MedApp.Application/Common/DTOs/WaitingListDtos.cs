namespace MedApp.Application.Common.DTOs;

public record WaitingListDto(
    Guid Id,
    Guid DoctorId,
    string DoctorName,
    Guid PatientId,
    string PatientName,
    Guid ClinicId,
    string ClinicName,
    DateOnly? PreferredDateFrom,
    DateOnly? PreferredDateTo,
    string Status,
    DateTime CreatedAt
);

public record JoinWaitingListRequest(
    Guid DoctorId,
    Guid ClinicId,
    DateOnly? PreferredDateFrom,
    DateOnly? PreferredDateTo,
    string? FirstName,
    string? LastName,
    string? Email,
    string? Phone,
    string? DNI
);

public record NotifyWaitingListRequest(
    Guid DoctorId,
    Guid ClinicId,
    DateTime AvailableSlot
);