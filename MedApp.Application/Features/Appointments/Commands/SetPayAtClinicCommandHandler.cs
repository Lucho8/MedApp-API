using MedApp.Application.Common.Exceptions;
using MedApp.Domain.Enums;
using MedApp.Domain.Interfaces;
using MediatR;

namespace MedApp.Application.Features.Appointments.Commands;

public class SetPayAtClinicCommandHandler : IRequestHandler<SetPayAtClinicCommand, bool>
{
    private readonly IUnitOfWork _uow;

    public SetPayAtClinicCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<bool> Handle(SetPayAtClinicCommand request, CancellationToken cancellationToken)
    {
        var appointments = await _uow.Appointments.FindAsync(a => a.Id == request.AppointmentId);
        var appointment = appointments.FirstOrDefault()
            ?? throw new AppException("Turno no encontrado.", 404);

        appointment.PaymentStatus = PaymentStatus.PayAtClinic;
        _uow.Appointments.Update(appointment);
        await _uow.SaveChangesAsync();
        return true;
    }
}