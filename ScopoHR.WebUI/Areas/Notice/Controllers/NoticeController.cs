using ScopoHR.Core.Services;
using ScopoHR.Core.ViewModels;
using ScopoHR.Domain.Repositories;
using ScopoHR.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using ScopoHR.Core.Helpers;
using ScopoHR.Core.Common;

namespace ScopoHR.WebUI.Areas.Notice.Controllers
{
    //[Authorize(Roles = AppRoles.SuperUser + ", " + AppRoles.Admin + "," + AppRoles.Notice)]
    public class NoticeController : Controller
    {
        private NoticeService noticeService;
        private EmployeeService employeeService;
        private OfficeTimingService officeTimingService;
        private OverTimeReportService overTimeReportService;
        public NoticeController(NoticeService _noticeService, EmployeeService _employeeService, OfficeTimingService officeTimingService, OverTimeReportService overTimeReportService)
        {
            this.employeeService = _employeeService;
            this.noticeService = _noticeService;
            this.officeTimingService = officeTimingService;
            this.overTimeReportService = overTimeReportService;
        }

        // GET: Notice/Notice
        public ActionResult Index()
        {
            
            return View();
        }

        public JsonResult GetAllNotice()
        {
            try
            {
                var data = noticeService.GetAll();
               
                return Json(new { data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }
        [HttpGet]
        public JsonResult GetNoticeByID(int NoticeID)
        {
            try
            {
                var data = noticeService.GetByID(NoticeID);
                return Json(new { data = data }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }

        }
        [HttpPost]
        public JsonResult CreateUpdateNotice(NoticeViewModel notice)
        {

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Model State is not valid!");
            }

            if (notice.NoticeID > 0)
            {
                try
                {
                    notice.ModifiedBy = User.Identity.Name;
                    noticeService.Update(notice);
                    return Json("Notice Updated Successfully!!!");
                }
                catch (Exception ex)
                {
                    Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                    return Json(ex.Message);
                }
            }
            if(notice.NoticeID == 0 && notice.NoticeTitle!=null)
            {
                try
                {
                    notice.ModifiedBy = User.Identity.Name;
                    noticeService.Create(notice);
                    return Json("Notice Save Successfully!!!");
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
        
        [HttpPost]
        public JsonResult DeleteNoticeByID(int NoticeID)
        {
            try
            {
                noticeService.Delete(NoticeID);
                return Json("Notice Deleted");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }

        }


        public ActionResult NoticeAssign()
        {
            return View();
        }

        public JsonResult GetAllNoticeList()
        {
            try
            {
                List<NoticeViewModel> noticesList = noticeService.GetDropDownList();
                return Json(new { data = noticesList }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        public JsonResult getAllRecentUserList()
        {
            try
            {
                var id = UserHelper.Instance.Get().BranchId;

                //List<EmployeeViewModel> userList = ;
                return Json(employeeService.GetRecentEmployees(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        public JsonResult GetAllNoticeDetailByID(int nID)
        {
            try
            {
                NoticeViewModel noticesList = noticeService.GetByID(nID);
                return Json(new { data = noticesList }, JsonRequestBehavior.AllowGet);
            }

             catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        //public JsonResult PublishNotice(PublishedNoticeViewModel publish)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        Response.StatusCode = (int)HttpStatusCode.BadRequest;
        //        return Json("Model State is not valid!");
        //    }
        //    try
        //    {
        //        noticeService.NoticeAssign(publish);
        //        return Json("Notice Has been Published Successfully!!!");
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
        //        return Json(ex.Message);
        //    }
        //}


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


        //public ActionResult PublishNotice()
        //{
        //    return View();
        //}


        [HttpPost]
        public ActionResult SendNotice(PublishedNoticeViewModel data)
        {
            try
            {
                var res = noticeService.SendNotice(data, User.Identity.Name);
                return Json(res);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }
        
        public ActionResult DropOutListReport()
        {
            return View();
        }


        public JsonResult GetWorkingShift()
        {
            try
            {
                List<DropDownViewModel> workingShiftList = officeTimingService.GetWorkingShiftsDropDown();
                return Json(workingShiftList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetAllProductionFloorLine()
        {
            try
            {
                List<ProductionFloorLineViewModel> productionFList = overTimeReportService.GetProductionFloorList();
                return Json(productionFList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetDropOutListReport(ReportFilteringViewModel dReport)
        {
            try
            {
                List<DropOutReportViewModel> dropOutReportList = noticeService.GetDropOutReport(dReport, UserHelper.Instance.Get().BranchId);
                return Json(dropOutReportList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        public ActionResult PrintNotice()
        {
            return View();
        }






    }
}