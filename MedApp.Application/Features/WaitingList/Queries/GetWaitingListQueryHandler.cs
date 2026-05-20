using MedApp.Application.Common.DTOs;
using MedApp.Domain.Interfaces;
using MediatR;

namespace MedApp.Application.Features.WaitingList.Queries;

public class GetWaitingListQueryHandler : IRequestHandler<GetWaitingListQuery, List<WaitingListDto>>
{
    private readonly IUnitOfWork _uow;

    public GetWaitingListQueryHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<List<WaitingListDto>> Handle(GetWaitingListQuery request, CancellationToken cancellationToken)
    {
        var entries = await _uow.WaitingLists.FindAsync(w =>
            w.DoctorId == request.DoctorId &&
            w.ClinicId == request.ClinicId &&
            w.Status == "Waiting");

        var result = new List<WaitingListDto>();

        foreach (var entry in entries)
        {
            var doctors = await _uow.Doctors.FindAsync(d => d.Id == entry.DoctorId);
            var doctor = doctors.FirstOrDefault();
            var patients = await _uow.Patients.FindAsync(p => p.Id == entry.PatientId);
            var patient = patients.FirstOrDefault();
            var clinics = await _uow.Clinics.FindAsync(c => c.Id == entry.ClinicId);
            var clinic = clinics.FirstOrDefault();

            if (doctor == null || patient == null || clinic == null) continue;

            result.Add(new WaitingListDto(
                entry.Id,
                doctor.Id,
                $"{doctor.FirstName} {doctor.LastName}",
                patient.Id,
                $"{patient.FirstName} {patient.LastName}",
                clinic.Id,
                clinic.Name,
                entry.PreferredDateFrom,
                entry.PreferredDateTo,
                entry.Status,
                entry.CreatedAt
            ));
        }

        return result;
    }
}