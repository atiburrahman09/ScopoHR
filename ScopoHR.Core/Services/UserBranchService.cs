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
    public class UserBranchService
    {
        private UnitOfWork _unitOfWork;
        private UserBranch _userBranch;
        
        public UserBranchService(UnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        
        public List<int> GetUserBranches(string userName)
        {
            var result = (from ub in _unitOfWork.UserBranchRepository.Get()
                          join br in _unitOfWork.BranchRepository.Get()
                          on ub.BranchID equals br.BranchID into br_group
                          from b in br_group.DefaultIfEmpty()
                          where ub.UserID == userName
                          select b.BranchID).ToList();
            return result;

        }

        public void AddToBranch(string userID, List<int> branchIDs)
        {

            foreach(var b in branchIDs)
            {
                _userBranch = new UserBranch()
                {
                    UserID = userID,
                    BranchID = b
                };
                _unitOfWork.UserBranchRepository.Insert(_userBranch);                
            }
            _unitOfWork.Save();
        }

        public bool IsInBranch(string userID, int branchID)
        {
            var result = (from b in _unitOfWork.UserBranchRepository.Get()
                          where b.BranchID == branchID && b.UserID == userID
                          select b).SingleOrDefault();
            return result != null;
        }
    }
}
