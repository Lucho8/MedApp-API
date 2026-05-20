using MedApp.Application.Common.DTOs;
using MedApp.Application.Common.Exceptions;
using MedApp.Application.Features.Clinics.Commands;
using MedApp.Application.Features.Clinics.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClinicsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClinicsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetClinicsQuery());
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> Create(CreateClinicRequest request)
    {
        try
        {
            var result = await _mediator.Send(new CreateClinicCommand(
                request.Name,
                request.Address,
                request.Phone,
                request.City));
            return Ok(result);
        }
        catch (AppException ex)
        {
            return StatusCode(ex.StatusCode, new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> Update(Guid id, UpdateClinicRequest request)
    {
        try
        {
            var result = await _mediator.Send(new UpdateClinicCommand(
                id,
                request.Name,
                request.Address,
                request.Phone,
                request.City));
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
            await _mediator.Send(new DeleteClinicCommand(id));
            return NoContent();
        }
        catch (AppException ex)
        {
            return StatusCode(ex.StatusCode, new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetClinicByIdQuery(id));
        return result == null ? NotFound() : Ok(result);
    }

}