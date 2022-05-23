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
    public class DesignationService
    {
        private UnitOfWork unitOfWork;
        private Designation designation;
        // Constructor
        public DesignationService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Designation Create(DesignationViewModel DesgVM)
        {
            designation = new Designation
            {
                DesignationName = DesgVM.DesignationName,
                DesignationNameBangla=DesgVM.DesignationNameBangla,
                DepartmentID=DesgVM.DepartmentID,
                IsOverTimeApplicable=DesgVM.IsOverTimeApplicable,
                ModifiedBy=DesgVM.ModifiedBy,
                LastModified=DateTime.Now,
                IsDeleted=false
            };

            unitOfWork.DesignationRepository.Insert(designation);
            unitOfWork.Save();

            return designation;

        }

        public void Update(DesignationViewModel DesgVM)
        {
            designation = new Designation
            {
                DesignationID=DesgVM.DesignationID,
                DesignationName = DesgVM.DesignationName,
                DesignationNameBangla = DesgVM.DesignationNameBangla,
                DepartmentID =DesgVM.DepartmentID,
                IsOverTimeApplicable = DesgVM.IsOverTimeApplicable,
                ModifiedBy = DesgVM.ModifiedBy,
                LastModified = DateTime.Now,
                IsDeleted = false

            };

            unitOfWork.DesignationRepository.Update(designation);
            unitOfWork.Save();

        }

        public void Delete(DesignationViewModel DesgVM)
        {
            designation = (from desg in unitOfWork.DesignationRepository.Get()
                           where desg.DesignationID == DesgVM.DesignationID
                           select desg).SingleOrDefault();

            unitOfWork.DesignationRepository.Delete(designation);
            unitOfWork.Save();
        }

        public List<DesignationViewModel> GetAll()
        {
            return (
                from desg in unitOfWork.DesignationRepository.Get()
                join dpt in unitOfWork.DepartmentRepository.Get()
                on desg.DepartmentID equals dpt.DepartmentID into departmentGroup
                from d in departmentGroup.DefaultIfEmpty()
                select new DesignationViewModel
                {
                    DesignationID = desg.DesignationID,
                    DesignationName = desg.DesignationName,
                    DesignationNameBangla=desg.DesignationNameBangla,
                    DepartmentID=desg.DepartmentID,
                    DepartmentName = d.DepartmentName,
                    IsOverTimeApplicable=desg.IsOverTimeApplicable
                }
                ).ToList();
        }

        public DesignationViewModel GetByID(int id)
        {

            return (
                from desg in unitOfWork.DesignationRepository.Get()
                where desg.DesignationID == id
                select new DesignationViewModel
                {
                    DesignationID = desg.DesignationID,
                    DesignationName = desg.DesignationName,
                    DesignationNameBangla=desg.DesignationNameBangla,
                    DepartmentID = desg.DepartmentID

                }
                ).SingleOrDefault();
        }

        public bool IsExists(DesignationViewModel designationVM)
        {
            IQueryable<int> result;

            if (designationVM.DesignationID == 0)
            {
                result = from des in unitOfWork.DesignationRepository.Get()
                         where des.DesignationName.ToLower().Trim() == designationVM.DesignationName.ToLower().Trim() &&
                         des.DepartmentID == designationVM.DepartmentID
                         select des.DesignationID;
            }
            else
            {
                result = from des in unitOfWork.DesignationRepository.Get()
                         where des.DesignationName.ToLower().Trim() == designationVM.DesignationName.ToLower().Trim() &&
                         des.DepartmentID == designationVM.DepartmentID && des.DesignationID != designationVM.DesignationID
                         select des.DesignationID;
            }

            if (result.Count() > 0)
            {
                return true;
            }
            return false;
        }

        public object GetDesignationByEmployeeID(int employeeID)
        {
            var res = (from e in unitOfWork.EmployeeRepository.Get()
                       join d in unitOfWork.DepartmentRepository.Get() on e.DepartmentID equals d.DepartmentID
                       join desg in unitOfWork.DesignationRepository.Get() on d.DepartmentID equals desg.DepartmentID
                       where e.EmployeeID==employeeID
                       select new DesignationViewModel
                       {
                           DesignationID=desg.DesignationID,
                           DesignationName=desg.DesignationName,
                           DesignationNameBangla=desg.DesignationNameBangla
                       }).ToList();
            return res;
        }

        public int IfExitsByName(string designationName)
        {
            int designationId= (from des in unitOfWork.DesignationRepository.Get()
                                     where des.DesignationName == designationName
                                     select des).Count()>0? Convert.ToInt32((from des in unitOfWork.DesignationRepository.Get()
                                                                             where des.DesignationName == designationName
                                                                             select des.DesignationID).FirstOrDefault()) : 0;


            
            return designationId;
        }

        public object GetAllDesignationByDepartment(int departmentID)
        {
            return (
                from desg in unitOfWork.DesignationRepository.Get()
                join dpt in unitOfWork.DepartmentRepository.Get()
                on desg.DepartmentID equals dpt.DepartmentID into departmentGroup
                from d in departmentGroup.DefaultIfEmpty()
                where desg.DepartmentID == departmentID

                select new DesignationViewModel
                {
                    DesignationID = desg.DesignationID,
                    DesignationName = desg.DesignationName,
                    DesignationNameBangla=desg.DesignationNameBangla,
                    DepartmentID = desg.DepartmentID,
                    DepartmentName = d.DepartmentName,
                    IsOverTimeApplicable = desg.IsOverTimeApplicable
                }
                ).ToList();
        }
    }
}
