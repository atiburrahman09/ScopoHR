using ScopoHR.Core.Common;
using ScopoHR.Core.Helpers;
using ScopoHR.Core.ViewModels;
using ScopoHR.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.Services
{
    public class ApplicationUserService
    {
        private UnitOfWork _unitOfWork;
        public ApplicationUserService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool IsSystemUser(string username)
        {
            return AppDefaults.SystemUsers.Contains(username);
        }

        public List<AppUserViewModel> GetAllApplicationUsers()
        {
            var result = (from u in _unitOfWork.ApplicationUserRepository.Get()     
                          join e in _unitOfWork.EmployeeRepository.Get()
                          on u.UserName equals e.CardNo
                          where !AppDefaults.SystemUsers.Contains(u.UserName)
                          select new AppUserViewModel
                          {
                              UserName = u.UserName,
                              EmployeeName = e.EmployeeName
                          }).ToList();
            return result;
            
        }

        public List<string> GetUserRoles(string userName)
        {
            var res = (from u in _unitOfWork.ApplicationUserRepository.Get()
                       join ur in _unitOfWork.AspNetUserRolesRepository.Get()
                       on u.Id equals ur.UserId

                       join r in _unitOfWork.AspNetRolesRepository.Get()
                       on ur.RoleId equals r.Id
                       
                       where u.UserName == userName
                       select r.Name).ToList();
            return res;            
        }
        
        public string GetAspNetUserID(string userName)
        {
            var result = (from u in _unitOfWork.ApplicationUserRepository.Get()
                          where u.UserName == userName
                          select u).SingleOrDefault();
            if (result != null)
                return result.Id;


            return null;
        }


        public void UnmapRoles(string aspNetUserID)
        {
            _unitOfWork.AspNetUserRolesRepository
                .RawQuery($"DELETE FROM AspNetUserRoles WHERE UserId = '{aspNetUserID}'");
        }

        public void UnmapBranches(string cardNo)
        {
            _unitOfWork.AspNetUserRolesRepository
                .RawQuery($"DELETE FROM UserBranches WHERE UserID = '{cardNo}'");
        }

        public string CheckUserExists(string userName)
        {
            var result = (from u in _unitOfWork.ApplicationUserRepository.Get()
                          where u.UserName == userName
                          select u).SingleOrDefault();
            if (result != null)
                return "True";


            return "False";
        }
    }
}
