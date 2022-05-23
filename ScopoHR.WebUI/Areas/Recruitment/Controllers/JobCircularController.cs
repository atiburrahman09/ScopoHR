using ScopoHR.Core.Common;
using ScopoHR.Core.Services;
using ScopoHR.Core.ViewModels;
using ScopoHR.Domain.Models;
using ScopoHR.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ScopoHR.WebUI.Areas.Recruitment.Controllers
{
    [Authorize(Roles = AppRoles.SuperUser + ", " + AppRoles.Admin)]
    public class JobCircularController : Controller
    {
        private UnitOfWork unitOfwork;
        private JobCircularService jobCircularService;
        private JobApplicationService jobApplicationService;

        public JobCircularController()
        {
            unitOfwork = new UnitOfWork(new ScopoContext());
            jobCircularService = new JobCircularService(unitOfwork);
            jobApplicationService = new JobApplicationService(unitOfwork);
        }


        public ActionResult Index()
        {
            return View();
        }

        

        [HttpPost]
        public JsonResult CreateJob(JobCircularViewModel jobCircularVM)
        {
            if (ModelState.IsValid)
            {
                jobCircularVM.CreatedBy = User.Identity.Name;

                jobCircularService.Create(jobCircularVM);
                return Json(new { });
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(false);
        }

        
        public JsonResult Edit(JobCircularViewModel jobCirVM)
        {
            jobCircularService.Update(jobCirVM);
            return Json("true", JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(JobCircularViewModel jobCirVM)
        {
            jobCircularService.Delete(jobCirVM.JobCircularId);
            return Json("true", JsonRequestBehavior.AllowGet);
        }




        public JsonResult GetAll()
        {
            var all=jobCircularService.GetAll();
            return Json(all, JsonRequestBehavior.AllowGet);
        }




    }
}