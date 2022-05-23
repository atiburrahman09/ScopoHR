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
    public class FineService
    {
        private UnitOfWork unitOfWork;
        private Fine fine;

        public FineService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public List<FineViewModel> GetFineByEmployeeID(int employeeID)
        {
            var res = (from f in unitOfWork.FineRepository.Get()
                       where f.EmployeeID == employeeID
                       select new FineViewModel
                       {
                           EmployeeID=f.EmployeeID,
                           Amount=f.Amount,
                           Date=f.Date,
                           FineID=f.FineID,
                           LastModified=f.LastModified
                       }).ToList();
            return res;
        }

        public void SaveFine(FineViewModel fineVM, string name)
        {
            fine = new Fine
            {
                EmployeeID = fineVM.EmployeeID,
                Amount =fineVM.Amount,
                Date=fineVM.Date,
                IsDeleted=false,
                LastModified=DateTime.Now,
                ModifiedBy=name
            };
            unitOfWork.FineRepository.Insert(fine);
            unitOfWork.Save();
        }
    }
}
