using MedApp.Application.Common.DTOs;
using MedApp.Domain.Interfaces;
using MediatR;

namespace MedApp.Application.Features.Clinics.Queries;

public class GetClinicByIdQueryHandler : IRequestHandler<GetClinicByIdQuery, ClinicDto?>
{
    private readonly IUnitOfWork _uow;

    public GetClinicByIdQueryHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<ClinicDto?> Handle(GetClinicByIdQuery request, CancellationToken cancellationToken)
    {
        var clinics = await _uow.Clinics.FindAsync(c => c.Id == request.Id && c.IsActive);
        var clinic = clinics.FirstOrDefault();
        if (clinic == null) return null;

        return new ClinicDto(clinic.Id, clinic.Name, clinic.Address, clinic.Phone, clinic.City);
    }
}