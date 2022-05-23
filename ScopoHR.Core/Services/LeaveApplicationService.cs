using ScopoHR.Core.Helpers;
using ScopoHR.Core.ViewModels;
using ScopoHR.Domain.Models;
using ScopoHR.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.Services
{
    public class LeaveApplicationService
    {
        private UnitOfWork unitOfWork;
        private LeaveApplication leaveApp;

        public LeaveApplicationService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public int Create(LeaveApplicationViewModel LeaveAppVM, string userName)
        {
            leaveApp = new LeaveApplication
            {

                LeaveTypeID = LeaveAppVM.LeaveTypeID,
                EmployeeID = LeaveAppVM.EmployeeID,
                FromDate = LeaveAppVM.FromDate,
                ToDate = LeaveAppVM.ToDate,
                ApplicationDate = LeaveAppVM.ApplicationDate,
                ReasonOfLeave = LeaveAppVM.ReasonOfLeave,
                ModifiedBy = userName,
                LastModified = DateTime.Now,
                IsDeleted = false,
                Status = (int)LeaveApplicationStatus.Pending,
                SubstituteDate = LeaveAppVM.SubstituteDate
            };

            unitOfWork.LeaveAppRepository.Insert(leaveApp);
            unitOfWork.Save();
            return leaveApp.LeaveApplicationID;
        }

        public void Update(LeaveApplicationViewModel LeaveAppVM, string userName)
        {
            leaveApp = new LeaveApplication
            {
                LeaveApplicationID = LeaveAppVM.LeaveApplicationID,
                LeaveTypeID = LeaveAppVM.LeaveTypeID,
                EmployeeID = LeaveAppVM.EmployeeID,
                FromDate = LeaveAppVM.FromDate,
                ToDate = LeaveAppVM.ToDate,
                Status = (int)LeaveAppVM.Status,
                ApplicationDate = LeaveAppVM.ApplicationDate,
                ApproavedBy = LeaveAppVM.ApproavedBy,
                ApprovalDate = LeaveAppVM.ApprovalDate,
                ReasonOfLeave = LeaveAppVM.ReasonOfLeave,
                IsDeleted = LeaveAppVM.IsDeleted,
                LastModified = DateTime.Now,
                ModifiedBy = userName,
                SubstituteDate = LeaveAppVM.SubstituteDate

            };

            unitOfWork.LeaveAppRepository.Update(leaveApp);
            unitOfWork.Save();
        }

        public void Update(List<LeaveApplicationViewModel> appList, string statusBy)
        {
            foreach (var LeaveAppVM in appList)
            {
                leaveApp = new LeaveApplication
                {
                    LeaveApplicationID = LeaveAppVM.LeaveApplicationID,
                    LeaveTypeID = LeaveAppVM.LeaveTypeID,
                    EmployeeID = LeaveAppVM.EmployeeID,
                    FromDate = LeaveAppVM.FromDate,
                    ToDate = LeaveAppVM.ToDate,
                    Status = (int)LeaveAppVM.Status,
                    ApplicationDate = LeaveAppVM.ApplicationDate,
                    ApproavedBy = statusBy,
                    ApprovalDate = LeaveAppVM.ApprovalDate,
                    ReasonOfLeave = LeaveAppVM.ReasonOfLeave,
                    IsDeleted = LeaveAppVM.IsDeleted,
                    ModifiedBy = statusBy,
                    LastModified = DateTime.Now,
                    SubstituteDate = LeaveAppVM.SubstituteDate
                };
                unitOfWork.LeaveAppRepository.Update(leaveApp);
            }
            unitOfWork.Save();
        }

        public void Delete(int id)
        {
            leaveApp = (
                from l in unitOfWork.LeaveAppRepository.Get()
                where l.LeaveApplicationID == id
                select l
                ).SingleOrDefault();

            unitOfWork.LeaveAppRepository.Delete(leaveApp);
            unitOfWork.Save();
        }

        public LeaveApplicationViewModel GetByID(int id)
        {
            return (
                from l in unitOfWork.LeaveAppRepository.Get()
                where l.LeaveApplicationID == id
                join emp in unitOfWork.EmployeeRepository.Get() on l.EmployeeID equals emp.EmployeeID
                join dsg in unitOfWork.DesignationRepository.Get() on emp.DesignationID equals dsg.DesignationID
                join ltype in unitOfWork.LeaveTypeRepository.Get() on l.LeaveTypeID equals ltype.LeaveTypeID
                select new LeaveApplicationViewModel
                {
                    LeaveApplicationID = l.LeaveApplicationID,
                    EmployeeID = l.EmployeeID,
                    EmployeeName = emp.EmployeeName,
                    Designation = dsg.DesignationName,
                    CardNo = emp.CardNo,
                    LeaveTypeID = ltype.LeaveTypeID,
                    FromDate = l.FromDate,
                    ToDate = l.ToDate,
                    Status = (LeaveApplicationStatus)l.Status,
                    ApproavedBy = l.ApproavedBy,
                    ApprovalDate = l.ApprovalDate,
                    ApplicationDate = l.ApplicationDate,
                    IsDeleted = l.IsDeleted,
                    ReasonOfLeave = l.ReasonOfLeave,
                    TotalDays = DbFunctions.DiffDays(l.FromDate, l.ToDate).Value + 1,
                    SubstituteDate = l.SubstituteDate


                }
                ).SingleOrDefault();
        }

        public object GetAllAppsByEmployeeCardNo(int branchId, string name, string cardNo)
        {
            var result = (
              from l in unitOfWork.LeaveAppRepository.Get()
              join emp in unitOfWork.EmployeeRepository.Get() on l.EmployeeID equals emp.EmployeeID
              join dsg in unitOfWork.DesignationRepository.Get() on emp.DesignationID equals dsg.DesignationID
              join ltype in unitOfWork.LeaveTypeRepository.Get() on l.LeaveTypeID equals ltype.LeaveTypeID
              where emp.BranchID == branchId && l.Status == 0 && l.IsDeleted == false && emp.CardNo == cardNo
              orderby l.ApplicationDate descending
              select new LeaveApplicationViewModel
              {
                  LeaveApplicationID = l.LeaveApplicationID,
                  EmployeeID = l.EmployeeID,
                  EmployeeName = emp.EmployeeName,
                  Designation = dsg.DesignationName,
                  CardNo = emp.CardNo,
                  LeaveTypeID = ltype.LeaveTypeID,
                  LeaveTypeName = ltype.LeaveTypeName,
                  FromDate = l.FromDate,
                  ToDate = l.ToDate,
                  TotalDays = DbFunctions.DiffDays(l.FromDate, l.ToDate).Value + 1,
                  Status = (LeaveApplicationStatus)l.Status,
                  ApproavedBy = l.ApproavedBy,
                  ApprovalDate = l.ApprovalDate,
                  ApplicationDate = l.ApplicationDate,
                  ReasonOfLeave = l.ReasonOfLeave,
                  IsDeleted = l.IsDeleted,
                  SubstituteDate = l.SubstituteDate
              }
              ).ToList();

            return result;
        }

        public bool DuplicateLeaveCheck(int employeeID, DateTime fromDate, DateTime toDate)
        {
            var res = (from l in unitOfWork.LeaveAppRepository.Get()
                       where l.EmployeeID == employeeID && l.FromDate >= fromDate && l.FromDate <= toDate && l.ToDate >= fromDate && l.ToDate <= toDate
                       select l).ToList();

            if (res.Count() > 0)
            {
                return true;
            }
            else return false;
        }

        public object DeleteApplication(int appID)
        {
            var appInfo = (from l in unitOfWork.LeaveAppRepository.Get()
                           where l.LeaveApplicationID == appID
                           select l).SingleOrDefault();
            appInfo.IsDeleted = true;
            unitOfWork.LeaveAppRepository.Update(appInfo);
            unitOfWork.Save();

            return true;
        }

        public List<LeaveApplicationViewModel> GetAll(int branchId, string userName)
        {
            //DateTime date = Convert.ToDateTime("11/01/2017");

            var result = (
                from l in unitOfWork.LeaveAppRepository.Get()
                join emp in unitOfWork.EmployeeRepository.Get() on l.EmployeeID equals emp.EmployeeID
                join dsg in unitOfWork.DesignationRepository.Get() on emp.DesignationID equals dsg.DesignationID
                join ltype in unitOfWork.LeaveTypeRepository.Get() on l.LeaveTypeID equals ltype.LeaveTypeID
                where emp.BranchID == branchId && l.Status == 0 && l.IsDeleted == false
                orderby l.ApplicationDate ascending
                select new LeaveApplicationViewModel
                {
                    LeaveApplicationID = l.LeaveApplicationID,
                    EmployeeID = l.EmployeeID,
                    EmployeeName = emp.EmployeeName,
                    Designation = dsg.DesignationName,
                    CardNo = emp.CardNo,
                    LeaveTypeID = ltype.LeaveTypeID,
                    LeaveTypeName = ltype.LeaveTypeName,
                    FromDate = l.FromDate,
                    ToDate = l.ToDate,
                    TotalDays = DbFunctions.DiffDays(l.FromDate, l.ToDate).Value + 1,
                    Status = (LeaveApplicationStatus)l.Status,
                    ApproavedBy = l.ApproavedBy,
                    ApprovalDate = l.ApprovalDate,
                    ApplicationDate = l.ApplicationDate,
                    ReasonOfLeave = l.ReasonOfLeave,
                    IsDeleted = l.IsDeleted,
                    SubstituteDate = l.SubstituteDate
                }
                ).Take(50).ToList();

            return result;
        }

        public List<LeaveApplicationViewModel> GetAll(LeaveApplicationStatus status, int branchId, int take, int skip, out int count)
        {
            count = (
                from l in unitOfWork.LeaveAppRepository.Get()
                join emp in unitOfWork.EmployeeRepository.Get() on l.EmployeeID equals emp.EmployeeID
                join dsg in unitOfWork.DesignationRepository.Get() on emp.DesignationID equals dsg.DesignationID
                join ltype in unitOfWork.LeaveTypeRepository.Get() on l.LeaveTypeID equals ltype.LeaveTypeID
                join res in unitOfWork.EmployeeRepository.Get() on emp.ReportToID equals res.EmployeeID into res_group
                from repTo in res_group.DefaultIfEmpty()
                where l.Status == (int)status && emp.BranchID == branchId && l.IsDeleted == false
                orderby l.ApplicationDate ascending
                select new LeaveApplicationViewModel
                {
                    LeaveApplicationID = l.LeaveApplicationID,
                    EmployeeID = l.EmployeeID,
                    EmployeeName = emp.EmployeeName,
                    Designation = dsg.DesignationName,
                    CardNo = emp.CardNo,
                    LeaveTypeID = ltype.LeaveTypeID,
                    LeaveTypeName = ltype.LeaveTypeName,
                    FromDate = l.FromDate,
                    ToDate = l.ToDate,
                    TotalDays = DbFunctions.DiffDays(l.FromDate, l.ToDate).Value + 1,
                    Status = (LeaveApplicationStatus)l.Status,
                    ApproavedBy = l.ApproavedBy,
                    ApprovalDate = l.ApprovalDate,
                    ApplicationDate = l.ApplicationDate,
                    ReasonOfLeave = l.ReasonOfLeave,
                    IsDeleted = l.IsDeleted,
                    SubstituteDate = l.SubstituteDate
                }
                ).Count();

            var result = (
                from l in unitOfWork.LeaveAppRepository.Get()
                join emp in unitOfWork.EmployeeRepository.Get() on l.EmployeeID equals emp.EmployeeID
                join dsg in unitOfWork.DesignationRepository.Get() on emp.DesignationID equals dsg.DesignationID
                join ltype in unitOfWork.LeaveTypeRepository.Get() on l.LeaveTypeID equals ltype.LeaveTypeID
                join res in unitOfWork.EmployeeRepository.Get() on emp.ReportToID equals res.EmployeeID into res_group
                from repTo in res_group.DefaultIfEmpty()
                where l.Status == (int)status && emp.BranchID == branchId
                orderby l.ApplicationDate ascending
                select new LeaveApplicationViewModel
                {
                    LeaveApplicationID = l.LeaveApplicationID,
                    EmployeeID = l.EmployeeID,
                    EmployeeName = emp.EmployeeName,
                    Designation = dsg.DesignationName,
                    CardNo = emp.CardNo,
                    LeaveTypeID = ltype.LeaveTypeID,
                    LeaveTypeName = ltype.LeaveTypeName,
                    FromDate = l.FromDate,
                    ToDate = l.ToDate,
                    TotalDays = DbFunctions.DiffDays(l.FromDate, l.ToDate).Value + 1,
                    Status = (LeaveApplicationStatus)l.Status,
                    ApproavedBy = l.ApproavedBy,
                    ApprovalDate = l.ApprovalDate,
                    ApplicationDate = l.ApplicationDate,
                    ReasonOfLeave = l.ReasonOfLeave,
                    SubstituteDate = l.SubstituteDate
                }
                ).Skip(skip).Take(take).ToList();
            return result;
        }

    }
}
