using MediatR;

namespace MedApp.Application.Features.Doctors.Commands;

public record DeleteDoctorCommand(Guid DoctorId) : IRequest<bool>;