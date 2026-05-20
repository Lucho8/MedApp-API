using MedApp.Application.Common.Interfaces;
using MedApp.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace MedApp.Infrastructure.Services;

public class SignalRNotificationService : INotificationService
{
    private readonly IHubContext<AppointmentHub> _hub;

    public SignalRNotificationService(IHubContext<AppointmentHub> hub)
    {
        _hub = hub;
    }

    public async Task NotifyNewAppointmentAsync(string doctorId, object appointment)
    {
        await _hub.Clients.Group($"doctor-{doctorId}")
            .SendAsync("NewAppointment", appointment);
    }

    public async Task NotifyAppointmentCancelledAsync(string doctorId, object appointment)
    {
        await _hub.Clients.Group($"doctor-{doctorId}")
            .SendAsync("AppointmentCancelled", appointment);
    }
}