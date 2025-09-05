using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<OrgUnit> OrganizationalUnits { get; set; }
    public DbSet<RoleReporting> RoleReportings { get; set; }
    public DbSet<EmployeeRoleHistory> EmployeeRoleHistory { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Many-to-many relationship for Role Reporting
        modelBuilder.Entity<RoleReporting>()
            .HasKey(rr => new { rr.DirectReportId, rr.ReportsToId });

        modelBuilder.Entity<RoleReporting>()
            .HasOne(rr => rr.DirectReport)
            .WithMany(r => r.DirectReports)
            .HasForeignKey(rr => rr.DirectReportId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RoleReporting>()
            .HasOne(rr => rr.ReportsTo)
            .WithMany(r => r.ReportsTo)
            .HasForeignKey(rr => rr.ReportsToId)
            .OnDelete(DeleteBehavior.Restrict);

        // Many-to-One relationship for Employee.CurrentRole -> Role
        modelBuilder.Entity<Employee>()
            .HasOne(e => e.CurrentRole)
            .WithMany(r => r.Employees)
            .HasForeignKey(e => e.CurrentRoleId);

        // Relationship for Employee Role History
        modelBuilder.Entity<Employee>()
            .HasMany(e => e.RoleHistory)
            .WithOne(erh => erh.Employee)
            .HasForeignKey(erh => erh.EmployeeId);

        modelBuilder.Entity<EmployeeRoleHistory>()
            .HasOne(erh => erh.Role)
            .WithMany()
            .HasForeignKey(erh => erh.RoleId);

        // Relationship for Organizational Unit managers
        modelBuilder.Entity<OrgUnit>()
            .HasOne(ou => ou.GeneralManager)
            .WithMany()
            .HasForeignKey(ou => ou.GMId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<OrgUnit>()
            .HasOne(ou => ou.LineManager)
            .WithMany()
            .HasForeignKey(ou => ou.LMId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<OrgUnit>()
            .HasOne(ou => ou.HRBusinessPartner)
            .WithMany()
            .HasForeignKey(ou => ou.HRBPId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}