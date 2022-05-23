using ScopoHR.Domain.IdentityModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class ScopoContext : DbContext
    {
        public ScopoContext(): base()
        {
            var objectContext = (this as IObjectContextAdapter).ObjectContext;

            // Sets the command timeout for all the commands
            objectContext.CommandTimeout = 120;
        }

        #region === Identity ===

        public DbSet<AspNetUsers> AspNetUsers { get; set; }
        public DbSet<AspNetRoles> AspNetRoles { get; set; }
        public DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }

        #endregion === Identity ===

        public DbSet<Tasks> Task { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<LeaveApplication> LeaveApplications { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }        
        public DbSet<Notice> Notices { get; set; }
        public DbSet<SalaryType> SalaryTypes { get; set; }
        public DbSet<SalaryMapping> SalaryMappings { get; set; }
        public DbSet<LeaveMapping> LeaveMappings { get; set; }
        public DbSet<ManpowerPlanning> ManpowerPlannings { get; set; }
        public DbSet<PublishNotice> PublishedNotices { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<JobCircular> JobCirculars { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<OfficeTiming> OfficeTimings { get; set; }
        public DbSet<MonthlySalary> MonthlySalary { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<UserBranch> UserBranches { get; set; }
        public DbSet<ProductionFloorLine> ProductionFloorLines { get; set; }
        public DbSet<YearMapping> YearMappings { get; set; }
        public DbSet<UserLoginAudit> UserLoginAudits{ get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<DailyAttendance> DailyAttendance { get; set; }
        public DbSet<SpecialAttendance> SpecialAttendance { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<CardNoMapping> CardNoMapping { get; set; }
        public DbSet<WorkingShift> WorkingShifts { get; set; }
        public DbSet<SecurityGuardRoster> SecurityGuardRosters { get; set; }
        public DbSet<InactiveEmployee> InactiveEmployees { get; set; }
        public DbSet<BiometricDevice> BiometricDevices { get; set; }
        public DbSet<SalaryIncrement> SalaryIncrements { get; set; }
        public DbSet<AdvanceSalary> AdvanceSalary { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<LoanDetails> LoanDetails { get; set; }
        public DbSet<Tax> Taxes { get; set; }
        public DbSet<Fine> Fines { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<License> Licenses { get; set; }
        public DbSet<Maternity> Maternities { get; set; }
        public DbSet<Sys_DocumentTypes> DocumentTypes { get; set; }
        public DbSet<WorkerBus> WorkerBuses { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<Sys_AttendanceBonus> Sys_AttendanceBonus { get; set; }
        public DbSet<Sys_Grace> Sys_Grace { get; set; }
        public DbSet<BankSalary> BankSalary { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Section> Sections { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<ScopoContext>(null);
            base.OnModelCreating(modelBuilder);
        }
    }
}
