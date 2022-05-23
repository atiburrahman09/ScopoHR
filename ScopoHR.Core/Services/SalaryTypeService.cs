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
    public class SalaryTypeService
    {
        private UnitOfWork unitOfWork;
        private SalaryType salaryType;
        public SalaryTypeService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        public SalaryType Create(SalaryTypeViewModel SalaryTypeVM, string userName)
        {
            salaryType = new SalaryType
            {
                SalaryTypeName = SalaryTypeVM.SalaryTypeName,
                ModifiedBy = userName,
                IsDeleted = false,
                LastModified = DateTime.Now                
            };

            unitOfWork.SalaryTypeRepository.Insert(salaryType);
            unitOfWork.Save();
            return salaryType;
        }

        public void Update(SalaryTypeViewModel SalaryTypeVM, string userName)
        {
            salaryType = new SalaryType
            {
                SalaryTypeID = SalaryTypeVM.SalaryTypeID,
                SalaryTypeName = SalaryTypeVM.SalaryTypeName, 
                LastModified = DateTime.Now,
                IsDeleted = false,
                ModifiedBy = userName                
            };

            unitOfWork.SalaryTypeRepository.Update(salaryType);
            unitOfWork.Save();
        }

        public void Delete(int id)
        {
            salaryType = (
                from s in unitOfWork.SalaryTypeRepository.Get()
                where s.SalaryTypeID == id
                select s
                ).SingleOrDefault();

            unitOfWork.SalaryTypeRepository.Delete(salaryType);
            unitOfWork.Save();
        }

        public SalaryTypeViewModel GetByID(int id)
        {
            return (
                from s in unitOfWork.SalaryTypeRepository.Get()
                where s.SalaryTypeID == id
                select new SalaryTypeViewModel
                {
                    SalaryTypeID = s.SalaryTypeID,
                    SalaryTypeName = s.SalaryTypeName                    
                }
                ).SingleOrDefault();
        }

        public List<SalaryTypeViewModel> GetAll()
        {
            return (
                from s in unitOfWork.SalaryTypeRepository.Get()   
                orderby s.SalaryTypeName ascending             
                select new SalaryTypeViewModel
                {
                    SalaryTypeID = s.SalaryTypeID,
                    SalaryTypeName = s.SalaryTypeName
                }
                ).ToList();
        }

        public bool IsExists(SalaryTypeViewModel salaryTypeVM)
        {

            IQueryable<int> result;

            if (salaryTypeVM.SalaryTypeID == 0)
            {
                result = from s in unitOfWork.SalaryTypeRepository.Get()
                         where s.SalaryTypeName.ToLower().Trim() == salaryTypeVM.SalaryTypeName.ToLower().Trim()
                         select s.SalaryTypeID;
            }
            else
            {
                result = from s in unitOfWork.SalaryTypeRepository.Get()
                         where s.SalaryTypeName.ToLower().Trim() == salaryTypeVM.SalaryTypeName.ToLower().Trim() && s.SalaryTypeID != salaryTypeVM.SalaryTypeID
                         select s.SalaryTypeID;
            }

            if (result.Count() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
