using System.Security.Claims;
using MedApp.Application.Common.DTOs;
using MedApp.Application.Common.Exceptions;
using MedApp.Application.Features.WaitingList.Commands;
using MedApp.Application.Features.WaitingList.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WaitingListController : ControllerBase
{
    private readonly IMediator _mediator;

    public WaitingListController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Join(JoinWaitingListRequest request)
    {
        try
        {
            Guid? userId = null;
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim != null)
                userId = Guid.Parse(userIdClaim);

            var result = await _mediator.Send(new JoinWaitingListCommand(
                request.DoctorId,
                request.ClinicId,
                request.PreferredDateFrom,
                request.PreferredDateTo,
                request.FirstName,
                request.LastName,
                request.Email,
                request.Phone,
                request.DNI,
                userId));

            return Ok(result);
        }
        catch (AppException ex)
        {
            return StatusCode(ex.StatusCode, new { message = ex.Message });
        }
    }

    [HttpGet("doctor/{doctorId}/clinic/{clinicId}")]
    [Authorize(Roles = "Doctor,Secretary")]
    public async Task<IActionResult> GetByDoctor(Guid doctorId, Guid clinicId)
    {
        var result = await _mediator.Send(new GetWaitingListQuery(doctorId, clinicId));
        return Ok(result);
    }

    [HttpPost("notify")]
    [Authorize(Roles = "Doctor,Secretary")]
    public async Task<IActionResult> Notify([FromBody] NotifyWaitingListRequest request)
    {
        var result = await _mediator.Send(new NotifyWaitingListCommand(
            request.DoctorId,
            request.ClinicId,
            request.AvailableSlot));
        return Ok(new { notified = result });
    }
}