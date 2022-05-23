using ScopoHR.Core.ViewModels;
using ScopoHR.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.Services
{
    public class ApplicationRoleService
    {

        private UnitOfWork _unitOfWork;
        public ApplicationRoleService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<AspNetUserRoleViewModel> GetAllRoles()
        {
            var result = (from r in _unitOfWork.AspNetRolesRepository.Get()
                          where r.Name.ToLower() != "superuser"
                          select new AspNetUserRoleViewModel
                          {
                              Id = r.Id,
                              Name = r.Name
                          }).ToList();
            return result;
        }
    }
}
