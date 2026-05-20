namespace MedApp.Application.Common.Interfaces;

public interface INotificationService
{
    Task NotifyNewAppointmentAsync(string doctorId, object appointment);
    Task NotifyAppointmentCancelledAsync(string doctorId, object appointment);
}