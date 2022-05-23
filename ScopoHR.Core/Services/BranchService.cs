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
    public class BranchService
    {
        private UnitOfWork _unitOfWork;
        public BranchService(UnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        

        public BranchViewModel GetBranchByID(int id)
        {
            var result = (from b in _unitOfWork.BranchRepository.Get()
                          where b.BranchID == id
                          select new BranchViewModel
                          {
                              BranchID = b.BranchID,
                              BranchAddress = b.BranchAddress,
                              BranchEmail = b.BranchEmail,
                              BranchFax = b.BranchFax,
                              BranchName = b.BranchName,
                              BranchPhone = b.BranchPhone,
                              IsCompany = b.IsCompany,
                              IsHeadOffice = b.IsHeadOffice
                          }).SingleOrDefault();
            return result;
        }

        public void SaveBranch(BranchViewModel branchVM)
        {
            if(branchVM.BranchID == 0)
            {
                createBranch(branchVM);
            }
            else
            {
                updateBranch(branchVM);
            }
        }

        public List<DropDownViewModel> GetBranchDropDown()
        {
            var result = (from b in _unitOfWork.BranchRepository.Get()
                          select new DropDownViewModel
                          {
                              Value = b.BranchID,
                              Text = b.BranchName
                          }).ToList();
            return result;
        }

        public List<BranchViewModel> GetAllBranch()
        {
            var result = (from b in _unitOfWork.BranchRepository.Get()
                          select new BranchViewModel
                          {
                              BranchID = b.BranchID,
                              BranchAddress = b.BranchAddress,
                              BranchEmail = b.BranchEmail,
                              BranchFax = b.BranchFax,
                              BranchName = b.BranchName,
                              BranchPhone = b.BranchPhone,
                              IsCompany = b.IsCompany,
                              IsHeadOffice = b.IsHeadOffice
                          }).ToList();
            return result;
        }
        
        // private methods
        private void createBranch(BranchViewModel branchVM)
        {
            _unitOfWork.BranchRepository.Insert(new Branch
            {                
                BranchAddress = branchVM.BranchAddress,
                BranchEmail = branchVM.BranchEmail,
                BranchFax = branchVM.BranchFax,
                BranchName = branchVM.BranchName,
                BranchPhone = branchVM.BranchPhone,
                IsCompany = branchVM.IsCompany,
                IsHeadOffice = branchVM.IsHeadOffice
            });

            _unitOfWork.Save();
        }
        private void updateBranch(BranchViewModel branchVM)
        {
            _unitOfWork.BranchRepository.Update(new Branch
            {
                BranchID = branchVM.BranchID,
                BranchAddress = branchVM.BranchAddress,
                BranchEmail = branchVM.BranchEmail,
                BranchFax = branchVM.BranchFax,
                BranchName = branchVM.BranchName,
                BranchPhone = branchVM.BranchPhone,
                IsCompany = branchVM.IsCompany,
                IsHeadOffice = branchVM.IsHeadOffice
            });
            _unitOfWork.Save();
        }
    }
}
