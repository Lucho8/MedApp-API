using MedApp.Application.Common.DTOs;
using MediatR;

namespace MedApp.Application.Features.WaitingList.Commands;

public record JoinWaitingListCommand(
    Guid DoctorId,
    Guid ClinicId,
    DateOnly? PreferredDateFrom,
    DateOnly? PreferredDateTo,
    string? FirstName,
    string? LastName,
    string? Email,
    string? Phone,
    string? DNI,
    Guid? RegisteredPatientUserId
) : IRequest<WaitingListDto>;