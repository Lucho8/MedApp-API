using MedApp.Application.Common.DTOs;
using MedApp.Domain.Interfaces;
using MediatR;

namespace MedApp.Application.Features.InsurancePlans.Queries;

public class GetInsurancePlansQueryHandler : IRequestHandler<GetInsurancePlansQuery, List<InsurancePlanDto>>
{
    private readonly IUnitOfWork _uow;

    public GetInsurancePlansQueryHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<List<InsurancePlanDto>> Handle(GetInsurancePlansQuery request, CancellationToken cancellationToken)
    {
        var plans = await _uow.InsurancePlans.FindAsync(p => p.IsActive);
        return plans.Select(p => new InsurancePlanDto(p.Id, p.Name)).ToList();
    }
}