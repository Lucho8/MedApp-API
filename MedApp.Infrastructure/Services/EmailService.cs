using MedApp.Application.Common.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace MedApp.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    private async Task SendAsync(string toEmail, string toName, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(
            _config["Email:FromName"]!,
            _config["Email:Username"]!));
        message.To.Add(new MailboxAddress(toName, toEmail));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = body };

        using var client = new SmtpClient();
        await client.ConnectAsync(
            _config["Email:Host"]!,
            int.Parse(_config["Email:Port"]!),
            SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(
            _config["Email:Username"]!,
            _config["Email:Password"]!);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

    public async Task SendAppointmentConfirmationAsync(string toEmail, string patientName, string doctorName, string clinicName, DateTime dateTime)
    {
        var body = $"""
            <h2>¡Tu turno está confirmado!</h2>
            <p>Hola <strong>{patientName}</strong>,</p>
            <p>Tu turno ha sido reservado exitosamente.</p>
            <ul>
                <li><strong>Médico:</strong> {doctorName}</li>
                <li><strong>Clínica:</strong> {clinicName}</li>
                <li><strong>Fecha y hora:</strong> {dateTime:dd/MM/yyyy HH:mm}</li>
            </ul>
            <p>Si necesitás cancelar, hacelo con al menos 2 horas de anticipación.</p>
            <p>¡Hasta pronto!</p>
        """;

        await SendAsync(toEmail, patientName, "Turno confirmado — MedApp", body);
    }

    public async Task SendAppointmentReminderAsync(string toEmail, string patientName, string doctorName, string clinicName, DateTime dateTime)
    {
        var body = $"""
            <h2>Recordatorio de turno</h2>
            <p>Hola <strong>{patientName}</strong>,</p>
            <p>Te recordamos que mañana tenés turno médico.</p>
            <ul>
                <li><strong>Médico:</strong> {doctorName}</li>
                <li><strong>Clínica:</strong> {clinicName}</li>
                <li><strong>Fecha y hora:</strong> {dateTime:dd/MM/yyyy HH:mm}</li>
            </ul>
            <p>¡Que te vaya bien!</p>
        """;

        await SendAsync(toEmail, patientName, "Recordatorio de turno — MedApp", body);
    }

    public async Task SendAppointmentCancellationAsync(string toEmail, string patientName, string doctorName, DateTime dateTime, string reason)
    {
        var body = $"""
            <h2>Turno cancelado</h2>
            <p>Hola <strong>{patientName}</strong>,</p>
            <p>Tu turno ha sido cancelado.</p>
            <ul>
                <li><strong>Médico:</strong> {doctorName}</li>
                <li><strong>Fecha y hora:</strong> {dateTime:dd/MM/yyyy HH:mm}</li>
                <li><strong>Motivo:</strong> {reason}</li>
            </ul>
            <p>Podés reservar un nuevo turno cuando quieras.</p>
        """;

        await SendAsync(toEmail, patientName, "Turno cancelado — MedApp", body);
    }

    public async Task SendWaitingListNotificationAsync(string toEmail, string patientName, string doctorName, DateTime availableSlot)
    {
        var body = $"""
            <h2>¡Hay un turno disponible!</h2>
        <p>Hola <strong>{patientName}</strong>,</p>
        <p>Se liberó un turno con el Dr. <strong>{doctorName}</strong>.</p>
        <ul>
            <li><strong>Fecha y hora:</strong> {availableSlot:dd/MM/yyyy HH:mm}</li>
        </ul>
        <p>Ingresá a MedApp para reservarlo antes de que se ocupe.</p>
    """;

    await SendAsync(toEmail, patientName, "Turno disponible — MedApp", body);
    }
}