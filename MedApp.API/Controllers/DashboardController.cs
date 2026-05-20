using MedApp.Application.Features.Dashboard.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Doctor,Secretary")]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public DashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{doctorId}")]
    public async Task<IActionResult> Get(Guid doctorId)
    {
        var result = await _mediator.Send(new GetDashboardQuery(doctorId));
        return Ok(result);
    }
}