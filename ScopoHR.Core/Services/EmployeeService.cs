using ScopoHR.Core.ViewModels;
using ScopoHR.Domain.Models;
using ScopoHR.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.Services
{
    public class EmployeeService
    {
        Employee employee;
        UnitOfWork unitOfWork;
        InactiveEmployee inActiveEmployee;
        SalaryIncrement salaryIncrement;
        SalaryMapping salaryMappings;

        public EmployeeService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public string GenerateCodeNo()
        {
            int length = 4;

            var data = (from emp in unitOfWork.EmployeeRepository.Get()
                        orderby emp.CardNo descending
                        select emp).FirstOrDefault();

            int newCodeNo;

            if (data != null)
            {
                newCodeNo = Convert.ToInt32(data.CardNo) + 1;
            }
            else
            {
                newCodeNo = 1;
            }

            return newCodeNo.ToString().PadLeft(length, '0');
        }

        public object GetDashboardData()
        {
            DashboardViewModel dataVM = new DashboardViewModel();

            dataVM.TotalEmployee = TotalEmployee();

            DateTime firstDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime lastDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);

            dataVM.NewJoin = (from e in unitOfWork.EmployeeRepository.Get()
                           where e.JoinDate >= firstDate && e.JoinDate <= lastDate
                           select e.EmployeeID).Count();

            dataVM.DropOut = (from ia in unitOfWork.InactiveEmployeeRepository.Get()
                           where ia.ApplicableDate >= firstDate && ia.ApplicableDate <= lastDate
                           select ia.EmployeeID).Count();

            dataVM.LeaveApplied = (from l in unitOfWork.LeaveAppRepository.Get()
                                   where l.ApplicationDate >= firstDate && l.ApplicationDate <= lastDate && l.IsDeleted==false
                                   select l.LeaveApplicationID).Count();
           

            return dataVM;
        }
        public int TotalEmployee() {
            return (from e in unitOfWork.EmployeeRepository.Get()
             where e.IsActive == true
             select e.EmployeeID).Count();
        }
        public int TotalEmployee(string floor)
        {
            if (floor == "" || floor == null)
            {
                return (from e in unitOfWork.EmployeeRepository.Get()
                        where e.IsActive == true
                        select e.EmployeeID).Count();
            }
            else {
                return (from e in unitOfWork.EmployeeRepository.Get()
                        join p in unitOfWork.productionFloorLineRepository.Get() on e.ProductionFloorLineID equals p.ProductionFloorLineID
                        where e.IsActive == true && p.Floor==floor
                        select e.EmployeeID).Count();
            }
        }

        public int Create(EmployeeViewModel empVM, int branchId, string userName)
        {
            empVM.ModifiedBy = userName;
            empVM.BranchID = branchId;

            employee = new Employee
            {
                EmployeeName = empVM.EmployeeName,
                CardNo = empVM.CardNo,
                GenderID = empVM.GenderID,
                DesignationID = empVM.DesignationID,
                DepartmentID = empVM.DepartmentID,
                MobileNo = empVM.MobileNo,
                MaritalStatus = empVM.MaritalStatus,
                SpouseName = empVM.SpouseName,
                ProductionFloorLineID = empVM.ProductionFloorLineID,
                JoinDate = empVM.JoinDate == DateTime.MinValue ? DateTime.Now : empVM.JoinDate,

                BranchID = empVM.BranchID,
                ModifiedBy = empVM.ModifiedBy,
                IsDeleted = false,
                LastModified = DateTime.Now,
                IsActive = empVM.IsActive,
                ShiftId = empVM.ShiftId,
                OTApplicable = empVM.OTApplicable ==null ? false : empVM.OTApplicable,
                AttendanceBonusApplicable = empVM.AttendanceBonusApplicable == null ? false : empVM.AttendanceBonusApplicable,
                EmployeeType = empVM.EmployeeType,
                SectionID=empVM.SectionID
            };

            unitOfWork.EmployeeRepository.Insert(employee);
            unitOfWork.Save();

            return employee.EmployeeID;
        }

        public object GetInActiveEmployeeDropDownByKeyword(string keyWord, int branchID)
        {
            var result = (from emp in unitOfWork.EmployeeRepository.Get()
                          where emp.BranchID == branchID && (emp.EmployeeName.ToLower()
                          .Contains(keyWord.ToLower())
                          || emp.CardNo.Contains(keyWord.ToLower()))
                          && emp.IsActive == false
                          select new EmployeeDropDown
                          {
                              CardNo = emp.CardNo,
                              EmployeeName = emp.EmployeeName,
                              EmployeeID = emp.EmployeeID
                          }).ToList();
            return result;
        }

        public void SaveEmployeeFromCSV(List<EmployeeViewModel> empVMList)
        {
            foreach (var empVM in empVMList)
            {
                employee = new Employee
                {
                    //CodeNo=empVM.CodeNo,
                    //EmployeeName=empVM.EmployeeName,
                    //DesignationID=empVM.DesignationID,
                    //DepartmentID=empVM.DepartmentID,
                    //BranchID=empVM.BranchID


                    EmployeeName = empVM.EmployeeName,
                    CardNo = empVM.CardNo,
                    GenderID = 1,
                    DesignationID = empVM.DesignationID,
                    DepartmentID = empVM.DepartmentID,
                    PresentAddress = "",
                    PermanentAddress = "",
                    MobileNo = "",
                    Email = "",
                    NID = "",
                    BloodGroup = "",
                    FatherName = "",
                    MotherName = "",
                    SpouseName = "",
                    ReferenceID = 0,
                    ReportToID = 0,
                    ProductionFloorLineID = empVM.ProductionFloorLineID,
                    BranchID = empVM.BranchID,
                    ModifiedBy = "",
                    IsDeleted = false,
                    JoinDate = DateTime.Now,
                    IsActive = true,
                    OTApplicable = true,
                    AttendanceBonusApplicable = true,
                    EmployeeType = 1
                };
                unitOfWork.EmployeeRepository.Insert(employee);

            }
            unitOfWork.Save();

        }

        public int Update(EmployeeViewModel empVM, int branchId, string userName)
        {
            empVM.ModifiedBy = userName;
            empVM.BranchID = branchId;

            employee = new Employee
            {
                EmployeeID = empVM.EmployeeID,
                EmployeeName = empVM.EmployeeName,
                CardNo = empVM.CardNo,
                GenderID = empVM.GenderID,
                DesignationID = empVM.DesignationID,
                DepartmentID = empVM.DepartmentID,
                PresentAddress = empVM.PresentAddress,
                PermanentAddress = empVM.PermanentAddress,
                MobileNo = empVM.MobileNo,
                Email = empVM.Email,
                NID = empVM.NID,
                BloodGroup = empVM.BloodGroup,
                FatherName = empVM.FatherName,
                MotherName = empVM.MotherName,
                SpouseName = empVM.SpouseName,
                ReferenceID = empVM.ReferenceID ?? null,
                ReportToID = empVM.ReportToID ?? null,
                ProductionFloorLineID = empVM.ProductionFloorLineID,
                BranchID = empVM.BranchID,
                ModifiedBy = userName,
                IsDeleted = empVM.IsDeleted,
                JoinDate = empVM.JoinDate,
                LastModified = DateTime.Now,
                IsActive = empVM.IsActive,
                MaritalStatus = empVM.MaritalStatus,
                DateOfBirth = empVM.DateOfBirth,
                GrandFatherName = empVM.GrandFathername,
                SalaryGrade = empVM.SalaryGrade,
                NomineeName = empVM.NomineeName,
                ShiftId = empVM.ShiftId,
                OTApplicable = empVM.OTApplicable,
                AttendanceBonusApplicable = empVM.AttendanceBonusApplicable,
                TicketNo = empVM.TicketNo,
                EmployeeType = empVM.EmployeeType,
                SectionID = empVM.SectionID
            };

            unitOfWork.EmployeeRepository.Update(employee);
            unitOfWork.Save();

            return employee.EmployeeID;
        }

        public EmployeeViewModel GetByID(int id)
        {
            return (
                    from emp in unitOfWork.EmployeeRepository.Get()
                    join dpt in unitOfWork.DepartmentRepository.Get()
                    on emp.DepartmentID equals dpt.DepartmentID into departmentGroup

                    from dept in departmentGroup.DefaultIfEmpty()
                    join dsg in unitOfWork.DesignationRepository.Get()
                    on emp.DesignationID equals dsg.DesignationID into designationGroup

                    from desg in designationGroup.DefaultIfEmpty()
                    join emp_nominee in unitOfWork.EmployeeRepository.Get()
                    on emp.ReferenceID equals emp_nominee.EmployeeID into nomineeGroup

                    from reference in nomineeGroup.DefaultIfEmpty()
                    join report in unitOfWork.EmployeeRepository.Get()
                    on emp.ReportToID equals report.EmployeeID into reportToGroup

                    from reportTo in reportToGroup.DefaultIfEmpty()
                    where emp.EmployeeID == id
                    select new EmployeeViewModel
                    {
                        EmployeeID = emp.EmployeeID,
                        EmployeeName = emp.EmployeeName,
                        CardNo = emp.CardNo,
                        GenderID = emp.GenderID,
                        DesignationID = emp.DesignationID,
                        DesignationName = desg.DesignationName,
                        DepartmentID = emp.DepartmentID,
                        DepartmentName = dept.DepartmentName,
                        PresentAddress = emp.PresentAddress,
                        PermanentAddress = emp.PermanentAddress,
                        MobileNo = emp.MobileNo,
                        Email = emp.Email,
                        NID = emp.NID,
                        BloodGroup = emp.BloodGroup,
                        FatherName = emp.FatherName,
                        MotherName = emp.MotherName,
                        SpouseName = emp.SpouseName,
                        ReferenceID = emp.ReferenceID,
                        ReferenceName = reference.EmployeeName,
                        NomineeName = emp.NomineeName,
                        ReportToID = emp.ReportToID,
                        ReportToName = reportTo.EmployeeName,
                        ProductionFloorLineID = emp.ProductionFloorLineID,
                        JoinDate = emp.JoinDate,
                        IsActive = emp.IsActive,
                        BranchID = emp.BranchID,
                        MaritalStatus = emp.MaritalStatus,
                        DateOfBirth = emp.DateOfBirth,
                        SalaryGrade = emp.SalaryGrade,
                        GrandFathername = emp.GrandFatherName,
                        ModifiedBy = emp.ModifiedBy,
                        IsDeleted = emp.IsDeleted,
                        ShiftId = emp.ShiftId,
                        OTApplicable = emp.OTApplicable,
                        AttendanceBonusApplicable = emp.AttendanceBonusApplicable,
                        TicketNo = emp.TicketNo,
                        EmployeeType = emp.EmployeeType,
                        SectionID = emp.SectionID
                    }
                ).SingleOrDefault();
        }

        public EmployeeViewModel GetEmployeeInfoByCardNo(string cardNo)
        {
            return (
                    from emp in unitOfWork.EmployeeRepository.Get()
                    join dpt in unitOfWork.DepartmentRepository.Get()
                    on emp.DepartmentID equals dpt.DepartmentID into departmentGroup

                    from dept in departmentGroup.DefaultIfEmpty()
                    join dsg in unitOfWork.DesignationRepository.Get()
                    on emp.DesignationID equals dsg.DesignationID into designationGroup

                    from desg in designationGroup.DefaultIfEmpty()
                    join emp_nominee in unitOfWork.EmployeeRepository.Get()
                    on emp.ReferenceID equals emp_nominee.EmployeeID into nomineeGroup

                    from reference in nomineeGroup.DefaultIfEmpty()
                    join report in unitOfWork.EmployeeRepository.Get()
                    on emp.ReportToID equals report.EmployeeID into reportToGroup

                    from reportTo in reportToGroup.DefaultIfEmpty()
                    where emp.CardNo == cardNo
                    select new EmployeeViewModel
                    {
                        EmployeeID = emp.EmployeeID,
                        EmployeeName = emp.EmployeeName,
                        CardNo = emp.CardNo,
                        GenderID = emp.GenderID,
                        DesignationID = emp.DesignationID,
                        DesignationName = desg.DesignationName,
                        DepartmentID = emp.DepartmentID,
                        DepartmentName = dept.DepartmentName,
                        PresentAddress = emp.PresentAddress,
                        PermanentAddress = emp.PermanentAddress,
                        MobileNo = emp.MobileNo,
                        Email = emp.Email,
                        NID = emp.NID,
                        BloodGroup = emp.BloodGroup,
                        FatherName = emp.FatherName,
                        MotherName = emp.MotherName,
                        SpouseName = emp.SpouseName,
                        ReferenceID = emp.ReferenceID,
                        ReferenceName = reference.EmployeeName,
                        NomineeName = emp.NomineeName,
                        ReportToID = emp.ReportToID,
                        ReportToName = reportTo.EmployeeName,
                        ProductionFloorLineID = emp.ProductionFloorLineID,
                        JoinDate = emp.JoinDate,
                        IsActive = emp.IsActive,
                        BranchID = emp.BranchID,
                        MaritalStatus = emp.MaritalStatus,
                        DateOfBirth = emp.DateOfBirth,
                        SalaryGrade = emp.SalaryGrade,
                        GrandFathername = emp.GrandFatherName,
                        ModifiedBy = emp.ModifiedBy,
                        IsDeleted = emp.IsDeleted,
                        ShiftId = emp.ShiftId,
                        OTApplicable = emp.OTApplicable,
                        AttendanceBonusApplicable = emp.AttendanceBonusApplicable,
                        TicketNo = emp.TicketNo,
                        EmployeeType = emp.EmployeeType,
                        SectionID = emp.SectionID
                    }
                ).SingleOrDefault();
        }

        public List<SectionViewModel> GetAllSectins()
        {
            List<SectionViewModel> list = (from l in unitOfWork.SectionRepository.Get()
                                           select new SectionViewModel
                                           {
                                               SectionID = l.SectionID,
                                               SectionName =l.SectionName,
                                               From=l.From,
                                               To=l.To
                                           }).ToList();

            return list;
        }

        public List<DropDownViewModel> GetEmpByLocation(int locationID)
        {
            var res = (from l in unitOfWork.WorkerBusRepository.Get()
                       join e in unitOfWork.EmployeeRepository.Get() on l.CardNo equals e.CardNo
                       where l.LocationID == locationID
                       select new DropDownViewModel

                       {
                           Text = e.EmployeeName,
                           StringValue = l.CardNo
                       }).ToList();
            return res;
        }

        public List<DropDownViewModel> GetAllEmployees()
        {
            var res = (from e in unitOfWork.EmployeeRepository.Get()
                       where e.IsActive == true
                       select new DropDownViewModel
                       {
                           Text = e.EmployeeName,
                           StringValue = e.CardNo
                       }).ToList();
            return res;
        }

        public void ModifyCardNo(ChangeCardNoViewModel changeCardNoVM)
        {
            // Update Employee
            var cardMappingsInfo = (dynamic)null;
            var employeeInfo = (from e in unitOfWork.EmployeeRepository.Get()
                                where e.CardNo == changeCardNoVM.OldCardNo
                                select e).SingleOrDefault();

            if (employeeInfo.TicketNo == null)
            {
                var totalNo = (from e in unitOfWork.EmployeeRepository.Get()
                               where !e.CardNo.StartsWith("14")
                               select e).Count();

                employeeInfo.TicketNo = totalNo + 1;
            }

            employeeInfo.CardNo = changeCardNoVM.NewCardNo;
            employeeInfo.LastModified = changeCardNoVM.LastModified;
            employeeInfo.ModifiedBy = changeCardNoVM.ModifiedBy;
            employeeInfo.IsActive = true;
            unitOfWork.EmployeeRepository.Update(employeeInfo);

            // Update or Insert into Card Mappings
            string acNo = string.Empty;


            if (!changeCardNoVM.OldCardNo.StartsWith("14"))
            {
                cardMappingsInfo = (from c in unitOfWork.CardNoMappingRepository.Get()
                                    where c.OriginalCardNo == changeCardNoVM.OldCardNo
                                    select c).SingleOrDefault();
            }
            else
            {
                cardMappingsInfo = null;
            }

            if (cardMappingsInfo == null)
            {
                cardMappingsInfo = new CardNoMapping
                {
                    GeneratedCardNo = changeCardNoVM.NewCardNo,
                    OriginalCardNo = changeCardNoVM.NewCardNo,
                    Gender = "",
                    IsDeleted = false,
                    LastModified = changeCardNoVM.LastModified,
                    ModifiedBy = changeCardNoVM.ModifiedBy
                };

                unitOfWork.CardNoMappingRepository.Insert(cardMappingsInfo);
            }
            else
            {
                cardMappingsInfo.OriginalCardNo = changeCardNoVM.NewCardNo;

                unitOfWork.CardNoMappingRepository.Update(cardMappingsInfo);
            }

            // Attendance
            if (changeCardNoVM.OldCardNo.StartsWith("14"))
            {
                var tempCardMapping = (from c in unitOfWork.CardNoMappingRepository.Get()
                                       where c.OriginalCardNo == changeCardNoVM.OldCardNo
                                       select c).SingleOrDefault();

                if (tempCardMapping != null)
                {
                    var attendanceList = (from c in unitOfWork.attendanceRepository.Get()
                                          where c.CardNo == tempCardMapping.GeneratedCardNo
                                          && c.InOutTime >= employeeInfo.JoinDate
                                          select c).ToList();

                    attendanceList.ForEach(x => x.CardNo = cardMappingsInfo.GeneratedCardNo);
                    unitOfWork.attendanceRepository.InsertRange(attendanceList);
                }


            }

            unitOfWork.Save();
        }





        public List<EmployeeViewModel> GetDropDownList(int branchID)
        {
            return (
                from emp in unitOfWork.EmployeeRepository.Get()
                orderby emp.CardNo ascending
                where emp.BranchID == branchID
                select new EmployeeViewModel
                {
                    EmployeeID = emp.EmployeeID,
                    EmployeeName = emp.EmployeeName
                }
                ).ToList();
        }

        public bool IsValidCardNo(string cardNo)
        {
            var result = (from emp in unitOfWork.EmployeeRepository.Get()
                          where emp.CardNo == cardNo
                          select emp).ToList();

            if (result.Count > 0)
            {
                return true;
            }

            return false;
        }

        public int GetEmployeeID(string cardNo)
        {
            var result = (from e in unitOfWork.EmployeeRepository.Get()
                          where e.CardNo == cardNo
                          select e.EmployeeID).Single();
            return result;
        }

        // Keyword is either name or card no in this case
        public List<EmployeeDropDown> GetEmployeeDropDownByKeyword(string keyWord, int branchID)
        {
            var result = (from emp in unitOfWork.EmployeeRepository.Get()
                          where emp.BranchID == branchID && (emp.EmployeeName.ToLower()
                          .Contains(keyWord.ToLower())
                          || emp.CardNo.Contains(keyWord.ToLower()))
                          //&& emp.IsActive == true
                          select new EmployeeDropDown
                          {
                              CardNo = emp.CardNo,
                              EmployeeName = emp.EmployeeName,
                              EmployeeID = emp.EmployeeID
                          }).ToList();
            return result;
        }

        public List<EmployeeDropDown> GetRecentEmployees(int branchId)
        {
            return (from emp in unitOfWork.EmployeeRepository.Get()
                    where emp.BranchID == branchId
                    //&& emp.IsActive == true
                    orderby emp.LastModified descending
                    select new EmployeeDropDown
                    {
                        CardNo = emp.CardNo,
                        EmployeeName = emp.EmployeeName,
                        EmployeeID = emp.EmployeeID
                    }).Take(20).ToList();
        }

        // Security Guards
        public List<EmployeeViewModel> GetAllGuards(int branchId)
        {
            var result = (from emp in unitOfWork.EmployeeRepository.Get()
                          join des in unitOfWork.DesignationRepository.Get()
                          on emp.DesignationID equals des.DesignationID
                          where emp.IsActive == true && emp.DepartmentID == 6
                          select new EmployeeViewModel
                          {
                              CardNo = emp.CardNo,
                              EmployeeName = emp.EmployeeName,
                              EmployeeID = emp.EmployeeID,
                              DesignationName = des.DesignationName
                          }).ToList();
            return result;
        }

        public List<EmployeeDropDown> GetRecentInActiveEmployees(int branchId)
        {
            return (from emp in unitOfWork.EmployeeRepository.Get()
                    where emp.BranchID == branchId
                    && emp.IsActive == false && emp.CardNo != "superuser" && emp.CardNo != "user" && emp.CardNo != "admin" && emp.CardNo != "director" && emp.CardNo != "dataentry" && emp.CardNo != "accounts"
                    orderby emp.LastModified descending
                    select new EmployeeDropDown
                    {
                        CardNo = emp.CardNo,
                        EmployeeName = emp.EmployeeName,
                        EmployeeID = emp.EmployeeID
                    }).Take(20).ToList();
        }
        public InActiveEmployeeViewModel getInActiveEmployeeeDetailsById(int employeeID)
        {
            return (from i in unitOfWork.InactiveEmployeeRepository.Get()
                    where i.EmployeeID == employeeID
                    select new InActiveEmployeeViewModel
                    {
                        ID = i.ID,
                        EmployeeID = i.EmployeeID,
                        Reason = i.Reason,
                        InActiveType = i.InActiveType,
                        ApplicableDate = i.ApplicableDate
                    }).SingleOrDefault();
        }

        public void SaveInactiveEmployeeInfo(InActiveEmployeeViewModel info, int id, string name)
        {
            inActiveEmployee = new InactiveEmployee
            {
                EmployeeID = info.EmployeeID,
                InActiveType = info.InActiveType,
                Reason = info.Reason,
                ModifiedBy = name,
                ApplicableDate = info.ApplicableDate
            };

            unitOfWork.InactiveEmployeeRepository.Insert(inActiveEmployee);

            var empInfo = (from e in unitOfWork.EmployeeRepository.Get()
                           where e.EmployeeID == info.EmployeeID
                           select e).SingleOrDefault();

            empInfo.IsActive = false;
            empInfo.LastModified = DateTime.Now;
            empInfo.ModifiedBy = name;

            unitOfWork.EmployeeRepository.Update(empInfo);

            unitOfWork.Save();

        }

        public void UpdateInactiveEmployeeInfo(InActiveEmployeeViewModel info, int id, string name)
        {
            inActiveEmployee = new InactiveEmployee
            {
                ID = info.ID,
                EmployeeID = info.EmployeeID,
                InActiveType = info.InActiveType,
                Reason = info.Reason,
                ModifiedBy = name,
                ApplicableDate = info.ApplicableDate
            };

            unitOfWork.InactiveEmployeeRepository.Update(inActiveEmployee);
            unitOfWork.Save();
        }
    }
}
