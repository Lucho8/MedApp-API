using MedApp.Application.Common.DTOs;
using MedApp.Application.Features.InsurancePlans.Commands;
using MedApp.Application.Features.InsurancePlans.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InsurancePlansController : ControllerBase
{
    private readonly IMediator _mediator;

    public InsurancePlansController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetInsurancePlansQuery());
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> Create(CreateInsurancePlanRequest request)
    {
        var result = await _mediator.Send(new CreateInsurancePlanCommand(request.Name));
        return Ok(result);
    }
}