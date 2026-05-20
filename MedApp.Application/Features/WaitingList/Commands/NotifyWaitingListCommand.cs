using MediatR;

namespace MedApp.Application.Features.WaitingList.Commands;

public record NotifyWaitingListCommand(
    Guid DoctorId,
    Guid ClinicId,
    DateTime AvailableSlot
) : IRequest<int>;