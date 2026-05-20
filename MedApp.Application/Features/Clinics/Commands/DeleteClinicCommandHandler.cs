using MedApp.Application.Common.Exceptions;
using MedApp.Domain.Interfaces;
using MediatR;

namespace MedApp.Application.Features.Clinics.Commands;

public class DeleteClinicCommandHandler : IRequestHandler<DeleteClinicCommand, bool>
{
    private readonly IUnitOfWork _uow;

    public DeleteClinicCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<bool> Handle(DeleteClinicCommand request, CancellationToken cancellationToken)
    {
        var clinics = await _uow.Clinics.FindAsync(c => c.Id == request.ClinicId);
        var clinic = clinics.FirstOrDefault()
            ?? throw new AppException("Clínica no encontrada.", 404);

        clinic.IsActive = false;
        _uow.Clinics.Update(clinic);
        await _uow.SaveChangesAsync();

        return true;
    }
}