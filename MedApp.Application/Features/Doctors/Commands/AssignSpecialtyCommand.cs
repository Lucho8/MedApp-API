using MediatR;

namespace MedApp.Application.Features.Doctors.Commands;

public record AssignSpecialtyCommand(
    Guid DoctorId,
    Guid SpecialtyId
) : IRequest<bool>;