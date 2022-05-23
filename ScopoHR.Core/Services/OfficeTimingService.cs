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
   public class OfficeTimingService
    {
        OfficeTiming officeTiming;
        UnitOfWork unitOfWork;
        
        // Constructor 
        public OfficeTimingService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        
        public OfficeTiming Create(OfficeTimingViewModel officeTimingVM, string userName)
        {
            officeTiming = new OfficeTiming
            {
                BranchID = officeTimingVM.BranchID,
                InTime = officeTimingVM.InTime,
                OutTime = officeTimingVM.OutTime,
                ModifiedBy = userName,
                LastModified = DateTime.Now,
                IsDeleted = false
            };
            unitOfWork.OfficeTimingRepository.Insert(officeTiming);
            unitOfWork.Save();
            return officeTiming;
        }
        
        public void Update(OfficeTimingViewModel officeTimingVM, string userName)
        {
            officeTiming = new OfficeTiming
            {
                OfficeTimingId = officeTimingVM.OfficeTimingId,
                BranchID = officeTimingVM.BranchID,
                InTime = officeTimingVM.InTime,
                OutTime = officeTimingVM.OutTime,
                IsDeleted = officeTimingVM.IsDeleted,
                LastModified = DateTime.Now,
                ModifiedBy = userName                
            };
            unitOfWork.OfficeTimingRepository.Update(officeTiming);
            unitOfWork.Save();
        }
        
        public OfficeTimingViewModel GetLast()
        {

            return (from ot in unitOfWork.OfficeTimingRepository.Get()
                    orderby ot.OfficeTimingId descending
                    select new OfficeTimingViewModel
                    {
                        OfficeTimingId=ot.OfficeTimingId,
                        BranchID = ot.BranchID,
                        InTime = ot.InTime,
                        OutTime = ot.OutTime                        
                    }


             ).FirstOrDefault();

            //var o = (from ot in unitOfWork.OfficeTimingRepository.Get()
            //          orderby ot.OfficeTimingId descending
            //          select ot).FirstOrDefault();
        }        

        public List<DropDownViewModel> GetWorkingShiftsDropDown()
        {
            var res = (from w in unitOfWork.WorkingShiftRepository.Get()
                       select new DropDownViewModel
                       {
                           Value = w.Id,
                           Text = w.Name
                       }).ToList();
            return res;
        }
    }
}
