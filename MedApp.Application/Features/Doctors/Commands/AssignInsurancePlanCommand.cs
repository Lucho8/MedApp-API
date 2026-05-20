using MediatR;

namespace MedApp.Application.Features.Doctors.Commands;

public record AssignInsurancePlanCommand(Guid DoctorId, Guid InsurancePlanId) : IRequest<bool>;