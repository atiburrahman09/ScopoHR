using ScopoHR.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScopoHR.Core.ViewModels;
using ScopoHR.Domain.Models;
using System.Web.Security;
using ScopoHR.Core.Common;
using System.Data;


namespace ScopoHR.Core.Services
{
    
    public class SalaryIncrementService
    {

        private UnitOfWork unitOfWork;
        private SalaryIncrement salaryIncrement;
        public SalaryIncrementService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public List<IncrementReportViewModel> GetSalaryIncrement(ReportFilteringViewModel siVM, int id)
        {
            var query = $"EXEC GetIncrementData '" + siVM.FromDate + "','" + siVM.ToDate + "','" + siVM.Floor + "','" + id + "','" + siVM.ShiftId + "','" + siVM.CardNo + "','" + " " + "','" + siVM.EmployeeType + "'";
            var result = unitOfWork.attendanceRepository.SelectQuery<IncrementReportViewModel>(query).ToList();
            return result;
        }
        public void SaveEmployeeSalaryIncrement(SalaryIncrementViewModel sIVM, string userName)
        {

            if (sIVM.SalaryIncrementID > 0)
            {
                salaryIncrement = (from s in unitOfWork.SalaryIncrementRepository.Get()
                                   where s.SalaryIncrementID == sIVM.SalaryIncrementID && s.EmployeeID == sIVM.EmployeeID
                                   select s).SingleOrDefault();

                //updateSalaryMappings(sIVM.IncrementAmount - salaryIncrement.IncrementAmount, sIVM.EmployeeID,userName);

                salaryIncrement.IncrementDate = sIVM.IncrementDate;
                salaryIncrement.IncrementAmount = sIVM.IncrementAmount;
                salaryIncrement.LastModified = DateTime.Now;
                salaryIncrement.ModifiedBy = userName;

                unitOfWork.SalaryIncrementRepository.Update(salaryIncrement);

            }
            else
            {
                //var total = 0;
                updateSalaryMappings(sIVM.IncrementAmount, sIVM.EmployeeID, userName);
                salaryIncrement = new SalaryIncrement
                {
                    IncrementDate = sIVM.IncrementDate,
                    IncrementAmount = sIVM.IncrementAmount,
                    EmployeeID = sIVM.EmployeeID,
                    IsDeleted = false,
                    LastModified = DateTime.Now,
                    ModifiedBy = userName
                };
                unitOfWork.SalaryIncrementRepository.Insert(salaryIncrement);
            }
            unitOfWork.Save();
        }
        private void updateSalaryMappings(decimal v, int employeeID, string userName)
        {
            decimal totalGross = 0;

            var res = (from s in unitOfWork.SalaryMappingRepository.Get()
                       where s.EmployeeID == employeeID
                       group s by s.EmployeeID into data
                       select new
                       {
                           total = data.Sum(s => s.Amount)
                       }).SingleOrDefault();

            if (res != null)
            {
                totalGross = res.total + v;

                var basicsalary = (from s in unitOfWork.SalaryMappingRepository.Get()
                                   where s.EmployeeID == employeeID && s.SalaryTypeID == 1
                                   select s).FirstOrDefault();
                basicsalary.Amount = Math.Round((totalGross - 1100) / Convert.ToDecimal(1.4));
                basicsalary.LastModified = DateTime.Now;
                basicsalary.ModifiedBy = userName;
                unitOfWork.SalaryMappingRepository.Update(basicsalary);


                var houseRent = (from s in unitOfWork.SalaryMappingRepository.Get()
                                 where s.EmployeeID == employeeID && s.SalaryTypeID == 6
                                 select s).FirstOrDefault();
                houseRent.Amount = Math.Round(((totalGross - 1100) / Convert.ToDecimal(1.4) * Convert.ToDecimal(0.4)));
                houseRent.LastModified = DateTime.Now;
                houseRent.ModifiedBy = userName;
                unitOfWork.SalaryMappingRepository.Update(houseRent);
            }

        }

        public void SaveSalaryIncrement(List<IncrementReportViewModel> sIVM, string name)
        {
            for (int i = 0; i < sIVM.Count(); i++)
            {
                
                salaryIncrement = new SalaryIncrement
                {
                    EmployeeID=sIVM[i].EmployeeID,
                    IncrementAmount=(decimal)sIVM[i].IncrementAmount,
                    IncrementDate=(DateTime)sIVM[i].IncrementDate,
                    IsDeleted=false,
                    LastModified=DateTime.Now,
                    ModifiedBy=name
                };
                unitOfWork.SalaryIncrementRepository.Insert(salaryIncrement);
                updateSalaryMappings((decimal)sIVM[i].IncrementAmount, sIVM[i].EmployeeID, name);
            }
            unitOfWork.Save();
        }

        public List<SalaryIncrementViewModel> GetEmployeeeSalaryIncrementDetailsById(int employeeID)
        {
            List<SalaryIncrementViewModel> res = (from s in unitOfWork.SalaryIncrementRepository.Get()
                                                  join e in unitOfWork.EmployeeRepository.Get() on s.EmployeeID equals e.EmployeeID
                                                  where s.EmployeeID == employeeID
                                                  select new SalaryIncrementViewModel
                                                  {
                                                      SalaryIncrementID = s.SalaryIncrementID,
                                                      EmployeeID = s.EmployeeID,
                                                      IncrementAmount = s.IncrementAmount,
                                                      IncrementDate = s.IncrementDate,
                                                      CardNo = e.CardNo
                                                  }).ToList();
            return res;
        }
    }
}
