using MedApp.Application.Common.DTOs;
using MediatR;

namespace MedApp.Application.Features.BlockedSlots.Commands;

public record CreateBlockedSlotCommand(
    Guid DoctorId,
    Guid ClinicId,
    DateOnly Date,
    TimeOnly? StartTime,
    TimeOnly? EndTime,
    string? Reason
) : IRequest<BlockedSlotDto>;