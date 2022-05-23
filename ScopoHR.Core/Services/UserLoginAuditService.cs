using NtitasCommon.Core.Interfaces;
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
    public class UserLoginAuditService : IUserLoginAuditService
    {
        private UnitOfWork _uow;

        public UserLoginAuditService(UnitOfWork uow)
        {
            _uow = uow;
        }
        
        public void clearAllLogin(string username, int branchId = 0)
        {
            List<UserLoginAudit> userLogins = null;
            if (branchId > 0)
            {
                userLogins = (from c in _uow.UserLoginAuditRepository.Get()
                              where c.BranchId == branchId
                              && c.LoggedOutTime == null
                              select c).ToList();
            }
            else 
            {
                userLogins = (from c in _uow.UserLoginAuditRepository.Get()
                              where c.LoggedOutTime == null
                              select c).ToList();
            }

            foreach (var userLogin in userLogins)
            {
                userLogin.LoggedOutTime = DateTime.Now;

                _uow.UserLoginAuditRepository.Update(userLogin);
            }

            _uow.Save();
        }

        public void insert(string username, int branchId)
        {
            clearAllLogin(username);
            UserLoginAudit model = new UserLoginAudit()
            {
                BranchId = branchId,
                UserName = username,
                LoggedInTime = DateTime.Now
            };

            _uow.UserLoginAuditRepository.Insert(model);
            _uow.Save();
        }

        public UserLoginAuditViewModel GetCurrentLoggedIn(string username)
        {
            UserLoginAuditViewModel userLogin = (from c in _uow.UserLoginAuditRepository.Get()
                                                 where c.LoggedOutTime == null
                                                 orderby c.Id
                                                 select new UserLoginAuditViewModel()
                                                 {
                                                     Id = c.Id,
                                                     BranchId = c.BranchId,
                                                     UserId = c.UserName,
                                                     LoggedInTime = c.LoggedInTime
                                                 }).FirstOrDefault();

            return userLogin;
        }

        public UserCacheViewModel GetUserCache(string username)
        {
            UserCacheViewModel userCache = (from c in _uow.UserLoginAuditRepository.Get()
                                            where c.LoggedOutTime == null
                                            orderby c.Id
                                            select new UserCacheViewModel()
                                            {
                                                UserId = c.UserName,
                                                BranchId = c.BranchId,
                                                BranchName = c.Branch.BranchName,
                                                LoggedInTime = c.LoggedInTime                                                
                                            }).FirstOrDefault();

            return userCache;
        }
    }
}
