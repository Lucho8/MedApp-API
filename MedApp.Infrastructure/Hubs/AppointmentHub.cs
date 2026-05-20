using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace MedApp.Infrastructure.Hubs;

[Authorize]
public class AppointmentHub : Hub
{
    public async Task JoinDoctorGroup(string doctorId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"doctor-{doctorId}");
    }

    public async Task LeaveDoctorGroup(string doctorId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"doctor-{doctorId}");
    }
}