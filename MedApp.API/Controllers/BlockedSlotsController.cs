using System.Security.Claims;
using MedApp.Application.Common.DTOs;
using MedApp.Application.Features.BlockedSlots.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Doctor,Secretary")]
public class BlockedSlotsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BlockedSlotsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromQuery] Guid doctorId, CreateBlockedSlotRequest request)
    {
        var result = await _mediator.Send(new CreateBlockedSlotCommand(
            doctorId, request.ClinicId, request.Date,
            request.StartTime, request.EndTime, request.Reason));
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteBlockedSlotCommand(id));
        return NoContent();
    }
}