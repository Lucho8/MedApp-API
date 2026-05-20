namespace MedApp.Application.Common.DTOs;

public record BlockedSlotDto(
    Guid Id,
    Guid DoctorId,
    Guid ClinicId,
    DateOnly Date,
    string? StartTime,
    string? EndTime,
    string? Reason
);

public record CreateBlockedSlotRequest(
    Guid ClinicId,
    DateOnly Date,
    TimeOnly? StartTime,
    TimeOnly? EndTime,
    string? Reason
);