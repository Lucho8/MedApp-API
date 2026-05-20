using System.Security.Claims;
using MedApp.Application.Common.DTOs;
using MedApp.Application.Common.Exceptions;
using MedApp.Application.Features.Appointments.Commands;
using MedApp.Application.Features.Appointments.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AppointmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAppointmentRequest request)
    {
        try
        {
            // Si el usuario está autenticado, usamos su ID
            Guid? userId = null;
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim != null)
                userId = Guid.Parse(userIdClaim);

            var result = await _mediator.Send(new CreateAppointmentCommand(
                request.DoctorId,
                request.ClinicId,
                request.DateTime,
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

    [HttpGet("doctor/{doctorId}")]
    [Authorize(Roles = "Doctor,Secretary")]
    public async Task<IActionResult> GetByDoctor(Guid doctorId, [FromQuery] DateOnly? date)
    {
        var result = await _mediator.Send(new GetAppointmentsByDoctorQuery(doctorId, date));
        return Ok(result);
    }

    [HttpGet("patient/{patientId}")]
    [Authorize]
    public async Task<IActionResult> GetByPatient(Guid patientId)
    {
        var result = await _mediator.Send(new GetAppointmentsByPatientQuery(patientId));
        return Ok(result);
    }

    [HttpPatch("{id}/cancel")]
    [Authorize]
    public async Task<IActionResult> Cancel(Guid id, CancelAppointmentRequest request)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            var result = await _mediator.Send(new CancelAppointmentCommand(
                id,
                Guid.Parse(userIdClaim),
                request.Reason));

            return Ok(result);
        }
        catch (AppException ex)
        {
            return StatusCode(ex.StatusCode, new { message = ex.Message });
        }
    }

    [HttpPatch("{id}/complete")]
[Authorize(Roles = "Doctor,Secretary")]
public async Task<IActionResult> Complete(Guid id)
{
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (userIdClaim == null) return Unauthorized();
    var result = await _mediator.Send(new CompleteAppointmentCommand(id, Guid.Parse(userIdClaim)));
    return Ok(result);
}

[HttpPatch("{id}/noshow")]
[Authorize(Roles = "Doctor,Secretary")]
public async Task<IActionResult> NoShow(Guid id)
{
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (userIdClaim == null) return Unauthorized();
    var result = await _mediator.Send(new MarkNoShowCommand(id, Guid.Parse(userIdClaim)));
    return Ok(result);
}

[HttpPatch("{id}/reschedule")]
[Authorize]
public async Task<IActionResult> Reschedule(Guid id, [FromBody] DateTime newDateTime)
{
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (userIdClaim == null) return Unauthorized();
    var result = await _mediator.Send(new RescheduleAppointmentCommand(id, newDateTime, Guid.Parse(userIdClaim)));
    return Ok(result);
}

[HttpPatch("{id}/pay-at-clinic")]
public async Task<IActionResult> PayAtClinic(Guid id)
{
    await _mediator.Send(new SetPayAtClinicCommand(id));
    return Ok(new { message = "Turno marcado para pagar en clínica." });
}


}