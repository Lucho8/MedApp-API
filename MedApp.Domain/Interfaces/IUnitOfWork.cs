using MedApp.Domain.Entities;

namespace MedApp.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<User> Users { get; }
    IRepository<Doctor> Doctors { get; }
    IRepository<Patient> Patients { get; }
    IRepository<Appointment> Appointments { get; }
    IRepository<Clinic> Clinics { get; }
    IRepository<Specialty> Specialties { get; }
    IRepository<Availability> Availabilities { get; }
    IRepository<BlockedSlot> BlockedSlots { get; }
    IRepository<WaitingList> WaitingLists { get; }
    IRepository<Payment> Payments { get; }
    IRepository<Notification> Notifications { get; }
    IRepository<GuestToken> GuestTokens { get; }
    IRepository<InsurancePlan> InsurancePlans { get; }
    IRepository<AuditLog> AuditLogs { get; }
    IRepository<DoctorSpecialty> DoctorSpecialties { get; }
    IRepository<DoctorClinic> DoctorClinics { get; }
    IRepository<DoctorInsurancePlan> DoctorInsurancePlans { get; }
    Task<int> SaveChangesAsync();
}