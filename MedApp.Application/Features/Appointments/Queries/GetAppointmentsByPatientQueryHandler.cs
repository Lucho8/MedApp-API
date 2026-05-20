using MedApp.Application.Common.DTOs;
using MedApp.Domain.Interfaces;
using MediatR;

namespace MedApp.Application.Features.Appointments.Queries;

public class GetAppointmentsByPatientQueryHandler : IRequestHandler<GetAppointmentsByPatientQuery, List<AppointmentDto>>
{
    private readonly IUnitOfWork _uow;

    public GetAppointmentsByPatientQueryHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<List<AppointmentDto>> Handle(GetAppointmentsByPatientQuery request, CancellationToken cancellationToken)
    {
        var appointments = await _uow.Appointments.FindAsync(a => a.PatientId == request.PatientId);

        var result = new List<AppointmentDto>();

        foreach (var a in appointments)
        {
            var doctors = await _uow.Doctors.FindAsync(d => d.Id == a.DoctorId);
            var doctor = doctors.First();
            var patients = await _uow.Patients.FindAsync(p => p.Id == a.PatientId);
            var patient = patients.First();
            var clinics = await _uow.Clinics.FindAsync(c => c.Id == a.ClinicId);
            var clinic = clinics.First();

            result.Add(new AppointmentDto(
                a.Id,
                doctor.Id,
                $"{doctor.FirstName} {doctor.LastName}",
                patient.Id,
                $"{patient.FirstName} {patient.LastName}",
                clinic.Id,
                clinic.Name,
                a.DateTime,
                a.DurationMinutes,
                a.Status.ToString(),
                a.PaymentStatus.ToString(),
                a.Notes,
                a.CancellationReason
            ));
        }

        return result;
    }
}