namespace MedApp.Application.Common.DTOs;

public record InsurancePlanDto(Guid Id, string Name);
public record CreateInsurancePlanRequest(string Name);