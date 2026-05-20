using MedApp.Application.Common.DTOs;
using MediatR;

namespace MedApp.Application.Features.Dashboard.Queries;

public record GetDashboardQuery(Guid DoctorId) : IRequest<DashboardDto>;