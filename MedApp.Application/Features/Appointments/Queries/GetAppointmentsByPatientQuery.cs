using MedApp.Application.Common.DTOs;
using MediatR;

namespace MedApp.Application.Features.Appointments.Queries;

public record GetAppointmentsByPatientQuery(Guid PatientId) : IRequest<List<AppointmentDto>>;