namespace MedApp.Application.Common.Interfaces;

public interface IPaymentService
{
    Task<string> CreatePreferenceAsync(Guid appointmentId, decimal amount, string patientEmail, string description);
    Task<bool> ProcessWebhookAsync(string paymentId);
}