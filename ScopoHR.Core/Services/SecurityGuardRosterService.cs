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
    public class SecurityGuardRosterService
    {
        private UnitOfWork unitOfWork;
        private SecurityGuardRoster securityGuardRoster;
        public SecurityGuardRosterService(UnitOfWork uow)
        {
            unitOfWork = uow;
        }

        public void SaveRoster(List<SecurityGuardRosterViewModel> roster, string username)
        {
            List<SecurityGuardRoster> existing = new List<SecurityGuardRoster>();
            DateTime date = roster[0].WorkingDate;
            existing = unitOfWork.SecurityGuardRosterRepository.Get()
                            .Where(x => x.WorkingDate == date)
                            .Select(x => x)
                            .ToList();
            if(existing.Count > 0)
            {
                unitOfWork.SecurityGuardRosterRepository.DeleteRange(existing);
                unitOfWork.Save();
            }

            foreach(var entry in roster)
            {
                securityGuardRoster = new SecurityGuardRoster
                {
                    EmployeeId = entry.EmployeeID,
                    ShiftId = entry.ShiftId,
                    WorkingDate = entry.WorkingDate,
                    PlaceOfDuty = entry.PlaceOfDuty,
                    Remarks = entry.Remarks,
                    IsDeleted = false,
                    LastModified = DateTime.Now,
                    ModifiedBy = username
                };
                unitOfWork.SecurityGuardRosterRepository.Insert(securityGuardRoster);
            }
            unitOfWork.Save();            
        }
        
        public IEnumerable<SecurityGuardRosterViewModel> SearchRoster(DateTime date)
        {
            var res = (from r in unitOfWork.SecurityGuardRosterRepository.Get()
                       join e in unitOfWork.EmployeeRepository.Get()
                       on r.EmployeeId equals e.EmployeeID
                       join d in unitOfWork.DesignationRepository.Get()
                       on e.DesignationID equals d.DesignationID 
                       where r.WorkingDate == date
                       select new SecurityGuardRosterViewModel
                       {
                           EmployeeID = r.EmployeeId,
                           WorkingDate = r.WorkingDate,
                           Id = r.Id,
                           PlaceOfDuty = r.PlaceOfDuty,
                           Remarks = r.Remarks,
                           ShiftId = r.ShiftId,
                           CardNo = e.CardNo,
                           DesignationName = d.DesignationName,
                           EmployeeName=e.EmployeeName
                       }).AsEnumerable();
            return res;    
        }        
    }
}
