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
    public class MedicineController : Controller
    {
        private MedicineService medService;
        public MedicineController(MedicineService medService)
        {
            this.medService = medService;
        }
        // GET: Common/Medicine
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SaveMedicine(MedicineViewModel medVM)
        {

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid Data Submitted!");
            }
            try
            {
                if (medVM.MedicineID > 0)
                {
                    medService.UpdateMedicine(medVM, User.Identity.Name);
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return Json(new { Message = "Medicine updated." });
                }
                else
                {
                    medService.CreateMedicine(medVM, User.Identity.Name);
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return Json(new { Message = "Medicine created." });

                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }
        [HttpPost]
        public JsonResult IsUnique(MedicineViewModel medVM)
        {
            try
            {
                if (!medService.IsUnique(medVM))
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                Response.StatusCode = (int)HttpStatusCode.PartialContent;
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetAllMedicines()
        {
            try
            {
                return Json(medService.GetAllMedicines(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}