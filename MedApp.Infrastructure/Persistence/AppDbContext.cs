using MedApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedApp.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Specialty> Specialties => Set<Specialty>();
    public DbSet<DoctorSpecialty> DoctorSpecialties => Set<DoctorSpecialty>();
    public DbSet<Clinic> Clinics => Set<Clinic>();
    public DbSet<DoctorClinic> DoctorClinics => Set<DoctorClinic>();
    public DbSet<InsurancePlan> InsurancePlans => Set<InsurancePlan>();
    public DbSet<DoctorInsurancePlan> DoctorInsurancePlans => Set<DoctorInsurancePlan>();
    public DbSet<Availability> Availabilities => Set<Availability>();
    public DbSet<BlockedSlot> BlockedSlots => Set<BlockedSlot>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<WaitingList> WaitingLists => Set<WaitingList>();
    public DbSet<GuestToken> GuestTokens => Set<GuestToken>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<SecretaryDoctor> SecretaryDoctors => Set<SecretaryDoctor>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}