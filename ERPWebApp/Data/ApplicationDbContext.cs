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

        modelBuilder.Entity<EmployeeRoleHistory>()
            .Property(e => e.StartDate)
            .HasColumnType("timestamp without time zone");

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

        // 1. Seed Organizational Units (without manager assignments)
        modelBuilder.Entity<OrgUnit>().HasData(
            new OrgUnit { UnitId = 1, Name = "Support Functions" },
            new OrgUnit { UnitId = 2, Name = "Workshop and Technical" },
            new OrgUnit { UnitId = 3, Name = "Parts and Inventory" },
            new OrgUnit { UnitId = 4, Name = "Sales Management" },
            new OrgUnit { UnitId = 5, Name = "Senior Suva Team" }
        );

        // 2. Seed Roles (requires OrganizationalUnitId)
        modelBuilder.Entity<Role>().HasData(
            new Role { RoleId = 1, Title = "Senior IT Officer", OrgUnitId = 1 },
            new Role { RoleId = 2, Title = "Supervisor West", OrgUnitId = 2 },
            new Role { RoleId = 3, Title = "WS Coordinator", OrgUnitId = 2 },
            new Role { RoleId = 4, Title = "Workshop Supervisor", OrgUnitId = 2 },
            new Role { RoleId = 5, Title = "N Parts Manager", OrgUnitId = 3 },
            new Role { RoleId = 6, Title = "PSSR Suva", OrgUnitId = 3 },
            new Role { RoleId = 7, Title = "PSSR West", OrgUnitId = 3 },
            new Role { RoleId = 8, Title = "Warehouse Supervisor", OrgUnitId = 3 },
            new Role { RoleId = 9, Title = "Parts Interpreter Labasa", OrgUnitId = 3 },
            new Role { RoleId = 10, Title = "Senior Parts Interpreter LTK", OrgUnitId = 3 },
            new Role { RoleId = 11, Title = "Senior Parts Interpreter Suva", OrgUnitId = 3 },
            new Role { RoleId = 12, Title = "National Sales Manager Machinery", OrgUnitId = 4 },
            new Role { RoleId = 13, Title = "Pacific Sales Manager", OrgUnitId = 4 },
            new Role { RoleId = 14, Title = "National Sales Manager Power Gen", OrgUnitId = 4 },
            new Role { RoleId = 15, Title = "Parts Interpreter Suva", OrgUnitId = 5 },
            new Role { RoleId = 16, Title = "Parts Cadet Suva", OrgUnitId = 5 }
        );

        //------MIGRATION 2--------
        // 3. Seed Employees (requires RoleId, which is now available)
        modelBuilder.Entity<Employee>().HasData(
            new Employee { EmployeeId = 1, FirstName = "Alice", LastName = "Smith", CurrentRoleId = 1, RoleId = 1 },
            new Employee { EmployeeId = 2, FirstName = "Bob", LastName = "Johnson", CurrentRoleId = 2, RoleId = 2 },
            new Employee { EmployeeId = 3, FirstName = "Charlie", LastName = "Brown", CurrentRoleId = 3, RoleId = 3 },
            new Employee { EmployeeId = 4, FirstName = "Diana", LastName = "Prince", CurrentRoleId = 4, RoleId = 4 },
            new Employee { EmployeeId = 5, FirstName = "Eve", LastName = "Adams", CurrentRoleId = 5, RoleId = 5 },
            new Employee { EmployeeId = 6, FirstName = "Frank", LastName = "Brian", CurrentRoleId = 6, RoleId = 6 },
            new Employee { EmployeeId = 7, FirstName = "George", LastName = "Coleson", CurrentRoleId = 7, RoleId = 7 },
            new Employee { EmployeeId = 8, FirstName = "Hannah", LastName = "Dean", CurrentRoleId = 8, RoleId = 8 },
            new Employee { EmployeeId = 9, FirstName = "Irene", LastName = "Edner", CurrentRoleId = 9, RoleId = 9 },
            new Employee { EmployeeId = 10, FirstName = "Justin", LastName = "Fennel", CurrentRoleId = 10, RoleId = 10 },
            new Employee { EmployeeId = 11, FirstName = "Karl", LastName = "Gregsmith", CurrentRoleId = 11, RoleId = 11 },
            new Employee { EmployeeId = 12, FirstName = "Laura", LastName = "Hollis", CurrentRoleId = 12, RoleId = 12 },
            new Employee { EmployeeId = 13, FirstName = "Meghan", LastName = "Innes", CurrentRoleId = 13, RoleId = 13 },
            new Employee { EmployeeId = 14, FirstName = "Nathan", LastName = "Jacobs", CurrentRoleId = 14, RoleId = 14 },
            new Employee { EmployeeId = 15, FirstName = "Keith", LastName = "Owens", CurrentRoleId = 15, RoleId = 15 },
            new Employee { EmployeeId = 16, FirstName = "Paulene", LastName = "Lowes", CurrentRoleId = 16, RoleId = 16 }
        );

        // 5. Seed Role Reporting (requires RoleId)
        modelBuilder.Entity<RoleReporting>().HasData(
            new RoleReporting { DirectReportId = 6, ReportsToId = 5 },
            new RoleReporting { DirectReportId = 7, ReportsToId = 5 },
            new RoleReporting { DirectReportId = 8, ReportsToId = 5 },
            new RoleReporting { DirectReportId = 9, ReportsToId = 5 },
            new RoleReporting { DirectReportId = 10, ReportsToId = 5 },
            new RoleReporting { DirectReportId = 11, ReportsToId = 5 },
            new RoleReporting { DirectReportId = 15, ReportsToId = 11 },
            new RoleReporting { DirectReportId = 16, ReportsToId = 11 }
        );

        // 6. Seed Employee Role History (requires EmployeeId and RoleId)
        modelBuilder.Entity<EmployeeRoleHistory>().HasData(
            new EmployeeRoleHistory { EmployeeRoleHistoryId = 1, EmployeeId = 1, RoleId = 1, StartDate = new DateTime(2020, 9, 6) },
            new EmployeeRoleHistory { EmployeeRoleHistoryId = 2, EmployeeId = 2, RoleId = 2, StartDate = new DateTime(2022, 9, 6) },
            new EmployeeRoleHistory { EmployeeRoleHistoryId = 3, EmployeeId = 3, RoleId = 3, StartDate = new DateTime(2023, 9, 6) },
            new EmployeeRoleHistory { EmployeeRoleHistoryId = 4, EmployeeId = 4, RoleId = 4, StartDate = new DateTime(2024, 9, 6) },
            new EmployeeRoleHistory { EmployeeRoleHistoryId = 5, EmployeeId = 5, RoleId = 5, StartDate = new DateTime(2021, 9, 6) },
            new EmployeeRoleHistory { EmployeeRoleHistoryId = 6, EmployeeId = 6, RoleId = 6, StartDate = new DateTime(2021, 9, 6) },
            new EmployeeRoleHistory { EmployeeRoleHistoryId = 7, EmployeeId = 7, RoleId = 7, StartDate = new DateTime(2022, 9, 6) },
            new EmployeeRoleHistory { EmployeeRoleHistoryId = 8, EmployeeId = 8, RoleId = 8, StartDate = new DateTime(2023, 9, 6) },
            new EmployeeRoleHistory { EmployeeRoleHistoryId = 9, EmployeeId = 9, RoleId = 9, StartDate = new DateTime(2024, 9, 6) },
            new EmployeeRoleHistory { EmployeeRoleHistoryId = 10, EmployeeId = 10, RoleId = 10, StartDate = new DateTime(2021, 9, 6) },
            new EmployeeRoleHistory { EmployeeRoleHistoryId = 11, EmployeeId = 11, RoleId = 11, StartDate = new DateTime(2020, 9, 6) },
            new EmployeeRoleHistory { EmployeeRoleHistoryId = 12, EmployeeId = 12, RoleId = 12, StartDate = new DateTime(2022, 9, 6) },
            new EmployeeRoleHistory { EmployeeRoleHistoryId = 13, EmployeeId = 13, RoleId = 13, StartDate = new DateTime(2023, 9, 6) },
            new EmployeeRoleHistory { EmployeeRoleHistoryId = 14, EmployeeId = 14, RoleId = 14, StartDate = new DateTime(2024, 9, 6) },
            new EmployeeRoleHistory { EmployeeRoleHistoryId = 15, EmployeeId = 15, RoleId = 15, StartDate = new DateTime(2022, 9, 6) },
            new EmployeeRoleHistory { EmployeeRoleHistoryId = 16, EmployeeId = 16, RoleId = 16, StartDate = new DateTime(2021, 9, 6) }
        );
        base.OnModelCreating(modelBuilder);
    }
}