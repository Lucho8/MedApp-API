namespace MedApp.Application.Common.DTOs;

public record DoctorDto(
    Guid Id,
    string FirstName,
    string LastName,
    string? Bio,
    string? PhotoUrl,
    int AppointmentDurationMinutes,
    decimal ConsultationPrice,
    List<string> Specialties,
    List<string> Clinics,
    List<string> InsurancePlans
);

public record UpdateDoctorRequest(
    string FirstName,
    string LastName,
    string? Bio,
    string? PhotoUrl,
    int AppointmentDurationMinutes,
    decimal ConsultationPrice
);