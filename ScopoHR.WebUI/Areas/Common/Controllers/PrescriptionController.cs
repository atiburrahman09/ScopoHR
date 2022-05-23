using ScopoHR.Core.Helpers;
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
    public class PrescriptionController : Controller
    {
        private PrescriptionService prescriptionService;
        private EmployeeService empService;
        public PrescriptionController(PrescriptionService prescriptionService, EmployeeService empService)
        {
            this.prescriptionService = prescriptionService;
            this.empService = empService;
        }
        // GET: Common/Prescription
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetEmployeeDropDownByKeyword(string inputString)
        {
            if (String.IsNullOrEmpty(inputString))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Please enter employee name or card no.", JsonRequestBehavior.AllowGet);
            }

            try
            {
                var id = UserHelper.Instance.Get().BranchId;
                return Json(empService.GetEmployeeDropDownByKeyword(inputString, id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CreateUpdateNotice(PrescriptionViewModel prescription)
        {

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Model State is not valid!");
            }

            if (prescription.PrescriptionID > 0)
            {
                try
                {
                    prescription.ModifiedBy = User.Identity.Name;                  
                    prescriptionService.Update(prescription);
                    return Json("Prescription Updated Successfully!!!");
                }
                catch (Exception ex)
                {
                    Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                    return Json(ex.Message);
                }
            }
            if (prescription.PrescriptionID == 0)
            {
                try
                {
                    prescription.ModifiedBy = User.Identity.Name;
                    prescriptionService.Create(prescription);
                    return Json("Prescription Save Successfully!!!");
                }
                catch (Exception ex)
                {
                    Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                    return Json(ex.Message);
                }
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json("Data has been Violated!! Please Input Your Data Correctly!!!");

        }

        public JsonResult GetRecentPrescriptionList()
        {
            try
            {
                List<PrescriptionViewModel> list = prescriptionService.GetRecentPrescriptionList();
                return Json(new { data = list }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        public JsonResult GetPrescriptionDropDownByKeyword(string inputString)
        {
            if (String.IsNullOrEmpty(inputString))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Please enter employee name or card no.", JsonRequestBehavior.AllowGet);
            }

            try
            {
                var id = UserHelper.Instance.Get().BranchId;
                return Json(prescriptionService.GetPrescriptionDropDownByKeyword(inputString, id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

    }
}