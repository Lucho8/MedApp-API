using MedApp.Application.Common.DTOs;
using MedApp.Domain.Entities;
using MedApp.Domain.Interfaces;
using MediatR;

namespace MedApp.Application.Features.InsurancePlans.Commands;

public class CreateInsurancePlanCommandHandler : IRequestHandler<CreateInsurancePlanCommand, InsurancePlanDto>
{
    private readonly IUnitOfWork _uow;

    public CreateInsurancePlanCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<InsurancePlanDto> Handle(CreateInsurancePlanCommand request, CancellationToken cancellationToken)
    {
        var plan = new InsurancePlan { Name = request.Name };
        await _uow.InsurancePlans.AddAsync(plan);
        await _uow.SaveChangesAsync();
        return new InsurancePlanDto(plan.Id, plan.Name);
    }
}