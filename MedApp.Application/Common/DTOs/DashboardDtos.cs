namespace MedApp.Application.Common.DTOs;

public record DashboardDto(
    int TotalAppointmentsToday,
    int TotalAppointmentsThisWeek,
    int TotalAppointmentsThisMonth,
    int CancelledThisMonth,
    int NewPatientsThisMonth,
    decimal TotalRevenueThisMonth,
    decimal PendingRevenueThisMonth,
    List<AppointmentsByDayDto> AppointmentsByDay
);

public record AppointmentsByDayDto(
    string Day,
    int Count
);