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
    public class BankAccountService
    {
        private UnitOfWork unitOfWork;
        private BankSalary bankSalary;

        public BankAccountService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public List<BankSalaryViewModel> GetAllBankAccount()
        {
            var res = (from b in unitOfWork.BankSalaryRepository.Get()
                       select new BankSalaryViewModel
                       {
                           CardNo = b.CardNo,
                           EmployeeID = b.EmployeeID,
                           AccountNo = b.AccountNo
                       }).ToList();

            return res;
        }

        public object GetEmployeeeBankAccountDetailsById(int employeeID)
        {
            var res = (from b in unitOfWork.BankSalaryRepository.Get()
                       where b.EmployeeID == employeeID
                       select new BankSalaryViewModel
                       {
                           ID=b.ID,
                           CardNo = b.CardNo,
                           EmployeeID = b.EmployeeID,
                           AccountNo = b.AccountNo,
                           Company = b.Company,
                           BankName = b.BankName
                       }).SingleOrDefault();

            return res;
        }

        public void Save(BankSalaryViewModel bsVM)
        {
            EmployeeViewModel empInfo = (from e in unitOfWork.EmployeeRepository.Get()
                                         join d in unitOfWork.DesignationRepository.Get() on e.DesignationID equals d.DesignationID
                                         where e.EmployeeID == bsVM.EmployeeID
                                         select new EmployeeViewModel
                                         {
                                             EmployeeName = e.EmployeeName,
                                             DepartmentName = d.DesignationName,
                                             EmployeeID = e.EmployeeID,
                                             CardNo = e.CardNo
                                         }).SingleOrDefault();

            if (bsVM.ID > 0)
            {
                bankSalary = new BankSalary
                {
                    ID = bsVM.ID,
                    EmployeeID = empInfo.EmployeeID,
                    EmployeeName = empInfo.EmployeeName,
                    Designation = empInfo.DesignationName,
                    CardNo = empInfo.CardNo,
                    AccountNo = bsVM.AccountNo,
                    Company = bsVM.Company,
                    BankName = bsVM.BankName,
                    ModifiedBy = bsVM.ModifiedBy,
                    LastModified = DateTime.Now,
                    IsDeleted = false
                };

                unitOfWork.BankSalaryRepository.Update(bankSalary);
            }
            else
            {

                bankSalary = new BankSalary
                {
                    EmployeeID = empInfo.EmployeeID,
                    EmployeeName = empInfo.EmployeeName,
                    Designation = empInfo.DesignationName,
                    CardNo = empInfo.CardNo,
                    AccountNo = bsVM.AccountNo,
                    Company = bsVM.Company,
                    BankName = bsVM.BankName,
                    ModifiedBy = bsVM.ModifiedBy,
                    LastModified = DateTime.Now,
                    IsDeleted = false
                };

                unitOfWork.BankSalaryRepository.Insert(bankSalary);
            }

            unitOfWork.Save();
        }

        public bool IsUniqueAccount(int employeeID, string bank)
        {
            var result = (from b in unitOfWork.BankSalaryRepository.Get()
                          where b.EmployeeID==employeeID && b.BankName==bank
                          select b).ToList();

            if (result.Count > 0)
            {
                return true;
            }

            return false;
        }
    }
}
