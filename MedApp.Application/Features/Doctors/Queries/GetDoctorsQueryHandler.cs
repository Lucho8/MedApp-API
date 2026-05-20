using Microsoft.EntityFrameworkCore;
using MedApp.Application.Common.DTOs;
using MedApp.Domain.Interfaces;
using MediatR;

namespace MedApp.Application.Features.Doctors.Queries;

public class GetDoctorsQueryHandler : IRequestHandler<GetDoctorsQuery, List<DoctorDto>>
{
    private readonly IUnitOfWork _uow;

    public GetDoctorsQueryHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<List<DoctorDto>> Handle(GetDoctorsQuery request, CancellationToken cancellationToken)
{
    var doctors = await _uow.Doctors.QueryAsync(q => q
        .Include(d => d.DoctorSpecialties).ThenInclude(ds => ds.Specialty)
        .Include(d => d.DoctorClinics).ThenInclude(dc => dc.Clinic)
        .Include(d => d.DoctorInsurancePlans).ThenInclude(di => di.InsurancePlan)
        .Where(d => d.IsActive));

    if (!string.IsNullOrEmpty(request.Specialty))
        doctors = doctors.Where(d =>
            d.DoctorSpecialties.Any(ds =>
                ds.Specialty.Name.ToLower().Contains(request.Specialty.ToLower())));

    return doctors.Select(d => new DoctorDto(
        d.Id, d.FirstName, d.LastName, d.Bio, d.PhotoUrl,
        d.AppointmentDurationMinutes, d.ConsultationPrice,
        d.DoctorSpecialties.Select(ds => ds.Specialty.Name).ToList(),
        d.DoctorClinics.Select(dc => dc.Clinic.Name).ToList(),
        d.DoctorInsurancePlans.Select(di => di.InsurancePlan.Name).ToList()
    )).ToList();
}

    

}