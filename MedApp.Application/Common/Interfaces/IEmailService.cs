namespace MedApp.Application.Common.Interfaces;

public interface IEmailService
{
    Task SendAppointmentConfirmationAsync(string toEmail, string patientName, string doctorName, string clinicName, DateTime dateTime);
    Task SendAppointmentReminderAsync(string toEmail, string patientName, string doctorName, string clinicName, DateTime dateTime);
    Task SendAppointmentCancellationAsync(string toEmail, string patientName, string doctorName, DateTime dateTime, string reason);
    Task SendWaitingListNotificationAsync(string toEmail, string patientName, string doctorName, DateTime availableSlot);
}