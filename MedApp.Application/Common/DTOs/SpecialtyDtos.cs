namespace MedApp.Application.Common.DTOs;

public record SpecialtyDto(Guid Id, string Name, string? Description);
public record CreateSpecialtyRequest(string Name, string? Description);
public record UpdateSpecialtyRequest(string Name, string? Description);