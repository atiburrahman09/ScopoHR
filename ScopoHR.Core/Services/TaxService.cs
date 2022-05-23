using ScopoHR.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScopoHR.Core.ViewModels;
using ScopoHR.Domain.Models;

namespace ScopoHR.Core.Services
{
    public class TaxService
    {
        private UnitOfWork unitOfWork;
        private Tax tax;

        public TaxService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public List<TaxViewModel> GetTaxByEmployeeID(int employeeID)
        {
            var res = (from t in unitOfWork.TaxRepository.Get()
                       where t.EmployeeID == employeeID
                       select new TaxViewModel
                       {
                           EmployeeID = t.EmployeeID,
                           Amount = t.Amount,
                           IsDeleted = t.IsDeleted,
                           LastModified = t.LastModified,
                           ModifiedBy = t.ModifiedBy,
                           TaxID = t.TaxID,
                           Year = t.Year
                       }).ToList();
            return res;
        }

        public void SaveTax(TaxViewModel taxVM, string name)
        {
            tax = new Tax
            {
                EmployeeID = taxVM.EmployeeID,
                Year = taxVM.Year,
                Amount = taxVM.Amount,
                IsDeleted = false,
                LastModified = DateTime.Now,
                ModifiedBy = name
            };
            unitOfWork.TaxRepository.Insert(tax);
            unitOfWork.Save();
        }

        public void Update(TaxViewModel taxVM, string name)
        {
            tax = new Tax
            {
                TaxID = taxVM.TaxID,
                EmployeeID = taxVM.EmployeeID,
                Year = taxVM.Year,
                Amount = taxVM.Amount,
                IsDeleted = false,
                LastModified = DateTime.Now,
                ModifiedBy = name
            };
            unitOfWork.TaxRepository.Update(tax);
            unitOfWork.Save();
        }

        public bool isExists(TaxViewModel taxVM)
        {
            IQueryable<int> result;

            if (taxVM.TaxID == 0)
            {
                result = (from c in unitOfWork.TaxRepository.Get()
                          where c.EmployeeID == taxVM.EmployeeID && taxVM.Year == c.Year
                          select c.TaxID);
            }
            else
            {
                result = (from c in unitOfWork.TaxRepository.Get()
                          where c.EmployeeID == taxVM.EmployeeID && taxVM.Year == c.Year && taxVM.TaxID != c.TaxID
                          select c.TaxID);
            }

            if (result.Count() > 0)
            {
                return true;
            }
            return false;

        }
    }
}
