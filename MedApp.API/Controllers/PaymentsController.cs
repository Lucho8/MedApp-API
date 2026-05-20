using MedApp.Application.Common.Interfaces;
using MedApp.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly IUnitOfWork _uow;

    public PaymentsController(IPaymentService paymentService, IUnitOfWork uow)
    {
        _paymentService = paymentService;
        _uow = uow;
    }

    [HttpPost("create/{appointmentId}")]
    public async Task<IActionResult> CreatePreference(Guid appointmentId)
    {
        var appointments = await _uow.Appointments.FindAsync(a => a.Id == appointmentId);
        var appointment = appointments.FirstOrDefault();
        if (appointment == null) return NotFound();

        var doctors = await _uow.Doctors.FindAsync(d => d.Id == appointment.DoctorId);
        var doctor = doctors.FirstOrDefault();
        if (doctor == null) return NotFound();

        var patients = await _uow.Patients.FindAsync(p => p.Id == appointment.PatientId);
        var patient = patients.FirstOrDefault();
        if (patient == null) return NotFound();

        var initPoint = await _paymentService.CreatePreferenceAsync(
            appointment.Id,
            doctor.ConsultationPrice,
            patient.Email,
            $"Consulta con {doctor.FirstName} {doctor.LastName}");

        return Ok(new { checkoutUrl = initPoint });
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> Webhook([FromQuery] string topic, [FromQuery] string id)
    {
        if (topic == "payment")
            await _paymentService.ProcessWebhookAsync(id);

        return Ok();
    }
}