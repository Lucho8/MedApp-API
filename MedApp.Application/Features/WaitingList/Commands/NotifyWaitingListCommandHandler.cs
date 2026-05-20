using MedApp.Application.Common.Interfaces;
using MedApp.Domain.Interfaces;
using MediatR;

namespace MedApp.Application.Features.WaitingList.Commands;

public class NotifyWaitingListCommandHandler : IRequestHandler<NotifyWaitingListCommand, int>
{
    private readonly IUnitOfWork _uow;
    private readonly IEmailService _emailService;

    public NotifyWaitingListCommandHandler(IUnitOfWork uow, IEmailService emailService)
    {
        _uow = uow;
        _emailService = emailService;
    }

    public async Task<int> Handle(NotifyWaitingListCommand request, CancellationToken cancellationToken)
    {
        var entries = await _uow.WaitingLists.FindAsync(w =>
            w.DoctorId == request.DoctorId &&
            w.ClinicId == request.ClinicId &&
            w.Status == "Waiting");

        var doctors = await _uow.Doctors.FindAsync(d => d.Id == request.DoctorId);
        var doctor = doctors.FirstOrDefault();
        if (doctor == null) return 0;

        int notified = 0;

        foreach (var entry in entries)
        {
            var patients = await _uow.Patients.FindAsync(p => p.Id == entry.PatientId);
            var patient = patients.FirstOrDefault();
            if (patient == null) continue;

            await _emailService.SendWaitingListNotificationAsync(
                patient.Email,
                $"{patient.FirstName} {patient.LastName}",
                $"{doctor.FirstName} {doctor.LastName}",
                request.AvailableSlot);

            entry.Status = "Notified";
            _uow.WaitingLists.Update(entry);
            notified++;
        }

        await _uow.SaveChangesAsync();
        return notified;
    }
}