using MedApp.Application.Common.Exceptions;
using MedApp.Domain.Interfaces;
using MediatR;

namespace MedApp.Application.Features.Availability.Commands;

public class DeleteAvailabilityCommandHandler : IRequestHandler<DeleteAvailabilityCommand, bool>
{
    private readonly IUnitOfWork _uow;

    public DeleteAvailabilityCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<bool> Handle(DeleteAvailabilityCommand request, CancellationToken cancellationToken)
    {
        var availabilities = await _uow.Availabilities.FindAsync(a => a.Id == request.AvailabilityId);
        var availability = availabilities.FirstOrDefault()
            ?? throw new AppException("Disponibilidad no encontrada.", 404);

        availability.IsActive = false;
        _uow.Availabilities.Update(availability);
        await _uow.SaveChangesAsync();

        return true;
    }
}