using MedApp.Application.Common.DTOs;
using MedApp.Application.Common.Exceptions;
using MedApp.Application.Features.Doctors.Commands;
using MedApp.Application.Features.Doctors.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DoctorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? specialty)
    {
        var result = await _mediator.Send(new GetDoctorsQuery(specialty));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetDoctorByIdQuery(id));
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> Update(Guid id, UpdateDoctorRequest request)
    {
        try
        {
            var result = await _mediator.Send(new UpdateDoctorCommand(
                id,
                request.FirstName,
                request.LastName,
                request.Bio,
                request.PhotoUrl,
                request.AppointmentDurationMinutes,
                request.ConsultationPrice));
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
            await _mediator.Send(new DeleteDoctorCommand(id));
            return NoContent();
        }
        catch (AppException ex)
        {
            return StatusCode(ex.StatusCode, new { message = ex.Message });
        }
    }

    [HttpPost("{id}/specialties")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> AssignSpecialty(Guid id, [FromBody] Guid specialtyId)
    {
        try
        {
            await _mediator.Send(new AssignSpecialtyCommand(id, specialtyId));
            return Ok(new { message = "Especialidad asignada correctamente." });
        }
        catch (AppException ex)
        {
            return StatusCode(ex.StatusCode, new { message = ex.Message });
        }
    }

    [HttpPost("{id}/clinics")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> AssignClinic(Guid id, [FromBody] Guid clinicId)
    {
        try
        {
            await _mediator.Send(new AssignClinicCommand(id, clinicId));
            return Ok(new { message = "Clínica asignada correctamente." });
        }
        catch (AppException ex)
        {
            return StatusCode(ex.StatusCode, new { message = ex.Message });
        }
    }

    [HttpPost("{id}/insurance-plans")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> AssignInsurancePlan(Guid id, [FromBody] Guid insurancePlanId)
    {
        try
        {
            await _mediator.Send(new AssignInsurancePlanCommand(id, insurancePlanId));
            return Ok(new { message = "Obra social asignada correctamente." });
        }
        catch (AppException ex)
        {
            return StatusCode(ex.StatusCode, new { message = ex.Message });
        }
    }   


}