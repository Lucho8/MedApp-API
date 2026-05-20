using MedApp.Application.Common.DTOs;
using MediatR;

namespace MedApp.Application.Features.Specialties.Queries;

public record GetSpecialtiesQuery : IRequest<List<SpecialtyDto>>;