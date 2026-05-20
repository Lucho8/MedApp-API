using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using MedApp.Application.Common.Interfaces;
using MedApp.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace MedApp.Infrastructure.Services;

public class MercadoPagoService : IPaymentService
{
    private readonly IConfiguration _config;
    private readonly IUnitOfWork _uow;
    private readonly HttpClient _http;

    public MercadoPagoService(IConfiguration config, IUnitOfWork uow, IHttpClientFactory httpClientFactory)
    {
        _config = config;
        _uow = uow;
        _http = httpClientFactory.CreateClient("MercadoPago");
    }

    public async Task<string> CreatePreferenceAsync(Guid appointmentId, decimal amount, string patientEmail, string description)
    {
        var token = _config["MercadoPago:AccessToken"]!;
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var body = new
        {
            items = new[]
            {
                new
                {
                    title = description,
                    quantity = 1,
                    unit_price = amount,
                    currency_id = "ARS"
                }
            },
            payer = new { email = patientEmail },
            external_reference = appointmentId.ToString(),
            back_urls = new
            {
                success = $"{_config["App:FrontendUrl"]}/turno/pago-exitoso",
                failure = $"{_config["App:FrontendUrl"]}/turno/pago-fallido",
                pending = $"{_config["App:FrontendUrl"]}/turno/pago-pendiente"
            },
            auto_return = "approved"
        };

        var json = JsonSerializer.Serialize(body);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _http.PostAsync("https://api.mercadopago.com/checkout/preferences", content);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();
        var doc = JsonDocument.Parse(result);
        return doc.RootElement.GetProperty("init_point").GetString()!;
    }

    public async Task<bool> ProcessWebhookAsync(string paymentId)
    {
        var token = _config["MercadoPago:AccessToken"]!;
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _http.GetAsync($"https://api.mercadopago.com/v1/payments/{paymentId}");
        if (!response.IsSuccessStatusCode) return false;

        var result = await response.Content.ReadAsStringAsync();
        var doc = JsonDocument.Parse(result);

        var status = doc.RootElement.GetProperty("status").GetString();
        var externalRef = doc.RootElement.GetProperty("external_reference").GetString();
        var transactionAmount = doc.RootElement.GetProperty("transaction_amount").GetDecimal();

        if (!Guid.TryParse(externalRef, out var appointmentId)) return false;

        var appointments = await _uow.Appointments.FindAsync(a => a.Id == appointmentId);
        var appointment = appointments.FirstOrDefault();
        if (appointment == null) return false;

        if (status == "approved")
        {
            appointment.PaymentStatus = Domain.Enums.PaymentStatus.Paid;

            var existingPayments = await _uow.Payments.FindAsync(p => p.AppointmentId == appointmentId);
            if (!existingPayments.Any())
            {
                await _uow.Payments.AddAsync(new Domain.Entities.Payment
                {
                    AppointmentId = appointmentId,
                    Amount = transactionAmount,
                    Currency = "ARS",
                    Provider = "MercadoPago",
                    ExternalPaymentId = paymentId,
                    Status = "approved",
                    UpdatedAt = DateTime.UtcNow
                });
            }

            _uow.Appointments.Update(appointment);
            await _uow.SaveChangesAsync();
        }

        return true;
    }
}