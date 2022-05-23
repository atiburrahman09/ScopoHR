using ScopoHR.Domain.Repositories;
using ScopoHR.Core.ViewModels;
using ScopoHR.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.Services
{
    public class LeaveTypeService
    {
        private LeaveType leaveType;
        private UnitOfWork unitOfWork;
        public LeaveTypeService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public LeaveType Create(LeaveTypeViewModel LTypeVM)
        {
            leaveType = new LeaveType
            {
                LeaveTypeName = LTypeVM.LeaveTypeName,
                IsForward = LTypeVM.IsForward,
                LeaveDays = LTypeVM.LeaveDays,
                IsPayable=LTypeVM.IsPayableToNextYear,
                IsDeleted=false,
                ModifiedBy=LTypeVM.ModifiedBy,
                LastModified=DateTime.Now
            };

            unitOfWork.LeaveTypeRepository.Insert(leaveType);
            unitOfWork.Save();
            return leaveType;
        }

        public void Update(LeaveTypeViewModel LtypeVM)
        {
            leaveType = new LeaveType
            {
                LeaveTypeID = LtypeVM.LeaveTypeID,
                LeaveTypeName = LtypeVM.LeaveTypeName,
                LeaveDays = LtypeVM.LeaveDays,
                IsForward = LtypeVM.IsForward,
                IsPayable = LtypeVM.IsPayableToNextYear,
                IsDeleted = false,
                ModifiedBy = LtypeVM.ModifiedBy,
                LastModified = DateTime.Now
            };
            unitOfWork.LeaveTypeRepository.Update(leaveType);
            unitOfWork.Save();
        }

        public List<LeaveTypeViewModel> GetAll()
        {
            return (
                from l in unitOfWork.LeaveTypeRepository.Get()
                orderby l.LeaveTypeName                
                select new LeaveTypeViewModel
                {
                    LeaveTypeID = l.LeaveTypeID,
                    LeaveTypeName = l.LeaveTypeName,
                    LeaveDays = l.LeaveDays,
                    IsForward = l.IsForward,
                    IsPayableToNextYear=l.IsPayable
                }
                ).ToList();
        }

        public LeaveTypeViewModel GetByID(int id)
        {
            return (
                from l in unitOfWork.LeaveTypeRepository.Get()
                where l.LeaveTypeID == id
                select new LeaveTypeViewModel
                {
                    LeaveTypeID = l.LeaveTypeID,
                    LeaveTypeName = l.LeaveTypeName
                }).SingleOrDefault();
        }

        public void Delete(int id)
        {
            leaveType = (from l in unitOfWork.LeaveTypeRepository.Get()
                         where l.LeaveTypeID == id
                         select l).SingleOrDefault();
            unitOfWork.LeaveTypeRepository.Delete(leaveType);
            unitOfWork.Save();
        }

        public List<LeaveDaysViewModel> GetAllWithData()
        {
            var res = (from l in unitOfWork.LeaveTypeRepository.Get()
                       select new LeaveDaysViewModel
                       {
                           LeaveDays = l != null ? l.LeaveDays : 0,
                           LeaveTypeID = l.LeaveTypeID,
                           LeaveTypeName = l.LeaveTypeName,
                       }
                      ).ToList();
            return res;
        }

        public bool IsExists(LeaveTypeViewModel leaveTypeVM)
        {
            IQueryable<int> result;

            if (leaveTypeVM.LeaveTypeID == 0)
            {
                result = from l in unitOfWork.LeaveTypeRepository.Get()
                         where l.LeaveTypeName.ToLower().Trim() == leaveTypeVM.LeaveTypeName.ToLower().Trim()
                         select l.LeaveTypeID;
            }
            else
            {
                result = from l in unitOfWork.LeaveTypeRepository.Get()
                         where l.LeaveTypeName.ToLower().Trim() == leaveTypeVM.LeaveTypeName.ToLower().Trim() && l.LeaveTypeID != leaveTypeVM.LeaveTypeID
                         select l.LeaveTypeID;
            }

            if (result.Count() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
