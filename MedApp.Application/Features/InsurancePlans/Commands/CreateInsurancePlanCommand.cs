using MedApp.Application.Common.DTOs;
using MediatR;

namespace MedApp.Application.Features.InsurancePlans.Commands;

public record CreateInsurancePlanCommand(string Name) : IRequest<InsurancePlanDto>;