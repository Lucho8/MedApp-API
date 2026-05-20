using MedApp.Application.Common.Exceptions;
using MedApp.Domain.Entities;
using MedApp.Domain.Interfaces;
using MediatR;

namespace MedApp.Application.Features.Doctors.Commands;

public class AssignClinicCommandHandler : IRequestHandler<AssignClinicCommand, bool>
{
    private readonly IUnitOfWork _uow;

    public AssignClinicCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<bool> Handle(AssignClinicCommand request, CancellationToken cancellationToken)
    {
        var doctors = await _uow.Doctors.FindAsync(d => d.Id == request.DoctorId);
        if (!doctors.Any())
            throw new AppException("Médico no encontrado.", 404);

        var clinics = await _uow.Clinics.FindAsync(c => c.Id == request.ClinicId);
        if (!clinics.Any())
            throw new AppException("Clínica no encontrada.", 404);

        var existing = await _uow.DoctorClinics.FindAsync(dc =>
            dc.DoctorId == request.DoctorId && dc.ClinicId == request.ClinicId);
        if (existing.Any())
            throw new AppException("El médico ya está asignado a esa clínica.");

        await _uow.DoctorClinics.AddAsync(new DoctorClinic
        {
            DoctorId = request.DoctorId,
            ClinicId = request.ClinicId
        });

        await _uow.SaveChangesAsync();
        return true;
    }
}