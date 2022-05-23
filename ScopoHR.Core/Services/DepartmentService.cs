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
    public class DepartmentService
    {
        private UnitOfWork unitOfWork;
        private Department department;

        // Constructor
        public DepartmentService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Department Create(DepartmentViewModel DeptVM , int branch, string Name)
        {
            department = new Department
            {
                DepartmentName = DeptVM.DepartmentName,
                DepartmentNameBangla=DeptVM.DepartmentNameBangla,
                BranchID = branch,
                ModifiedBy = Name,
                IsDeleted = false,
                LastModified= DateTime.Now
            };

            unitOfWork.DepartmentRepository.Insert(department);
            unitOfWork.Save();
            return department;
        }

        public void Update(DepartmentViewModel DeptVM, int branch, string Name)
        {
            department = new Department
            {
                DepartmentID = DeptVM.DepartmentID,
                DepartmentName = DeptVM.DepartmentName,
                DepartmentNameBangla = DeptVM.DepartmentNameBangla,
                BranchID = branch,
                ModifiedBy = Name,
                IsDeleted = false,
                LastModified = DateTime.Now
            };
            unitOfWork.DepartmentRepository.Update(department);
            unitOfWork.Save();
        }

        public void Delete(int id)
        {
            department = (
                    from dept in unitOfWork.DepartmentRepository.Get()
                    where dept.DepartmentID == id
                    select dept
                ).SingleOrDefault();

            unitOfWork.DepartmentRepository.Delete(department);
            unitOfWork.Save();
        }

        public List<DepartmentViewModel> GetDropDownList()
        {
            return (
                    from dept in unitOfWork.DepartmentRepository.Get()
                    select new DepartmentViewModel
                    {
                        DepartmentID = dept.DepartmentID,
                        DepartmentName = dept.DepartmentName,
                        DepartmentNameBangla = dept.DepartmentNameBangla,
                    }
                ).ToList();
        }

        public bool IsUnique(DepartmentViewModel departmentVM, int branch)
        {
            IQueryable<int> result;

            if (departmentVM.DepartmentID == 0)
            {
                result = from dep in unitOfWork.DepartmentRepository.Get()
                         where dep.BranchID == branch && dep.DepartmentName.ToLower().Trim() == departmentVM.DepartmentName.ToLower().Trim()
                         select dep.DepartmentID;
            }
            else
            {
                result = from dep in unitOfWork.DepartmentRepository.Get()
                         where dep.BranchID == branch && dep.DepartmentName.ToLower().Trim() == departmentVM.DepartmentName.ToLower().Trim() && dep.DepartmentID != departmentVM.DepartmentID
                         select dep.DepartmentID;
            }

            if (result.Count() > 0)
            {
                return true;
            }
            return false;
        }
        

        public List<DepartmentViewModel> GetAll()
        {

            return (
                    from dep in unitOfWork.DepartmentRepository.Get()
                    orderby dep.DepartmentName ascending
                    select new DepartmentViewModel
                    { DepartmentID = dep.DepartmentID, DepartmentName = dep.DepartmentName,DepartmentNameBangla=dep.DepartmentNameBangla }
                    ).ToList();
        }

        public DepartmentViewModel GetByID(int id)
        {

            return (
                    from dept in unitOfWork.DepartmentRepository.Get()
                    where dept.DepartmentID == id
                    select new DepartmentViewModel
                    {
                        DepartmentID = dept.DepartmentID,
                        DepartmentName = dept.DepartmentName,
                        DepartmentNameBangla = dept.DepartmentNameBangla,
                    }
                ).SingleOrDefault();
        }


    }
}
