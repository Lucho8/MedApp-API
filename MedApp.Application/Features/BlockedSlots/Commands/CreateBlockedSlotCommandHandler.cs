using MedApp.Application.Common.DTOs;
using MedApp.Domain.Interfaces;
using MediatR;

namespace MedApp.Application.Features.BlockedSlots.Commands;

public class CreateBlockedSlotCommandHandler : IRequestHandler<CreateBlockedSlotCommand, BlockedSlotDto>
{
    private readonly IUnitOfWork _uow;

    public CreateBlockedSlotCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<BlockedSlotDto> Handle(CreateBlockedSlotCommand request, CancellationToken cancellationToken)
    {
        var slot = new Domain.Entities.BlockedSlot
        {
            DoctorId = request.DoctorId,
            ClinicId = request.ClinicId,
            Date = request.Date,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Reason = request.Reason
        };

        await _uow.BlockedSlots.AddAsync(slot);
        await _uow.SaveChangesAsync();

        return new BlockedSlotDto(
            slot.Id, slot.DoctorId, slot.ClinicId, slot.Date,
            slot.StartTime?.ToString("HH:mm"),
            slot.EndTime?.ToString("HH:mm"),
            slot.Reason);
    }
}