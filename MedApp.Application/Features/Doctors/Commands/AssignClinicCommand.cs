using MediatR;

namespace MedApp.Application.Features.Doctors.Commands;

public record AssignClinicCommand(
    Guid DoctorId,
    Guid ClinicId
) : IRequest<bool>;