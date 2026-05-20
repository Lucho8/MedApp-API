using MedApp.Application.Common.DTOs;
using MedApp.Domain.Interfaces;
using MediatR;

namespace MedApp.Application.Features.Clinics.Queries;

public class GetClinicsQueryHandler : IRequestHandler<GetClinicsQuery, List<ClinicDto>>
{
    private readonly IUnitOfWork _uow;

    public GetClinicsQueryHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<List<ClinicDto>> Handle(GetClinicsQuery request, CancellationToken cancellationToken)
    {
        var clinics = await _uow.Clinics.FindAsync(c => c.IsActive);
        return clinics.Select(c => new ClinicDto(
            c.Id, c.Name, c.Address, c.Phone, c.City
        )).ToList();
    }
}