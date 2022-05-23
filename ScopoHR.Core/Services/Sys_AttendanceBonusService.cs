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
    public class Sys_AttendanceBonusService
    {
        private Sys_AttendanceBonus bonus;
        private UnitOfWork unitOfWork;
        public Sys_AttendanceBonusService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void CreateAttendanceBonus(Sys_AttendanceBonusViewModel model,string name)
        {
            bonus = new Sys_AttendanceBonus
            {
                AttendanceBonus = model.AttendanceBonus,
                IsApplicable = model.IsApplicable,
                IsDeleted = false,
                LastModified = DateTime.Now,
                ModifiedBy = name
            };
            unitOfWork.Sys_AttendanceBonusRepository.Insert(bonus);
            unitOfWork.Save();
        }

        public void UpdateAttendanceBonus(Sys_AttendanceBonusViewModel model,string name)
        {
            bonus = new Sys_AttendanceBonus
            {
                AttendanceBonusID=model.AttendanceBonusID,
                AttendanceBonus = model.AttendanceBonus,
                IsApplicable = model.IsApplicable,
                IsDeleted = false,
                LastModified = DateTime.Now,
                ModifiedBy = name
            };
            unitOfWork.Sys_AttendanceBonusRepository.Update(bonus);
            unitOfWork.Save();
        }

        public object GetAttendanceBonusData()
        {
            var res = (from a in unitOfWork.Sys_AttendanceBonusRepository.Get()
                       select a).SingleOrDefault();
            return res;
        }
    }
}
