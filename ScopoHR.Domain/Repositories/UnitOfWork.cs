using ScopoHR.Domain.IdentityModels;
using ScopoHR.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Repositories
{
    public class UnitOfWork
    {
        private ScopoContext db;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="db">ScopoContext</param>
        public UnitOfWork(ScopoContext db)
        {
            this.db = db;            
        }


        /// <summary>
        /// Save all data to DataBase
        /// </summary>
        public void Save()
        {

            db.SaveChanges();
        }

        private IRepository<Tasks> taskRepo;
        public IRepository<Tasks> TaskRepository
        {
            get
            {
                if (this.taskRepo == null)
                {
                    this.taskRepo = new Repository<Tasks>(db);
                }
                return taskRepo;
            }
        }

        private IRepository<Department> departmentRepo;
        public IRepository<Department> DepartmentRepository
        {
            get
            {
                if (this.departmentRepo == null)
                {
                    this.departmentRepo = new Repository<Department>(db);
                }
                return departmentRepo;
            }
        }


        private IRepository<Designation> designationRepo;
        public IRepository<Designation> DesignationRepository
        {
            get
            {
                if (this.designationRepo == null)
                {
                    this.designationRepo = new Repository<Designation>(db);
                }
                return designationRepo;
            }
        }

        private IRepository<Employee> employeeRepo;
        public IRepository<Employee> EmployeeRepository
        {
            get
            {
                if(this.employeeRepo == null)
                {
                    this.employeeRepo = new Repository<Employee>(db);
                }
                return employeeRepo;
            }
            
        }

        private IRepository<LeaveType> leaveTypeRepo;
        public IRepository<LeaveType> LeaveTypeRepository
        {
            get
            {
                if (this.leaveTypeRepo == null)
                {
                    this.leaveTypeRepo = new Repository<LeaveType>(db);
                }
                return leaveTypeRepo;
            }

        }

        private IRepository<LeaveApplication> leaveAppRepo;
        public IRepository<LeaveApplication> LeaveAppRepository
        {
            get
            {
                if (this.leaveAppRepo == null)
                {
                    this.leaveAppRepo = new Repository<LeaveApplication>(db);
                }
                return leaveAppRepo;
            }

        }


        

        private IRepository<Notice> noticeRepo;
        public IRepository<Notice> NoticeRepository
        {
            get
            {
                if(this.noticeRepo == null)
                {
                    this.noticeRepo = new Repository<Notice>(db);
                }
                return noticeRepo;
            }
        }

        // SalaryType Repository 

        private IRepository<SalaryType> salaryTypeRepo;
        public IRepository<SalaryType> SalaryTypeRepository
        {
            get
            {
                if(this.salaryTypeRepo == null)
                {
                    this.salaryTypeRepo = new Repository<SalaryType>(db);
                }
                return salaryTypeRepo;
            }
        }

        // Salary Mapping Repository 

        private IRepository<SalaryMapping> salaryMappingRepo;
        public IRepository<SalaryMapping> SalaryMappingRepository
        {
            get
            {
                if (this.salaryMappingRepo == null)
                {
                    this.salaryMappingRepo = new Repository<SalaryMapping>(db);
                }
                return salaryMappingRepo;
            }
        }

        // Leave Mapping Repository 

        private IRepository<LeaveMapping> leaveMappingRepo;
        public IRepository<LeaveMapping> LeaveMappingRepository
        {
            get
            {
                if (this.leaveMappingRepo == null)
                {
                    this.leaveMappingRepo = new Repository<LeaveMapping>(db);
                }
                return leaveMappingRepo;
            }
        }

        // Manpower Planning Repository 

        private IRepository<ManpowerPlanning> manpowerPlanningRepo;
        public IRepository<ManpowerPlanning> ManpowerPlanningRepository
        {
            get
            {
                if (this.manpowerPlanningRepo == null)
                {
                    this.manpowerPlanningRepo = new Repository<ManpowerPlanning>(db);
                }
                return manpowerPlanningRepo;
            }
        }

        private IRepository<PublishNotice> publishNoticeRepo;
        public IRepository<PublishNotice> publishNoticeRepository
        {
            get
            {
                if (this.publishNoticeRepo == null)
                {
                    this.publishNoticeRepo = new Repository<PublishNotice>(db);
                }
                return publishNoticeRepo;
            }
        }

        private IRepository<Attendance> attendanceRepo;
        public IRepository<Attendance> attendanceRepository
        {
            get
            {
                if (this.attendanceRepo == null)
                {
                    this.attendanceRepo = new Repository<Attendance>(db);
                }
                return attendanceRepo;
            }
        }


        private IRepository<JobCircular> jobcircularRepo;
        public IRepository<JobCircular> jobcircularRepository
        {
            get
            {
                if (this.jobcircularRepo == null)
                {
                    this.jobcircularRepo = new Repository<JobCircular>(db);
                }
                return jobcircularRepo;
            }
        }


        private IRepository<JobApplication> jobapplicationRepo;
        public IRepository<JobApplication> jobapplicationRepository
        {
            get
            {
                if (this.jobapplicationRepo == null)
                {
                    this.jobapplicationRepo = new Repository<JobApplication>(db);
                }
                return jobapplicationRepo;
            }
        }

       

        private IRepository<YearMapping> yearRepo;
        public IRepository<YearMapping> YearRepository
        {
            get
            {
                if (this.yearRepo == null)
                {
                    this.yearRepo = new Repository<YearMapping>(db);
                }
                return yearRepo;

            }
        }

        //Office timing Repo

        private IRepository<OfficeTiming> officeTimingRepo;
        public IRepository<OfficeTiming> OfficeTimingRepository
        {
            get
            {
                if (this.officeTimingRepo == null)
                {
                    this.officeTimingRepo = new Repository<OfficeTiming>(db);
                }
                return officeTimingRepo;
            }
        }


        private IRepository<Branch> branchRepo;
        public IRepository<Branch> BranchRepository
        {
            get
            {
                if (this.branchRepo == null)
                {
                    this.branchRepo = new Repository<Branch>(db);
                }
                return branchRepo;

            }
        }

        private IRepository<UserBranch> userBranchRepo;
        public IRepository<UserBranch> UserBranchRepository
        {
            get
            {
                if (this.userBranchRepo == null)
                {
                    this.userBranchRepo = new Repository<UserBranch>(db);
                }
                return userBranchRepo;

            }
        }

        private IRepository<ProductionFloorLine> productionFloorLineRepo;
        public IRepository<ProductionFloorLine> productionFloorLineRepository
        {
            get
            {
                if (this.productionFloorLineRepo == null)
                {
                    this.productionFloorLineRepo = new Repository<ProductionFloorLine>(db);
                }
                return productionFloorLineRepo;

            }
        }

        private IRepository<AspNetUsers> appUserRepo;
        public IRepository<AspNetUsers> ApplicationUserRepository
        {
            get
            {
                if(appUserRepo == null)
                {
                    appUserRepo = new Repository<AspNetUsers>(db);
                }
                return appUserRepo;
            }
        }

        private IRepository<AspNetRoles> aspNetRolesRepo;
        public IRepository<AspNetRoles> AspNetRolesRepository
        {
            get
            {
                if (aspNetRolesRepo == null)
                {
                    aspNetRolesRepo = new Repository<AspNetRoles>(db);
                }
                return aspNetRolesRepo;
            }
        }

        private IRepository<AspNetUserRoles> aspNetUserRolesRepo;
        public IRepository<AspNetUserRoles> AspNetUserRolesRepository
        {
            get
            {
                if (aspNetRolesRepo == null)
                {
                    aspNetUserRolesRepo = new Repository<AspNetUserRoles>(db);
                }
                return aspNetUserRolesRepo;
            }
        }

        private IRepository<UserLoginAudit> userLoginAudit;
        public IRepository<UserLoginAudit> UserLoginAuditRepository
        {
            get
            {
                if (userLoginAudit == null)
                {
                    userLoginAudit = new Repository<UserLoginAudit>(db);
                }
                return userLoginAudit;
            }
        }

        private IRepository<MonthlySalary> monthlySalaryRepo;
        public IRepository<MonthlySalary> MonthlySalaryRepository
        {
            get
            {
                if(monthlySalaryRepo == null)
                {
                    monthlySalaryRepo = new Repository<MonthlySalary>(db);
                }
                return monthlySalaryRepo;
            }
        }

        private IRepository<Holiday> holidayRepo;
        public IRepository<Holiday> HolidayRepository
        {
            get
            {
                if(holidayRepo == null)
                {
                    holidayRepo = new Repository<Holiday>(db);
                }
                return holidayRepo;
            }
        }

        private IRepository<DailyAttendance> dailyAttendanceRepo;
        public IRepository<DailyAttendance> DailyAttendanceRepository
        {
            get
            {
                if(dailyAttendanceRepo == null)
                {
                    dailyAttendanceRepo = new Repository<DailyAttendance>(db);
                }
                return dailyAttendanceRepo;
            }
        }

        private IRepository<Client> clientRepo;
        public IRepository<Client> ClientRepository
        {
            get
            {
                if (clientRepo == null)
                {
                    clientRepo = new Repository<Client>(db);
                }
                return clientRepo;
            }
        }


        private IRepository<Project> projectRepo;
        public IRepository<Project> ProjectRepository
        {
            get
            {
                if (projectRepo == null)
                {
                    projectRepo = new Repository<Project>(db);
                }
                return projectRepo;
            }
        }

        private IRepository<Document> documentRepo;
        public IRepository<Document> DocumentRepository
        {
            get
            {
                if (documentRepo == null)
                {
                    documentRepo = new Repository<Document>(db);
                }
                return documentRepo;
            }
        }



        private IRepository<CardNoMapping> cardNoMappingRepo;
        public IRepository<CardNoMapping> CardNoMappingRepository
        {
            get
            {
                if (cardNoMappingRepo == null)
                {
                    cardNoMappingRepo = new Repository<CardNoMapping>(db);
                }
                return cardNoMappingRepo;
            }
        }


        private IRepository<SecurityGuardRoster> securityGuardRosterRepo;
        public IRepository<SecurityGuardRoster> SecurityGuardRosterRepository
        {
            get
            {
                if (securityGuardRosterRepo == null)
                {
                    securityGuardRosterRepo = new Repository<SecurityGuardRoster>(db);
                }
                return securityGuardRosterRepo;
            }
        }

        private IRepository<InactiveEmployee> inactiveEmployeeRepo;
        public IRepository<InactiveEmployee> InactiveEmployeeRepository
        {
            get
            {
                if (this.inactiveEmployeeRepo == null)
                {
                    this.inactiveEmployeeRepo = new Repository<InactiveEmployee>(db);
                }
                return inactiveEmployeeRepo;
            }
        }

        private IRepository<BiometricDevice> biometricDeviceRepo;
        public IRepository<BiometricDevice> BiometricDeviceRepository
        {
            get
            {
                if (this.biometricDeviceRepo == null)
                {
                    this.biometricDeviceRepo = new Repository<BiometricDevice>(db);
                }
                return biometricDeviceRepo;
            }
        }

        private IRepository<WorkingShift> workingShiftRepo;
        public IRepository<WorkingShift> WorkingShiftRepository
        {
            get
            {
                if (this.workingShiftRepo == null)
                {
                    this.workingShiftRepo = new Repository<WorkingShift>(db);
                }
                return workingShiftRepo;
            }
        }

        private IRepository<SalaryIncrement> salaryIncrementRepo;
        public IRepository<SalaryIncrement> SalaryIncrementRepository
        {
            get
            {
                if (this.salaryIncrementRepo == null)
                {
                    this.salaryIncrementRepo = new Repository<SalaryIncrement>(db);
                }
                return salaryIncrementRepo;
            }
        }

        private IRepository<AdvanceSalary> advanceSalaryRepo;
        public IRepository<AdvanceSalary> AdvanceSalaryRepository
        {
            get
            {
                if (this.advanceSalaryRepo == null)
                {
                    this.advanceSalaryRepo = new Repository<AdvanceSalary>(db);
                }
                return advanceSalaryRepo;
            }
        }

        private IRepository<Loan> loanRepo;
        public IRepository<Loan> LoanRepository
        {
            get
            {
                if (this.loanRepo == null)
                {
                    this.loanRepo = new Repository<Loan>(db);
                }
                return loanRepo;
            }
        }

        private IRepository<LoanDetails> loanDetailsRepo;
        public IRepository<LoanDetails> LoanDetailsRepository
        {
            get
            {
                if (this.loanDetailsRepo == null)
                {
                    this.loanDetailsRepo = new Repository<LoanDetails>(db);
                }
                return loanDetailsRepo;
            }
        }
        private IRepository<Tax> taxRepo;
        public IRepository<Tax> TaxRepository
        {
            get
            {
                if (this.taxRepo == null)
                {
                    this.taxRepo = new Repository<Tax>(db);
                }
                return taxRepo;
            }
        }
        private IRepository<Fine> fineRepo;
        public IRepository<Fine> FineRepository
        {
            get
            {
                if (this.fineRepo == null)
                {
                    this.fineRepo = new Repository<Fine>(db);
                }
                return fineRepo;
            }
        }

        private IRepository<Promotion> promotionRepo;
        public IRepository<Promotion> PromotionRepository
        {
            get
            {
                if (this.promotionRepo == null)
                {
                    this.promotionRepo = new Repository<Promotion>(db);
                }
                return promotionRepo;
            }
        }
        private IRepository<License> LicenseRepo;
        public IRepository<License> LicenseRepository
        {
            get
            {
                if (this.LicenseRepo == null)
                {
                    this.LicenseRepo = new Repository<License>(db);
                }
                return LicenseRepo;
            }
        }
        private IRepository<Maternity> maternityRepo;
        public IRepository<Maternity> MaternityRepository
        {
            get
            {
                if (this.maternityRepo == null)
                {
                    this.maternityRepo = new Repository<Maternity>(db);
                }
                return maternityRepo;
            }
        }

        private IRepository<WorkerBus> workerBusRepo;
        public IRepository<WorkerBus> WorkerBusRepository
        {
            get
            {
                if (this.workerBusRepo == null)
                {
                    this.workerBusRepo = new Repository<WorkerBus>(db);
                }
                return workerBusRepo;
            }
        }
        private IRepository<Location> locationRepo;
        public IRepository<Location> LocationRepository
        {
            get
            {
                if (this.locationRepo == null)
                {
                    this.locationRepo = new Repository<Location>(db);
                }
                return locationRepo;
            }
        }

        private IRepository<Sys_DocumentTypes> docTypeRepo;
        public IRepository<Sys_DocumentTypes> DocumentTypesRepository
        {
            get
            {
                if (this.docTypeRepo == null)
                {
                    this.docTypeRepo = new Repository<Sys_DocumentTypes>(db);
                }
                return this.docTypeRepo;
            }
        }
        private IRepository<Sys_Grace> sys_GraceRepo;
        public IRepository<Sys_Grace> Sys_GraceRepository
        {
            get
            {
                if (this.sys_GraceRepo == null)
                {
                    this.sys_GraceRepo = new Repository<Sys_Grace>(db);
                }
                return this.sys_GraceRepo;
            }
        }
        private IRepository<Sys_AttendanceBonus> sys_AttendanceBonusRepo;
        public IRepository<Sys_AttendanceBonus> Sys_AttendanceBonusRepository
        {
            get
            {
                if (this.sys_AttendanceBonusRepo == null)
                {
                    this.sys_AttendanceBonusRepo = new Repository<Sys_AttendanceBonus>(db);
                }
                return this.sys_AttendanceBonusRepo;
            }
        }

        private IRepository<BankSalary> bankSalaryRepo;
        public IRepository<BankSalary> BankSalaryRepository
        {
            get
            {
                if (this.bankSalaryRepo == null)
                {
                    this.bankSalaryRepo = new Repository<BankSalary>(db);
                }
                return this.bankSalaryRepo;
            }
        }
        private IRepository<Prescription> prescriptionRepo;
        public IRepository<Prescription> PrescriptionRepository
        {
            get
            {
                if (this.prescriptionRepo == null)
                {
                    this.prescriptionRepo = new Repository<Prescription>(db);
                }
                return this.prescriptionRepo;
            }
        }
        private IRepository<Medicine> medicineRepo;
        public IRepository<Medicine> MedicineRepository
        {
            get
            {
                if (this.medicineRepo == null)
                {
                    this.medicineRepo = new Repository<Medicine>(db);
                }
                return this.medicineRepo;
            }
        }

        private IRepository<Section> sectionRepo;
        public IRepository<Section> SectionRepository
        {
            get
            {
                if (this.sectionRepo == null)
                {
                    this.sectionRepo = new Repository<Section>(db);
                }
                return this.sectionRepo;
            }
        }
    } // END: Class
}// END: NameSpace
