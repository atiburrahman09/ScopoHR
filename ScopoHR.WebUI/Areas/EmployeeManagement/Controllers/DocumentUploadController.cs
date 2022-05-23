using ScopoHR.Core.Common;
using ScopoHR.Core.Helpers;
using ScopoHR.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ScopoHR.WebUI.Areas.EmployeeManagement.Controllers
{
    [Authorize(Roles = AppRoles.EmployeeSetup + ", " +AppRoles.Admin + ", " + AppRoles.SuperUser)]
    public class DocumentUploadController : Controller
    {
        private DocumentService documentService;
        private EmployeeService employeeService;

        public DocumentUploadController(
                    DocumentService documentService, 
                    EmployeeService employeeService)
        {
            this.documentService = documentService;
            this.employeeService = employeeService;
        }
        // GET: EmployeeManagement/DocumentUpload
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult GetDocumentTypes()
        {
            var data = documentService.GetDocumentTypes();
            return Json(data, JsonRequestBehavior.AllowGet);
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
                return Json(employeeService.GetEmployeeDropDownByKeyword(inputString, id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}