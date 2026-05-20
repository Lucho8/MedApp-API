using MedApp.Domain.Enums;

namespace MedApp.Domain.Entities;

public class Notification : BaseEntity
{
    public Guid? UserId { get; set; }
    public string RecipientEmail { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; } = false;

    public User? User { get; set; }
}