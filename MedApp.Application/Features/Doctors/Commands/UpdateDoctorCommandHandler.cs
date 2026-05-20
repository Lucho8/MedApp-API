using MedApp.Application.Common.DTOs;
using MedApp.Application.Common.Exceptions;
using MedApp.Domain.Interfaces;
using MediatR;

namespace MedApp.Application.Features.Doctors.Commands;

public class UpdateDoctorCommandHandler : IRequestHandler<UpdateDoctorCommand, DoctorDto>
{
    private readonly IUnitOfWork _uow;

    public UpdateDoctorCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<DoctorDto> Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
    {
        var doctors = await _uow.Doctors.FindAsync(d => d.Id == request.DoctorId);
        var doctor = doctors.FirstOrDefault()
            ?? throw new AppException("Médico no encontrado.", 404);

        doctor.FirstName = request.FirstName;
        doctor.LastName = request.LastName;
        doctor.Bio = request.Bio;
        doctor.PhotoUrl = request.PhotoUrl;
        doctor.AppointmentDurationMinutes = request.AppointmentDurationMinutes;
        doctor.ConsultationPrice = request.ConsultationPrice;

        _uow.Doctors.Update(doctor);
        await _uow.SaveChangesAsync();

        return new DoctorDto(
            doctor.Id,
            doctor.FirstName,
            doctor.LastName,
            doctor.Bio,
            doctor.PhotoUrl,
            doctor.AppointmentDurationMinutes,
            doctor.ConsultationPrice,
            doctor.DoctorSpecialties.Select(ds => ds.Specialty.Name).ToList(),
            doctor.DoctorClinics.Select(dc => dc.Clinic.Name).ToList(),
            doctor.DoctorInsurancePlans.Select(di => di.InsurancePlan.Name).ToList()
        );
    }
}