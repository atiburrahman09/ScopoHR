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
    public class MedicineService
    {
        private UnitOfWork unitOfWork;
        private Medicine medicine;

        public MedicineService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public object GetAllMedicines()
        {
            var res = (from m in unitOfWork.MedicineRepository.Get()
                       select new MedicineViewModel
                       {
                           MedicineID=m.MedicineID,
                           MedicineName=m.MedicineName,
                           UnitPrice=m.UnitPrice
                       }).ToList();

            return res;
        }

        public void CreateMedicine(MedicineViewModel medVM, string name)
        {
            medicine = new Medicine
            {
                MedicineName = medVM.MedicineName,
                UnitPrice = medVM.UnitPrice,
                ModifiedBy = name,
                LastModified = DateTime.Now,
                IsDeleted = false
            };

            unitOfWork.MedicineRepository.Insert(medicine);
            unitOfWork.Save();
        }

        public void UpdateMedicine(MedicineViewModel medVM, string name)
        {
            medicine = new Medicine
            {
                MedicineID=medVM.MedicineID,
                MedicineName = medVM.MedicineName,
                UnitPrice = medVM.UnitPrice,
                ModifiedBy = name,
                LastModified = DateTime.Now,
                IsDeleted = false
            };

            unitOfWork.MedicineRepository.Update(medicine);
            unitOfWork.Save();
        }
        public bool IsUnique(MedicineViewModel medVM)
        {
            IQueryable<int> result;

            if (medVM.MedicineID == 0)
            {
                result = from m in unitOfWork.MedicineRepository.Get()
                         where m.MedicineName.ToLower().Trim() == medVM.MedicineName.ToLower().Trim()
                         select m.MedicineID;
            }
            else
            {
                result = from m in unitOfWork.MedicineRepository.Get()
                         where m.MedicineName.ToLower().Trim() == medVM.MedicineName.ToLower().Trim() && m.MedicineID != medVM.MedicineID
                         select m.MedicineID;
            }

            if (result.Count() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
