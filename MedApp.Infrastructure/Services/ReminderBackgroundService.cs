using MedApp.Application.Common.Interfaces;
using MedApp.Domain.Enums;
using MedApp.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MedApp.Infrastructure.Services;

public class ReminderBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ReminderBackgroundService> _logger;

    public ReminderBackgroundService(IServiceScopeFactory scopeFactory, ILogger<ReminderBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await SendReminders();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando recordatorios");
            }

            // Corre cada hora
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }

    private async Task SendReminders()
    {
        using var scope = _scopeFactory.CreateScope();
        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

        var now = DateTime.UtcNow;
        var in24Hours = now.AddHours(24);
        var in25Hours = now.AddHours(25);

        // Turnos que son en las próximas 24-25 horas y están confirmados
        var appointments = await uow.Appointments.FindAsync(a =>
            a.DateTime >= in24Hours &&
            a.DateTime < in25Hours &&
            a.Status == AppointmentStatus.Confirmed);

        foreach (var appointment in appointments)
        {
            var doctors = await uow.Doctors.FindAsync(d => d.Id == appointment.DoctorId);
            var doctor = doctors.FirstOrDefault();
            var patients = await uow.Patients.FindAsync(p => p.Id == appointment.PatientId);
            var patient = patients.FirstOrDefault();
            var clinics = await uow.Clinics.FindAsync(c => c.Id == appointment.ClinicId);
            var clinic = clinics.FirstOrDefault();

            if (doctor == null || patient == null || clinic == null) continue;

            await emailService.SendAppointmentReminderAsync(
                patient.Email,
                $"{patient.FirstName} {patient.LastName}",
                $"Dr. {doctor.FirstName} {doctor.LastName}",
                clinic.Name,
                appointment.DateTime);

            _logger.LogInformation("Recordatorio enviado a {Email} para turno {Id}", patient.Email, appointment.Id);
        }
    }
}