using Microsoft.EntityFrameworkCore;
using MedApp.Application.Common.DTOs;
using MedApp.Domain.Interfaces;
using MediatR;

namespace MedApp.Application.Features.Doctors.Queries;

public class GetDoctorByIdQueryHandler : IRequestHandler<GetDoctorByIdQuery, DoctorDto?>
{
    private readonly IUnitOfWork _uow;

    public GetDoctorByIdQueryHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<DoctorDto?> Handle(GetDoctorByIdQuery request, CancellationToken cancellationToken)
    {
    var doctors = await _uow.Doctors.QueryAsync(q => q
        .Include(d => d.DoctorSpecialties).ThenInclude(ds => ds.Specialty)
        .Include(d => d.DoctorClinics).ThenInclude(dc => dc.Clinic)
        .Include(d => d.DoctorInsurancePlans).ThenInclude(di => di.InsurancePlan)
        .Where(d => d.Id == request.Id && d.IsActive));

    var doctor = doctors.FirstOrDefault();
    if (doctor == null) return null;

    return new DoctorDto(
        doctor.Id, doctor.FirstName, doctor.LastName, doctor.Bio, doctor.PhotoUrl,
        doctor.AppointmentDurationMinutes, doctor.ConsultationPrice,
        doctor.DoctorSpecialties.Select(ds => ds.Specialty.Name).ToList(),
        doctor.DoctorClinics.Select(dc => dc.Clinic.Name).ToList(),
        doctor.DoctorInsurancePlans.Select(di => di.InsurancePlan.Name).ToList()
    );
    }

}