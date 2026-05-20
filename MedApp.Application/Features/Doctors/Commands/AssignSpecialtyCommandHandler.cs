using MedApp.Application.Common.Exceptions;
using MedApp.Domain.Entities;
using MedApp.Domain.Interfaces;
using MediatR;

namespace MedApp.Application.Features.Doctors.Commands;

public class AssignSpecialtyCommandHandler : IRequestHandler<AssignSpecialtyCommand, bool>
{
    private readonly IUnitOfWork _uow;

    public AssignSpecialtyCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<bool> Handle(AssignSpecialtyCommand request, CancellationToken cancellationToken)
    {
        var doctors = await _uow.Doctors.FindAsync(d => d.Id == request.DoctorId);
        if (!doctors.Any())
            throw new AppException("Médico no encontrado.", 404);

        var specialties = await _uow.Specialties.FindAsync(s => s.Id == request.SpecialtyId);
        if (!specialties.Any())
            throw new AppException("Especialidad no encontrada.", 404);

        var existing = await _uow.DoctorSpecialties.FindAsync(ds =>
            ds.DoctorId == request.DoctorId && ds.SpecialtyId == request.SpecialtyId);
        if (existing.Any())
            throw new AppException("El médico ya tiene esa especialidad.");

        await _uow.DoctorSpecialties.AddAsync(new DoctorSpecialty
        {
            DoctorId = request.DoctorId,
            SpecialtyId = request.SpecialtyId
        });

        await _uow.SaveChangesAsync();
        return true;
    }
}