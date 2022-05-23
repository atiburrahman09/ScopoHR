using ScopoHR.Core.Services;
using ScopoHR.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ScopoHR.WebUI.Areas.Common.Controllers
{
    public class LicenseController : Controller
    {
        private LicenseService licenseService;
        public LicenseController(LicenseService licenseService)
        {
            this.licenseService = licenseService;
        }
        // GET: Common/License
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SaveLicense(LicenseViewModel licenseVM)
        {

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid Data Submitted!");
            }
            try
            {
                if (licenseVM.LicenseID > 0)
                {

                    licenseService.UpdateLicense(licenseVM, User.Identity.Name);
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return Json(new { Message = "License updated." });
                }
                else
                {
                    licenseService.CreateLicense(licenseVM, User.Identity.Name);
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return Json(new { Message = "License created." });

                }



            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }
        public JsonResult GetAllLicenses()
        {
            try
            {
                return Json(licenseService.GetAllLicenses(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

    }
}