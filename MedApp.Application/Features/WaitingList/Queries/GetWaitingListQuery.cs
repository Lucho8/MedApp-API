using MedApp.Application.Common.DTOs;
using MediatR;

namespace MedApp.Application.Features.WaitingList.Queries;

public record GetWaitingListQuery(Guid DoctorId, Guid ClinicId) : IRequest<List<WaitingListDto>>;