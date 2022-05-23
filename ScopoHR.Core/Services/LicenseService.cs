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
    public class LicenseService
    {
        private UnitOfWork unitOfWork;
        private License license;

        public LicenseService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void CreateLicense(LicenseViewModel licenseVM, string name)
        {
            license = new License
            {
                LicenseNo = licenseVM.LicenseNo,
                LicenseName = licenseVM.LicenseName,
                LicenseValidity = licenseVM.LicenseValidity,
                RenewedDate = licenseVM.RenewedDate,
                Remarks = licenseVM.Remarks,
                ExpiryDate = licenseVM.ExpiryDate,
                LicenseFee=licenseVM.LicenseFee,
                Tips=licenseVM.Tips,
                BudgetAmount=licenseVM.BudgetAmount,
                BudgetDate=licenseVM.BudgetDate,
                IsDeleted = false,
                LastModified = DateTime.Now,
                ModifiedBy = name
            };

            unitOfWork.LicenseRepository.Insert(license);
            unitOfWork.Save();
        }

        public void UpdateLicense(LicenseViewModel licenseVM, string name)
        {
            license = new License
            {
                LicenseID = licenseVM.LicenseID,
                LicenseNo = licenseVM.LicenseNo,
                LicenseName = licenseVM.LicenseName,
                LicenseValidity = licenseVM.LicenseValidity,
                RenewedDate = licenseVM.RenewedDate,
                LicenseFee = licenseVM.LicenseFee,
                Tips = licenseVM.Tips,
                BudgetAmount = licenseVM.BudgetAmount,
                BudgetDate = licenseVM.BudgetDate,
                Remarks = licenseVM.Remarks,
                ExpiryDate=licenseVM.ExpiryDate,
                IsDeleted = false,
                LastModified = DateTime.Now,
                ModifiedBy = name
            };

            unitOfWork.LicenseRepository.Update(license);
            unitOfWork.Save();
        }

        public object GetAllLicenses()
        {
            var res = (from l in unitOfWork.LicenseRepository.Get()
                       where l.IsDeleted == false
                       select new LicenseViewModel
                       {
                           LicenseID = l.LicenseID,
                           LicenseNo = l.LicenseNo,
                           LicenseName = l.LicenseName,
                           RenewedDate = l.RenewedDate,
                           Remarks = l.Remarks,
                           LicenseValidity = l.LicenseValidity,
                           ExpiryDate = l.ExpiryDate,
                           LicenseFee = l.LicenseFee,
                           Tips = l.Tips,
                           BudgetAmount = l.BudgetAmount,
                           BudgetDate = l.BudgetDate,
                       }).ToList();

            return res;
        }
    }
}
