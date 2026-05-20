using MedApp.Application.Common.DTOs;
using MedApp.Application.Common.Exceptions;
using MedApp.Domain.Interfaces;
using MediatR;

namespace MedApp.Application.Features.WaitingList.Commands;

public class JoinWaitingListCommandHandler : IRequestHandler<JoinWaitingListCommand, WaitingListDto>
{
    private readonly IUnitOfWork _uow;

    public JoinWaitingListCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<WaitingListDto> Handle(JoinWaitingListCommand request, CancellationToken cancellationToken)
    {
        var doctors = await _uow.Doctors.FindAsync(d => d.Id == request.DoctorId && d.IsActive);
        var doctor = doctors.FirstOrDefault()
            ?? throw new AppException("Médico no encontrado.", 404);

        var clinics = await _uow.Clinics.FindAsync(c => c.Id == request.ClinicId && c.IsActive);
        var clinic = clinics.FirstOrDefault()
            ?? throw new AppException("Clínica no encontrada.", 404);

        Domain.Entities.Patient patient;

        if (request.RegisteredPatientUserId.HasValue)
        {
            var patients = await _uow.Patients.FindAsync(p => p.UserId == request.RegisteredPatientUserId);
            patient = patients.FirstOrDefault()
                ?? throw new AppException("Paciente no encontrado.", 404);
        }
        else
        {
            if (string.IsNullOrEmpty(request.Email))
                throw new AppException("El email es requerido.");

            var existing = await _uow.Patients.FindAsync(p =>
                p.Email == request.Email && p.UserId == null);
            patient = existing.FirstOrDefault()!;

            if (patient == null)
            {
                patient = new Domain.Entities.Patient
                {
                    FirstName = request.FirstName ?? "",
                    LastName = request.LastName ?? "",
                    Email = request.Email,
                    Phone = request.Phone,
                    DNI = request.DNI
                };
                await _uow.Patients.AddAsync(patient);
            }
        }

        var entry = new Domain.Entities.WaitingList
        {
            DoctorId = request.DoctorId,
            PatientId = patient.Id,
            ClinicId = request.ClinicId,
            PreferredDateFrom = request.PreferredDateFrom,
            PreferredDateTo = request.PreferredDateTo,
            Status = "Waiting"
        };

        await _uow.WaitingLists.AddAsync(entry);
        await _uow.SaveChangesAsync();

        return new WaitingListDto(
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
        );
    }
}