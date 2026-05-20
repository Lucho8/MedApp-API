using MediatR;

namespace MedApp.Application.Features.BlockedSlots.Commands;

public record DeleteBlockedSlotCommand(Guid SlotId) : IRequest<bool>;