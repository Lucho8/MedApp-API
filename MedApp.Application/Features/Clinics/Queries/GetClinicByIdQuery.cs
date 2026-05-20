using MedApp.Application.Common.DTOs;
using MediatR;

namespace MedApp.Application.Features.Clinics.Queries;

public record GetClinicByIdQuery(Guid Id) : IRequest<ClinicDto?>;