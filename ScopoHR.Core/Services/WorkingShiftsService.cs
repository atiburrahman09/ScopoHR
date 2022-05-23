using ScopoHR.Core.ViewModels;
using ScopoHR.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.Services
{
    public class WorkingShiftsService
    {
        private UnitOfWork unitOfWork;

        public WorkingShiftsService(UnitOfWork _uow)
        {
            this.unitOfWork = _uow;
        }

        public IEnumerable<WorkingShiftViewModel> GetAll()
        {
            return this.unitOfWork.WorkingShiftRepository.Get()
                .Select(x => new WorkingShiftViewModel
                {
                    Id = x.Id,
                    InTime = x.InTime,
                    IsDeleted = x.IsDeleted,
                    LastModified = x.LastModified,
                    ModifiedBy = x.ModifiedBy,
                    Name = x.Name,
                    OutTime = x.OutTime
                }).AsEnumerable();                
        }

        public void Create(WorkingShiftViewModel vm)
        {
            unitOfWork.WorkingShiftRepository.Insert(vm.ToEntity());
            unitOfWork.Save();
        }

        public void Update(WorkingShiftViewModel vm)
        {
            unitOfWork.WorkingShiftRepository.Update(vm.ToEntity());
            unitOfWork.Save();
        }

        public WorkingShiftViewModel GetById(int id)
        {
            return this.unitOfWork
                .WorkingShiftRepository
                .Get()
                .Where(x => x.Id == id)
                .Select(x => new WorkingShiftViewModel
                {
                    Id = x.Id,
                    InTime = x.InTime,
                    IsDeleted = x.IsDeleted,
                    LastModified = x.LastModified,
                    ModifiedBy = x.ModifiedBy,
                    Name = x.Name,
                    OutTime = x.OutTime
                }).SingleOrDefault();
        }

        public void Delete(int id)
        {
            var item = unitOfWork
                .WorkingShiftRepository
                .GetById(id);
            unitOfWork.
                WorkingShiftRepository
                .Delete(item);
            unitOfWork.Save();
        }
    }
}
