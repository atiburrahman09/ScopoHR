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
    public class JobApplicationController : Controller
    {

        private JobCircularService jobCircularService;
        private JobApplicationService jobApplicationService;


        public JobApplicationController(JobCircularService _jobCircularService,JobApplicationService _jobApplicationService)
        {
            this.jobCircularService = _jobCircularService;
            this.jobApplicationService = _jobApplicationService;
        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GoHome()
        {
            return View();
        }


        public JsonResult GetAllJobs()
        {
            var all = jobCircularService.GetAll();
            return Json(all, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Create(JobApplicationViewModel jobApplicationVM)
        {
            if (ModelState.IsValid)
            {
                jobApplicationVM.CreatedDate = DateTime.Now;


                jobApplicationService.Create(jobApplicationVM);
                return Json(new { });
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(false);
        }


        



    }
}