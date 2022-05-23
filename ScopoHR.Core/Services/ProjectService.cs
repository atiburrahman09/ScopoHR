using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScopoHR.Core.ViewModels;
using ScopoHR.Domain.Repositories;
using ScopoHR.Domain.Models;

namespace ScopoHR.Core.Services
{
    public class ProjectService
    {
        private UnitOfWork unitOfWork;
        private Project project;

        public ProjectService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public List<ProjectViewModel> GetAllProjectList()
        {
            return (from p in unitOfWork.ProjectRepository.Get()
                    select new ProjectViewModel
                    {
                        ProjectID=p.ProjectID,
                        ProjectName=p.ProjectName
                    }).ToList();
        }

        public void Update(ProjectViewModel projectVM)
        {
            project = new Project
            {
                ProjectID=projectVM.ProjectID,
                ProjectName = projectVM.ProjectName,
                ProjectDesc = projectVM.ProjectDesc,
                Budget = projectVM.Budget,
                Status = projectVM.Status,
                DelivaryDate = projectVM.DelivaryDate,
                ClientID = projectVM.ClientID
            };

            unitOfWork.ProjectRepository.Update(project);
            unitOfWork.Save();
        }

        public void Create(ProjectViewModel projectVM)
        {
            project = new Project
            {
                ProjectName = projectVM.ProjectName,
                ProjectDesc = projectVM.ProjectDesc,
                Budget = projectVM.Budget,
                Status = projectVM.Status,
                DelivaryDate = projectVM.DelivaryDate,
                ClientID = projectVM.ClientID
            };

            unitOfWork.ProjectRepository.Insert(project);
            unitOfWork.Save();
        }

        public ProjectViewModel GetProjectDetailByID(int projectID)
        {
            return (from p in unitOfWork.ProjectRepository.Get()
                    where p.ProjectID == projectID
                    select new ProjectViewModel
                    {
                        ProjectID = p.ProjectID,
                        ProjectName = p.ProjectName,
                        ProjectDesc=p.ProjectDesc,
                        Budget=p.Budget,
                        Status=p.Status,
                        DelivaryDate=p.DelivaryDate,
                        ClientID=p.ClientID

                    }).SingleOrDefault();
        }

        public bool IsUnique(ProjectViewModel projectVM)
        {
            IQueryable<int> result;

            if (projectVM.ProjectID == 0)
            {
                result = (from p in unitOfWork.ProjectRepository.Get()
                          where p.ProjectName.ToLower() == projectVM.ProjectName.ToLower()
                          select p.ProjectID);
            }
            else
            {
                result = (from p in unitOfWork.ProjectRepository.Get()
                          where p.ProjectName.ToLower().Trim() == projectVM.ProjectName.ToLower().Trim() && p.ProjectID != projectVM.ProjectID
                          select p.ProjectID);
            }

            if (result.Count() > 0)
            {
                return true;
            }
            return false;
            
        }
    }
}
