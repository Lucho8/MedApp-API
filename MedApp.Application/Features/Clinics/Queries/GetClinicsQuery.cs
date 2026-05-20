using MedApp.Application.Common.DTOs;
using MediatR;

namespace MedApp.Application.Features.Clinics.Queries;

public record GetClinicsQuery : IRequest<List<ClinicDto>>;