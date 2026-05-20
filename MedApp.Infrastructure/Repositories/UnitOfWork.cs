using MedApp.Domain.Entities;
using MedApp.Domain.Interfaces;
using MedApp.Infrastructure.Persistence;

namespace MedApp.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IRepository<User> Users { get; }
    public IRepository<Doctor> Doctors { get; }
    public IRepository<Patient> Patients { get; }
    public IRepository<Appointment> Appointments { get; }
    public IRepository<Clinic> Clinics { get; }
    public IRepository<Specialty> Specialties { get; }
    public IRepository<Availability> Availabilities { get; }
    public IRepository<BlockedSlot> BlockedSlots { get; }
    public IRepository<WaitingList> WaitingLists { get; }
    public IRepository<Payment> Payments { get; }
    public IRepository<Notification> Notifications { get; }
    public IRepository<GuestToken> GuestTokens { get; }
    public IRepository<InsurancePlan> InsurancePlans { get; }
    public IRepository<AuditLog> AuditLogs { get; }

    public IRepository<DoctorSpecialty> DoctorSpecialties { get; }
    public IRepository<DoctorClinic> DoctorClinics { get; }
    public IRepository<DoctorInsurancePlan> DoctorInsurancePlans { get; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Users = new Repository<User>(context);
        Doctors = new Repository<Doctor>(context);
        Patients = new Repository<Patient>(context);
        Appointments = new Repository<Appointment>(context);
        Clinics = new Repository<Clinic>(context);
        Specialties = new Repository<Specialty>(context);
        Availabilities = new Repository<Availability>(context);
        BlockedSlots = new Repository<BlockedSlot>(context);
        WaitingLists = new Repository<WaitingList>(context);
        Payments = new Repository<Payment>(context);
        Notifications = new Repository<Notification>(context);
        GuestTokens = new Repository<GuestToken>(context);
        InsurancePlans = new Repository<InsurancePlan>(context);
        AuditLogs = new Repository<AuditLog>(context);
        DoctorSpecialties = new Repository<DoctorSpecialty>(context);
        DoctorClinics = new Repository<DoctorClinic>(context);
        DoctorInsurancePlans = new Repository<DoctorInsurancePlan>(context);
    }

    public async Task<int> SaveChangesAsync() =>
        await _context.SaveChangesAsync();

    public void Dispose() =>
        _context.Dispose();
}