using MedApp.Application.Common.Exceptions;
using MedApp.Domain.Entities;
using MedApp.Domain.Interfaces;
using MediatR;

namespace MedApp.Application.Features.Doctors.Commands;

public class AssignInsurancePlanCommandHandler : IRequestHandler<AssignInsurancePlanCommand, bool>
{
    private readonly IUnitOfWork _uow;

    public AssignInsurancePlanCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<bool> Handle(AssignInsurancePlanCommand request, CancellationToken cancellationToken)
    {
        var doctors = await _uow.Doctors.FindAsync(d => d.Id == request.DoctorId);
        if (!doctors.Any())
            throw new AppException("Médico no encontrado.", 404);

        var plans = await _uow.InsurancePlans.FindAsync(p => p.Id == request.InsurancePlanId);
        if (!plans.Any())
            throw new AppException("Obra social no encontrada.", 404);

        var existing = await _uow.DoctorInsurancePlans.FindAsync(di =>
            di.DoctorId == request.DoctorId && di.InsurancePlanId == request.InsurancePlanId);
        if (existing.Any())
            throw new AppException("El médico ya tiene esa obra social.");

        await _uow.DoctorInsurancePlans.AddAsync(new DoctorInsurancePlan
        {
            DoctorId = request.DoctorId,
            InsurancePlanId = request.InsurancePlanId
        });

        await _uow.SaveChangesAsync();
        return true;
    }
}