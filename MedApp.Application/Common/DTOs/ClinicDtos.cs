namespace MedApp.Application.Common.DTOs;

public record ClinicDto(
    Guid Id,
    string Name,
    string Address,
    string? Phone,
    string? City
);

public record CreateClinicRequest(
    string Name,
    string Address,
    string? Phone,
    string? City
);

public record UpdateClinicRequest(
    string Name,
    string Address,
    string? Phone,
    string? City
);