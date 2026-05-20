using MedApp.Application.Common.DTOs;
using MedApp.Domain.Entities;
using MedApp.Domain.Interfaces;
using MediatR;

namespace MedApp.Application.Features.Clinics.Commands;

public class CreateClinicCommandHandler : IRequestHandler<CreateClinicCommand, ClinicDto>
{
    private readonly IUnitOfWork _uow;

    public CreateClinicCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<ClinicDto> Handle(CreateClinicCommand request, CancellationToken cancellationToken)
    {
        var clinic = new Clinic
        {
            Name = request.Name,
            Address = request.Address,
            Phone = request.Phone,
            City = request.City
        };

        await _uow.Clinics.AddAsync(clinic);
        await _uow.SaveChangesAsync();

        return new ClinicDto(clinic.Id, clinic.Name, clinic.Address, clinic.Phone, clinic.City);
    }
}