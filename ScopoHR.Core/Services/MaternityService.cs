using ScopoHR.Domain.Models;
using ScopoHR.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScopoHR.Core.ViewModels;

namespace ScopoHR.Core.Services
{
    public class MaternityService
    {
        private UnitOfWork unitOfWork;
        private Maternity maternity;

        public MaternityService(UnitOfWork unitOfWork, Maternity maternity)
        {
            this.unitOfWork = unitOfWork;
            this.maternity = maternity;
        }

        public void UpdateMaternity(MaternityViewModel mtVM, string name)
        {
            maternity = new Maternity
            {
                MaternityID=mtVM.MaternityID,
                LeaveApplicationID=mtVM.LeaveApplicationID,
                CardNo = mtVM.CardNo,
                EmployeeID = mtVM.EmployeeID,
                FirstInstallmentAmount = mtVM.FirstInstallmentAmount,
                FirstInstallmentDate = mtVM.FirstInstallmentDate,
                MaternityDuration = mtVM.MaternityDuration,
                MaternityLeaveDate = mtVM.MaternityLeaveDate,
                FirstPaymentDate = mtVM.FirstPaymentDate,
                FirstReceivedDate = mtVM.FirstReceivedDate,
                FirstRequisitionAmount = mtVM.FirstRequisitionAmount,
                FirstRequisitionDate = mtVM.FirstRequisitionDate,
                SecondInstallmentAmount = mtVM.SecondInstallmentAmount,
                SecondInstallmentDate = mtVM.SecondInstallmentDate,
                SecondPaymentDate = mtVM.SecondPaymentDate,
                SecondReceivedDate = mtVM.SecondReceivedDate,
                SecondRequisitionAmount = mtVM.SecondRequisitionAmount,
                SecondRequisitionDate = mtVM.SecondRequisitionDate,
                IsDeleted = false,
                LastModified = DateTime.Now,
                ModifiedBy = name,
                Appx_DelivaryDate=mtVM.Appx_DelivaryDate
            };

            unitOfWork.MaternityRepository.Update(maternity);
            unitOfWork.Save();
        }

        public object GetEmployeeeMaternityDetailsById(int employeeID)
        {
            var res = (from m in unitOfWork.MaternityRepository.Get()
                       join e in unitOfWork.EmployeeRepository.Get()
                       on m.EmployeeID equals e.EmployeeID
                       where m.IsDeleted == false && m.EmployeeID==employeeID
                       select new MaternityViewModel
                       {
                           MaternityID = m.MaternityID,
                           LeaveApplicationID=m.LeaveApplicationID,
                           CardNo = m.CardNo,
                           EmployeeID = m.EmployeeID,
                           EmployeeName = e.EmployeeName,
                           FirstInstallmentAmount = m.FirstInstallmentAmount,
                           FirstInstallmentDate = m.FirstInstallmentDate,
                           FirstPaymentDate = m.FirstPaymentDate,
                           FirstRequisitionDate = m.FirstRequisitionDate,
                           FirstReceivedDate = m.FirstReceivedDate,
                           FirstRequisitionAmount = m.FirstRequisitionAmount,
                           MaternityDuration = m.MaternityDuration,
                           MaternityLeaveDate = m.MaternityLeaveDate,
                           SecondInstallmentAmount = m.SecondInstallmentAmount,
                           SecondInstallmentDate = m.SecondInstallmentDate,
                           SecondPaymentDate = m.SecondPaymentDate,
                           SecondReceivedDate = m.SecondReceivedDate,
                           SecondRequisitionAmount = m.SecondRequisitionAmount,
                           SecondRequisitionDate = m.SecondRequisitionDate,
                           Appx_DelivaryDate=m.Appx_DelivaryDate

                       }).ToList();

            return res;
        }

        public void CreateMaternityFromLeave(int totalPD, decimal? totalSalary, DateTime fromDate, int totalDays, int employeeID,string name,int leaveApplicationID)
        {
            var employeeInfo = (from e in unitOfWork.EmployeeRepository.Get()
                                where e.EmployeeID == employeeID
                                select e).SingleOrDefault();
            maternity = new Maternity
            {
                CardNo = employeeInfo.CardNo,
                EmployeeID = employeeID,
                LeaveApplicationID=leaveApplicationID,
                FirstInstallmentAmount =Math.Round((Convert.ToDecimal(totalSalary) / totalPD) * 56),                          
                FirstRequisitionAmount = Math.Round((Convert.ToDecimal(totalSalary) / totalPD) * 56),
                SecondInstallmentAmount = Math.Round((Convert.ToDecimal(totalSalary) / totalPD) * 56),
                SecondRequisitionAmount = Math.Round((Convert.ToDecimal(totalSalary) / totalPD) * 56),
                MaternityLeaveDate = fromDate,
                MaternityDuration = totalDays,
                IsDeleted = false,
                LastModified = DateTime.Now,
                ModifiedBy = name
            };

            unitOfWork.MaternityRepository.Insert(maternity);
            unitOfWork.Save();
        }

        public void CreateMaternity(MaternityViewModel mtVM, string name)
        {
            var employeeInfo = (from e in unitOfWork.EmployeeRepository.Get()
                                where e.EmployeeID == mtVM.EmployeeID
                                select e).SingleOrDefault();
            maternity = new Maternity
            {
                CardNo = employeeInfo.CardNo,
                EmployeeID = mtVM.EmployeeID,
                FirstInstallmentAmount = mtVM.FirstInstallmentAmount,
                FirstInstallmentDate = mtVM.FirstInstallmentDate,
                MaternityDuration = mtVM.MaternityDuration,
                MaternityLeaveDate = mtVM.MaternityLeaveDate,
                FirstPaymentDate = mtVM.FirstPaymentDate,
                FirstReceivedDate = mtVM.FirstReceivedDate,
                FirstRequisitionAmount = mtVM.FirstRequisitionAmount,
                FirstRequisitionDate = mtVM.FirstRequisitionDate,
                SecondInstallmentAmount = mtVM.SecondInstallmentAmount,
                SecondInstallmentDate = mtVM.SecondInstallmentDate,
                SecondPaymentDate = mtVM.SecondPaymentDate,
                SecondReceivedDate = mtVM.SecondReceivedDate,
                SecondRequisitionAmount = mtVM.SecondRequisitionAmount,
                SecondRequisitionDate = mtVM.SecondRequisitionDate,
                IsDeleted = false,
                LastModified = DateTime.Now,
                ModifiedBy = name
            };

            unitOfWork.MaternityRepository.Insert(maternity);
            unitOfWork.Save();
        }

        public object GetAllMaternities()
        {
            var res = (from m in unitOfWork.MaternityRepository.Get()
                       join e in unitOfWork.EmployeeRepository.Get() 
                       on m.EmployeeID equals e.EmployeeID 
                       where m.IsDeleted == false
                       select new MaternityViewModel
                       {
                           MaternityID=m.MaternityID,
                           LeaveApplicationID=m.LeaveApplicationID,
                           CardNo=m.CardNo,
                           EmployeeID=m.EmployeeID,
                           EmployeeName=e.EmployeeName,
                           FirstInstallmentAmount=m.FirstInstallmentAmount,
                           FirstInstallmentDate=m.FirstInstallmentDate,
                           FirstPaymentDate=m.FirstPaymentDate,
                           FirstRequisitionDate=m.FirstRequisitionDate,
                           FirstReceivedDate=m.FirstReceivedDate,
                           FirstRequisitionAmount=m.FirstRequisitionAmount,
                           MaternityDuration=m.MaternityDuration,
                           MaternityLeaveDate=m.MaternityLeaveDate,
                           SecondInstallmentAmount=m.SecondInstallmentAmount,
                           SecondInstallmentDate=m.SecondInstallmentDate,
                           SecondPaymentDate=m.SecondPaymentDate,
                           SecondReceivedDate=m.SecondReceivedDate,
                           SecondRequisitionAmount=m.SecondRequisitionAmount,
                           SecondRequisitionDate=m.SecondRequisitionDate,
                           Appx_DelivaryDate=m.Appx_DelivaryDate
                           
                       }).ToList();

            return res;
        }

        public object GetEmployeeeMaternityDetailsByCardNo(string cardNo)
        {
            var id = (from e in unitOfWork.EmployeeRepository.Get()
                           where e.CardNo == cardNo
                           select e.EmployeeID).SingleOrDefault();

            var res = (from m in unitOfWork.MaternityRepository.Get()
                       join l in unitOfWork.LeaveAppRepository.Get() on m.LeaveApplicationID equals l.LeaveApplicationID
                       where m.IsDeleted == false && m.EmployeeID == id
                       select new MaternityViewModel
                       {
                           MaternityID = m.MaternityID,
                           FirstInstallmentDate = m.FirstInstallmentDate,
                           SecondInstallmentDate = m.SecondInstallmentDate,
                           MaternityLeaveDate=m.MaternityLeaveDate,
                           FromDate=l.FromDate,
                           ToDate=l.ToDate

                       }).ToList();

            return res;
        }
    }
}
