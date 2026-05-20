namespace MedApp.Application.Common.DTOs;

public record AvailabilityDto(
    Guid Id,
    Guid DoctorId,
    Guid ClinicId,
    string DayOfWeek,
    string StartTime,
    string EndTime,
    bool IsActive
);

public record CreateAvailabilityRequest(
    Guid ClinicId,
    DayOfWeek DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime
);

public record AvailableSlotDto(
    DateTime Start,
    DateTime End
);