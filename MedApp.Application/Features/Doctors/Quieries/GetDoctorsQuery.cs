using MedApp.Application.Common.DTOs;
using MediatR;

namespace MedApp.Application.Features.Doctors.Queries;

public record GetDoctorsQuery(string? Specialty = null) : IRequest<List<DoctorDto>>;