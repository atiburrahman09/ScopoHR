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
    public class Sys_GraceService
    {
        private Sys_Grace grace;
        private UnitOfWork unitOfWork;

        public Sys_GraceService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void CreateGrace(Sys_GraceViewModel model,string name)
        {
            grace = new Sys_Grace
            {
                BeforeInTimeGrace = model.BeforeInTimeGrace,
                AfterInTimeGrace = model.AfterInTimeGrace,
                BeforeOutTimeGrace = model.BeforeOutTimeGrace,
                AfterOutTimeGrace = model.AfterOutTimeGrace,
                IsApplicable = model.IsApplicable,
                IsDeleted = false,
                LastModified = DateTime.Now,
                ModifiedBy = name
            };
            unitOfWork.Sys_GraceRepository.Insert(grace);
            unitOfWork.Save();
        }

        public void UpdateGrace(Sys_GraceViewModel model, string name)
        {
            grace = new Sys_Grace
            {
                GraceID=model.GraceID,
                BeforeInTimeGrace = model.BeforeInTimeGrace,
                AfterInTimeGrace = model.AfterInTimeGrace,
                BeforeOutTimeGrace = model.BeforeOutTimeGrace,
                AfterOutTimeGrace = model.AfterOutTimeGrace,
                IsApplicable = model.IsApplicable,
                IsDeleted = false,
                LastModified = DateTime.Now,
                ModifiedBy = name
            };
            unitOfWork.Sys_GraceRepository.Update(grace);
            unitOfWork.Save();
        }

        public object GetGraceData()
        {
            var res = (from g in unitOfWork.Sys_GraceRepository.Get()
                       select g).SingleOrDefault();

            return res;
        }
    }
}
