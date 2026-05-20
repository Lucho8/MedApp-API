using MedApp.Application.Common.DTOs;
using MediatR;

namespace MedApp.Application.Features.Clinics.Commands;

public record CreateClinicCommand(
    string Name,
    string Address,
    string? Phone,
    string? City
) : IRequest<ClinicDto>;