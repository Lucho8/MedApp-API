using MedApp.Application.Common.DTOs;
using MediatR;

namespace MedApp.Application.Features.Doctors.Queries;

public record GetDoctorByIdQuery(Guid Id) : IRequest<DoctorDto?>;