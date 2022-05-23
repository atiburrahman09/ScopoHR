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
    public class LoanService
    {
        private UnitOfWork unitOfWork;
        private Loan loan;
        private LoanDetails loanDetails;

        public LoanService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public List<AdvanceSalaryViewModel> GetAdvSalaryByCardNo(string cardNo)
        {
            List<AdvanceSalaryViewModel> res = (from s in unitOfWork.AdvanceSalaryRepository.Get()
                                                where s.CardNo == cardNo
                                                select new AdvanceSalaryViewModel
                                                {
                                                    CardNo=s.CardNo,
                                                    Advance=s.Advance,
                                                    Year=s.Year,
                                                    Month=s.Month,
                                                    AdvanceTaken=s.AdvanceTaken,
                                                    LastModified=s.LastModified,
                                                    ModifiedBy=s.ModifiedBy
                                                }).ToList();
            return res;
        }

        public List<LoanVIewModel> GetLoanByEmployeeID(int employeeID)
        {
            List<LoanVIewModel> res = (from l in unitOfWork.LoanRepository.Get()
                                       where l.EmployeeID == employeeID
                                       select new LoanVIewModel
                                       {
                                           EmployeeID=l.EmployeeID,
                                           DisbursementDate=l.DisbursementDate,
                                           LoanAmount=l.LoanAmount,
                                           Duration=l.Duration,
                                           LoanID=l.LoanID,
                                           StartsFrom=l.StartsFrom
                                       }).ToList();
            return res;
        }

        public List<LoanDetailsViewModel> GetLoanDetailsByLoanID(int loanID)
        {
            List<LoanDetailsViewModel> res = (from l in unitOfWork.LoanDetailsRepository.Get()
                                              where l.LoanID == loanID
                                              select new LoanDetailsViewModel
                                              {
                                                  LoanID=l.LoanID,
                                                  RepaymentAmount=l.RepaymentAmount,
                                                  RepaymentDate=l.RepaymentDate,
                                                  LoanDetailsID=l.LoanDetailsID
                                              }).ToList();

            return res;
        }

        //public void SaveAdvanceSalary(AdvanceSalaryViewModel salaryVM, string name)
        //{
        //    advanceSalary = new AdvanceSalary
        //    {
        //        CardNo = salaryVM.CardNo,
        //        Advance = salaryVM.Advance,
        //        Year = salaryVM.Year,
        //        Month = salaryVM.Month,
        //        AdvanceTaken = salaryVM.AdvanceTaken,
        //        LastModified = DateTime.Now,
        //        ModifiedBy = name,
        //        IsDeleted=false
        //    };
        //    unitOfWork.AdvanceSalaryRepository.Insert(advanceSalary);
        //    unitOfWork.Save();
        //}

        public void SaveLoan(LoanVIewModel loanVM, string name)
        {

            var loanAmount = loanVM.LoanAmount;
            //var repaymentMonth = loanVM.StartsFrom;
            var duration = loanVM.Duration;
            DateTime repaymentDate = Convert.ToDateTime(loanVM.StartsFrom + "-"+"01-"+ Convert.ToDateTime(loanVM.DisbursementDate).Year);
            loan = new Loan
            {
                DisbursementDate = loanVM.DisbursementDate,
                Duration = loanVM.Duration,
                EmployeeID = loanVM.EmployeeID,
                LoanAmount = loanVM.LoanAmount,
                StartsFrom = loanVM.StartsFrom,
                ModifiedBy = name,
                LastModified = DateTime.Now,
                IsDeleted = false
            };
            unitOfWork.LoanRepository.Insert(loan);
            unitOfWork.Save();

            for (int i = 0; i < loanVM.Duration; i++)
            {
                loanDetails = new LoanDetails
                {
                    LoanID = loan.LoanID,
                    RepaymentAmount = Math.Round(loanAmount / duration, 0),                    
                    RepaymentDate = repaymentDate
                };

                unitOfWork.LoanDetailsRepository.Insert(loanDetails);
                
                loanAmount =loanAmount- Math.Round(loanAmount / duration, 0);
                duration--;
                //repaymentMonth++;
                repaymentDate = Convert.ToDateTime(repaymentDate).AddMonths(1);
            }

            unitOfWork.Save();

        }
    }
}
