using MedApp.Application.Common.Exceptions;
using MedApp.Domain.Interfaces;
using MediatR;

namespace MedApp.Application.Features.Doctors.Commands;

public class DeleteDoctorCommandHandler : IRequestHandler<DeleteDoctorCommand, bool>
{
    private readonly IUnitOfWork _uow;

    public DeleteDoctorCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<bool> Handle(DeleteDoctorCommand request, CancellationToken cancellationToken)
    {
        var doctors = await _uow.Doctors.FindAsync(d => d.Id == request.DoctorId);
        var doctor = doctors.FirstOrDefault()
            ?? throw new AppException("Médico no encontrado.", 404);

        doctor.IsActive = false;
        _uow.Doctors.Update(doctor);
        await _uow.SaveChangesAsync();

        return true;
    }
}