using MedApp.Application.Common.DTOs;
using MedApp.Application.Common.Exceptions;
using MedApp.Domain.Interfaces;
using MediatR;

namespace MedApp.Application.Features.Clinics.Commands;

public class UpdateClinicCommandHandler : IRequestHandler<UpdateClinicCommand, ClinicDto>
{
    private readonly IUnitOfWork _uow;

    public UpdateClinicCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<ClinicDto> Handle(UpdateClinicCommand request, CancellationToken cancellationToken)
    {
        var clinics = await _uow.Clinics.FindAsync(c => c.Id == request.ClinicId);
        var clinic = clinics.FirstOrDefault()
            ?? throw new AppException("Clínica no encontrada.", 404);

        clinic.Name = request.Name;
        clinic.Address = request.Address;
        clinic.Phone = request.Phone;
        clinic.City = request.City;

        _uow.Clinics.Update(clinic);
        await _uow.SaveChangesAsync();

        return new ClinicDto(clinic.Id, clinic.Name, clinic.Address, clinic.Phone, clinic.City);
    }
}