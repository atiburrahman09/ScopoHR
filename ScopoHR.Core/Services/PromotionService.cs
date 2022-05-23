using ScopoHR.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScopoHR.Core.ViewModels;
using ScopoHR.Domain.Models;

namespace ScopoHR.Core.Services
{
    
    public class PromotionService
    {
        private UnitOfWork unitOfWork;
        private Promotion promotion;
        public PromotionService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public List<PromotionViewModel> GetEmployeeePromotionDetailsById(int employeeID)
        {
            List<PromotionViewModel> res = (from p in unitOfWork.PromotionRepository.Get()
                                                  join e in unitOfWork.EmployeeRepository.Get() on p.EmployeeID equals e.EmployeeID
                                                  join d in unitOfWork.DesignationRepository.Get() on p.NewDesignationID equals d.DesignationID
                                                  join dp in unitOfWork.DepartmentRepository.Get() on d.DepartmentID equals dp.DepartmentID
                                                  where p.EmployeeID == employeeID
                                                  select new PromotionViewModel
                                                  {
                                                     CardNo=p.CardNo,
                                                     EmployeeID=p.EmployeeID,
                                                     PromotionID=p.PromotionID,
                                                     PromotionDate=p.PromotionDate,
                                                     Remarks=p.Remarks,
                                                     EmployeeName=e.EmployeeName,
                                                     NewDesignation=d.DesignationName,
                                                     DepartmentID=d.DepartmentID
                                                  }).ToList();
            return res;
        }

        public void SaveEmployeePromotion(PromotionViewModel promotionVM, string name)
        {
            var employeeInfo = (from e in unitOfWork.EmployeeRepository.Get()
                                where e.EmployeeID == promotionVM.EmployeeID
                                select e).SingleOrDefault();


            if (promotionVM.PromotionID > 0)
            {
                promotion = (from p in unitOfWork.PromotionRepository.Get()
                                   where p.PromotionID == promotionVM.PromotionID && p.EmployeeID == promotionVM.EmployeeID
                                   select p).SingleOrDefault();

                promotion.PromotionDate = promotionVM.PromotionDate;
                promotion.Remarks = promotionVM.Remarks;
                promotion.LastModified = DateTime.Now;
                promotion.ModifiedBy = name;
                promotion.PreviousDesignationID = employeeInfo.DesignationID;
                promotion.NewDesignationID = promotionVM.NewDesignationID;

                unitOfWork.PromotionRepository.Update(promotion);

            }
            else
            {
                promotion = new Promotion
                {
                  CardNo= employeeInfo.CardNo,
                  EmployeeID=promotionVM.EmployeeID,
                  PromotionDate=promotionVM.PromotionDate,
                  Remarks=promotionVM.Remarks,
                  IsDeleted=false,
                  LastModified=DateTime.Now,
                  ModifiedBy=name,
                  PreviousDesignationID=employeeInfo.DesignationID,
                  NewDesignationID=promotionVM.NewDesignationID
                };
                unitOfWork.PromotionRepository.Insert(promotion);
            }

            employeeInfo.DepartmentID = promotionVM.DepartmentID;
            employeeInfo.DesignationID = promotionVM.NewDesignationID;
            employeeInfo.LastModified = DateTime.Now;
            employeeInfo.ModifiedBy = name;

            unitOfWork.EmployeeRepository.Update(employeeInfo);

            unitOfWork.Save();
        }
    }
}
