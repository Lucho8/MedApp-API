using MedApp.Application.Common.Exceptions;
using MedApp.Domain.Interfaces;
using MediatR;

namespace MedApp.Application.Features.BlockedSlots.Commands;

public class DeleteBlockedSlotCommandHandler : IRequestHandler<DeleteBlockedSlotCommand, bool>
{
    private readonly IUnitOfWork _uow;

    public DeleteBlockedSlotCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<bool> Handle(DeleteBlockedSlotCommand request, CancellationToken cancellationToken)
    {
        var slots = await _uow.BlockedSlots.FindAsync(s => s.Id == request.SlotId);
        var slot = slots.FirstOrDefault()
            ?? throw new AppException("Bloqueo no encontrado.", 404);

        _uow.BlockedSlots.Remove(slot);
        await _uow.SaveChangesAsync();
        return true;
    }
}