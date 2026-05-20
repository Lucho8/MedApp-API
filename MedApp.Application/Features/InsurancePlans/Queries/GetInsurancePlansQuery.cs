using MedApp.Application.Common.DTOs;
using MediatR;

namespace MedApp.Application.Features.InsurancePlans.Queries;

public record GetInsurancePlansQuery : IRequest<List<InsurancePlanDto>>;