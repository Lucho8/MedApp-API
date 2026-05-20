using MedApp.Application.Common.DTOs;
using MedApp.Application.Common.Exceptions;
using MedApp.Application.Features.Availability.Commands;
using MedApp.Application.Features.Availability.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AvailabilityController : ControllerBase
{
    private readonly IMediator _mediator;

    public AvailabilityController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("doctor/{doctorId}/clinic/{clinicId}")]
    public async Task<IActionResult> GetAvailability(Guid doctorId, Guid clinicId)
    {
        var result = await _mediator.Send(new GetAvailabilityQuery(doctorId, clinicId));
        return Ok(result);
    }

    [HttpGet("slots")]
    public async Task<IActionResult> GetAvailableSlots(
        [FromQuery] Guid doctorId,
        [FromQuery] Guid clinicId,
        [FromQuery] DateOnly date)
    {
        var result = await _mediator.Send(new GetAvailableSlotsQuery(doctorId, clinicId, date));
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> Create(CreateAvailabilityRequest request, [FromQuery] Guid doctorId)
    {
        try
        {
            var result = await _mediator.Send(new CreateAvailabilityCommand(
                doctorId,
                request.ClinicId,
                request.DayOfWeek,
                request.StartTime,
                request.EndTime));
            return Ok(result);
        }
        catch (AppException ex)
        {
            return StatusCode(ex.StatusCode, new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _mediator.Send(new DeleteAvailabilityCommand(id));
            return NoContent();
        }
        catch (AppException ex)
        {
            return StatusCode(ex.StatusCode, new { message = ex.Message });
        }
    }
}