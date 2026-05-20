using MedApp.Application.Common.DTOs;
using MedApp.Domain.Enums;
using MedApp.Domain.Interfaces;
using MediatR;

namespace MedApp.Application.Features.Dashboard.Queries;

public class GetDashboardQueryHandler : IRequestHandler<GetDashboardQuery, DashboardDto>
{
    private readonly IUnitOfWork _uow;

    public GetDashboardQueryHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<DashboardDto> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var todayStart = now.Date;
        var weekStart = todayStart.AddDays(-(int)now.DayOfWeek);
        var monthStart = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        var allAppointments = await _uow.Appointments.FindAsync(a => a.DoctorId == request.DoctorId);
        var allPayments = await _uow.Payments.FindAsync(p => true);

        var today = allAppointments.Where(a =>
            a.DateTime >= todayStart && a.DateTime < todayStart.AddDays(1) &&
            a.Status != AppointmentStatus.Cancelled).ToList();

        var thisWeek = allAppointments.Where(a =>
            a.DateTime >= weekStart &&
            a.Status != AppointmentStatus.Cancelled).ToList();

        var thisMonth = allAppointments.Where(a =>
            a.DateTime >= monthStart &&
            a.Status != AppointmentStatus.Cancelled).ToList();

        var cancelledThisMonth = allAppointments.Count(a =>
            a.DateTime >= monthStart &&
            a.Status == AppointmentStatus.Cancelled);

        // Pacientes nuevos este mes
        var patientIds = thisMonth.Select(a => a.PatientId).Distinct();
        var newPatients = 0;
        foreach (var pid in patientIds)
        {
            var patients = await _uow.Patients.FindAsync(p => p.Id == pid);
            var patient = patients.FirstOrDefault();
            if (patient != null && patient.CreatedAt >= monthStart)
                newPatients++;
        }

        // Ingresos
        var monthAppointmentIds = thisMonth.Select(a => a.Id).ToHashSet();
        var monthPayments = allPayments.Where(p => monthAppointmentIds.Contains(p.AppointmentId)).ToList();
        var totalRevenue = monthPayments.Where(p => p.Status == "approved").Sum(p => p.Amount);
        var pendingRevenue = thisMonth
            .Where(a => a.PaymentStatus == PaymentStatus.Unpaid)
            .Count() * (await GetAvgPrice(request.DoctorId));

        // Turnos por día esta semana
        var byDay = Enumerable.Range(0, 7).Select(i =>
        {
            var day = weekStart.AddDays(i);
            var count = allAppointments.Count(a =>
                a.DateTime.Date == day.Date &&
                a.Status != AppointmentStatus.Cancelled);
            return new AppointmentsByDayDto(day.ToString("ddd dd/MM"), count);
        }).ToList();

        return new DashboardDto(
            today.Count,
            thisWeek.Count,
            thisMonth.Count,
            cancelledThisMonth,
            newPatients,
            totalRevenue,
            pendingRevenue,
            byDay
        );
    }

    private async Task<decimal> GetAvgPrice(Guid doctorId)
    {
        var doctors = await _uow.Doctors.FindAsync(d => d.Id == doctorId);
        return doctors.FirstOrDefault()?.ConsultationPrice ?? 0;
    }
}