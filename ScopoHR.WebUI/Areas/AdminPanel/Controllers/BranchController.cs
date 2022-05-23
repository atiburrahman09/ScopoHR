using ScopoHR.Core.Common;
using ScopoHR.Core.Services;
using ScopoHR.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ScopoHR.WebUI.Areas.AdminPanel.Controllers
{
    [Authorize(Roles = AppRoles.SuperUser + ", " + AppRoles.Admin + "," + AppRoles.ManageBranch)]
    public class BranchController : Controller
    {
        private BranchService _branchService;
        public BranchController(BranchService branchService)
        {
            this._branchService = branchService;
        }

        // GET: AdminPanel/Branch
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public JsonResult SaveBranch(BranchViewModel branchVM)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid Data Submitted!");
            }


            try
            {
                _branchService.SaveBranch(branchVM);
                return Json("Successfully Saved!");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }
        
        public JsonResult GetBranchDropDown()
        {
            try
            {
                return Json(_branchService.GetBranchDropDown(), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetBranchByID(int id)
        {
            if (id <= 0)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid Branch Selected!");
            }

            try
            {
                return Json(_branchService.GetBranchByID(id), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}