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
    public class SalaryMappingService
    {
        private UnitOfWork unitOfWork;
        private SalaryMapping salaryMapping;

        public SalaryMappingService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public List<SalaryTypeAmountViewModel> GetSalaryMapping(int employeeID)
        {

            var data = (from s in unitOfWork.SalaryTypeRepository.Get()                        
                        join e in unitOfWork.SalaryMappingRepository.Get().Where(em=> em.EmployeeID == employeeID)
                        on s.SalaryTypeID equals e.SalaryTypeID into group1
                        from g in group1.DefaultIfEmpty()
                        select new SalaryTypeAmountViewModel
                        {
                            SalaryTypeID = s.SalaryTypeID,
                            SalaryTypeName = s.SalaryTypeName,
                            Amount = g !=null ? g.Amount : 0
                        }).ToList();
            
            return data;
        }

        public List<SalaryTypeAmountViewModel> GetSalaryByCardNo(string cardNo)
        {
            var data = (from s in unitOfWork.SalaryMappingRepository.Get()
                        join e in unitOfWork.EmployeeRepository.Get() on s.EmployeeID equals e.EmployeeID
                        where e.CardNo == cardNo
                        select new SalaryTypeAmountViewModel
                        {
                            SalaryTypeID = s.SalaryTypeID,
                            Amount = s.Amount
                        }).ToList();

            return data;
        }

        public decimal? GetBasicSalaryByCardNo(string cardNo)
        {
            var data = (from s in unitOfWork.SalaryMappingRepository.Get()
                        join e in unitOfWork.EmployeeRepository.Get() on s.EmployeeID equals e.EmployeeID
                        where e.CardNo == cardNo && s.SalaryTypeID == 1
                        select s.Amount).FirstOrDefault();

            return data;
        }

        public decimal? GetHouseRentByCardNo(string cardNo)
        {
            var data = (from s in unitOfWork.SalaryMappingRepository.Get()
                        join e in unitOfWork.EmployeeRepository.Get() on s.EmployeeID equals e.EmployeeID
                        where e.CardNo == cardNo && s.SalaryTypeID == 6
                        select s.Amount).FirstOrDefault();

            return data;
        }

        public int SaveMapping(SalaryMappingViewModel salaryMappingVM, string name)
        {
            var existingMapping = unitOfWork.SalaryMappingRepository
                                    .Get()
                                    .Where(x => x.EmployeeID == salaryMappingVM.EmployeeID)
                                    .ToList();

            // Delete All existing mapping
            if(existingMapping != null || existingMapping.Count != 0)
            {
                unitOfWork.SalaryMappingRepository.DeleteRange(existingMapping);
            }

            // Insert mapping
            foreach(var item in salaryMappingVM.SalaryTypeAmountList)
            {
                salaryMapping = new SalaryMapping
                {
                    EmployeeID = salaryMappingVM.EmployeeID,
                    SalaryTypeID = item.SalaryTypeID,
                    Amount = item.Amount ?? 0,
                    LastModified= DateTime.Now,
                    ModifiedBy=name
                };
                unitOfWork.SalaryMappingRepository.Insert(salaryMapping);
            }
            unitOfWork.Save();

            return 0;
        }

        public List<SalaryTypeViewModel> GetAll()
        {
            var data = (from s in unitOfWork.SalaryTypeRepository.Get()
                        select new SalaryTypeViewModel
                        {
                            SalaryTypeID = s.SalaryTypeID,
                            SalaryTypeName = s.SalaryTypeName
                        }).ToList();
            return data;
        }

        public bool IsSalaryMappingExists(int employeeID)
        {
            var res = (from s in unitOfWork.SalaryMappingRepository.Get()
                       where s.EmployeeID == employeeID
                       select s).ToList();
            if(res.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
