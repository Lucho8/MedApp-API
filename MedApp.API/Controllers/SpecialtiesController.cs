using MedApp.Application.Common.DTOs;
using MedApp.Application.Features.Specialties.Commands;
using MedApp.Application.Features.Specialties.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpecialtiesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SpecialtiesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetSpecialtiesQuery());
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> Create(CreateSpecialtyRequest request)
    {
        var result = await _mediator.Send(new CreateSpecialtyCommand(request.Name, request.Description));
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteSpecialtyCommand(id));
        return NoContent();
    }
}