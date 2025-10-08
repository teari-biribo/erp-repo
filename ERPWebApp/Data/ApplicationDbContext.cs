
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<OrgUnit> OrganizationalUnits { get; set; }
    public DbSet<RoleReporting> RoleReportings { get; set; }
    public DbSet<EmployeeRoleHistory> EmployeeRoleHistory { get; set; }

    //Workflows
    // public DbSet<Workflow> Workflows => Set<Workflow>();
    // public DbSet<WorkflowStep> WorkflowSteps => Set<WorkflowStep>();
    // public DbSet<WorkflowInstance> WorkflowInstances => Set<WorkflowInstance>();
    // public DbSet<WorkflowInstanceStep> WorkflowInstanceSteps => Set<WorkflowInstanceStep>();
    public DbSet<Workflow> Workflows { get; set; }
    public DbSet<WorkflowStep> WorkflowSteps { get; set; }
    public DbSet<WorkflowInstance> WorkflowInstances { get; set; }
    public DbSet<WorkflowInstanceStep> WorkflowInstanceSteps { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Relationship between ApplicationUser and Employee
        modelBuilder.Entity<ApplicationUser>()
            .HasOne(u => u.Employee)
            .WithMany()
            .HasForeignKey(u => u.EmployeeId)
            .OnDelete(DeleteBehavior.SetNull);

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

        // Workflow 
        modelBuilder.Entity<Workflow>(entity =>
        {
            entity.ToTable("Workflows");
            entity.HasKey(w => w.Id);
            entity.Property(w => w.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            entity.Property(w => w.Description)
                    .HasMaxLength(350);
            entity.HasMany(w => w.Steps)
                    .WithOne(s => s.Workflow)
                    .HasForeignKey(s => s.WorkflowId)
                    .OnDelete(DeleteBehavior.Cascade);
        });

        // WorkflowStep
        modelBuilder.Entity<WorkflowStep>(entity =>
        {
            entity.ToTable("WorkflowSteps");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.StepOrder).IsRequired();
            entity.Property(s => s.StepName)
                    .IsRequired()
                    .HasMaxLength(100);
            entity.Property(s => s.RoleName)
                    .IsRequired()
                    .HasMaxLength(100);

            // Each step order unique within a workflow
            entity.HasIndex(s => new { s.WorkflowId, s.StepOrder })
                    .IsUnique();
        });

        // WorkflowInstance
        modelBuilder.Entity<WorkflowInstance>(entity =>
        {
            entity.ToTable("WorkflowInstances");
            entity.HasKey(i => i.Id);
            entity.Property(i => i.Status)
                    .IsRequired()
                    .HasMaxLength(50);
            entity.Property(i => i.EntityType)
                    .IsRequired()
                    .HasMaxLength(100);
            // entity.Property(i => i.CreatedAt)
            //         .HasColumnType("timestamp without time zone");
            entity.HasMany(i => i.Steps)
                    .WithOne(s => s.WorkflowInstance)
                    .HasForeignKey(s => s.WorkflowInstanceId)
                    .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(i => i.Workflow)
                    .WithMany()
                    .HasForeignKey(i => i.WorkflowId)
                    .OnDelete(DeleteBehavior.Restrict);
        });

        // WorkflowInstanceStep
        modelBuilder.Entity<WorkflowInstanceStep>(entity =>
        {
            entity.ToTable("WorkflowInstanceSteps");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Status)
                    .IsRequired()
                    .HasMaxLength(50);
            entity.Property(s => s.AssignedUserId)
                    .IsRequired()
                    .HasMaxLength(450);
            entity.Property(s => s.Comments)
                    .HasMaxLength(1000);
            // entity.Property(i => i.ActionDate)
            //         .HasColumnType("timestamp without time zone");
            entity.HasOne(s => s.WorkflowStep)
                    .WithMany()
                    .HasForeignKey(s => s.WorkflowStepId)
                    .OnDelete(DeleteBehavior.Restrict);
        });

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

        // Seed a sample workflow instance
        // modelBuilder.Entity<WorkflowInstance>().HasData(
        //     new WorkflowInstance
        //     {
        //         Id = 1,
        //         WorkflowId = 1, // "Budget Request Approval"
        //         EntityType = "BudgetApproval",
        //         EntityId = 1001, // hypothetical Request ID
        //         Status = "InProgress",
        //         CreatedAt = new DateTime(2025, 9, 6)
        //     }
        // );

        // // Seed workflow instance steps
        // modelBuilder.Entity<WorkflowInstanceStep>().HasData(
        //     new WorkflowInstanceStep
        //     {
        //         Id = 1,
        //         WorkflowInstanceId = 1,
        //         WorkflowStepId = 1, // Supervisor Approval
        //         AssignedUserId = "2", // Bob Johnson (assuming ApplicationUser.Id matches EmployeeId)
        //         Status = "Pending",
        //         Comments = null,
        //     },
        //     new WorkflowInstanceStep
        //     {
        //         Id = 2,
        //         WorkflowInstanceId = 1,
        //         WorkflowStepId = 2, // Department Head Approval
        //         AssignedUserId = "12", // Laura Hollis
        //         Status = "NotStarted",
        //         Comments = null
        //     },
        //     new WorkflowInstanceStep
        //     {
        //         Id = 3,
        //         WorkflowInstanceId = 1,
        //         WorkflowStepId = 3, // Finance Approval
        //         AssignedUserId = "1", // Alice Smith
        //         Status = "NotStarted",
        //         Comments = null
        //     }
        // );

        // //------MIGRATION 2--------
        // 3. Seed Employees (requires RoleId - ensure orgunits and roles is already seeded)
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

        // 7. Seed sample workflow and steps
        // modelBuilder.Entity<Workflow>().HasData(
        //     new Workflow
        //     {
        //         Id = 1,
        //         Name = "Budget Approval",
        //         Description = "Approval workflow for budget approval requiring multiple management approvals."
        //     }
        // );

        // modelBuilder.Entity<WorkflowStep>().HasData(
        //     new WorkflowStep
        //     {
        //         Id = 1,
        //         WorkflowId = 1,
        //         StepOrder = 1,
        //         StepName = "Supervisor Approval",
        //         RoleName = "Supervisor West"
        //     },
        //     new WorkflowStep
        //     {
        //         Id = 2,
        //         WorkflowId = 1,
        //         StepOrder = 2,
        //         StepName = "Department Head Approval",
        //         RoleName = "National Sales Manager Machinery"
        //     },
        //     new WorkflowStep
        //     {
        //         Id = 3,
        //         WorkflowId = 1,
        //         StepOrder = 3,
        //         StepName = "Finance Approval",
        //         RoleName = "Senior IT Officer"
        //     }
        // );
    }
}