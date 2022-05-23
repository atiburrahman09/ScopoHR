using ScopoHR.Core.Common;
using ScopoHR.Core.Helpers;
using ScopoHR.Core.Services;
using ScopoHR.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ScopoHR.WebUI.Areas.Reports.Controllers
{
    [Authorize(Roles =AppRoles.SuperUser + ", " + AppRoles.Director + "," + AppRoles.Admin + "," + AppRoles.Reports)]
    public class DailyAttendanceController : Controller
    {
        private DailyReportService dailyReportService;
        private OfficeTimingService officeTimingService;

        public DailyAttendanceController(DailyReportService _dailyReportService, OfficeTimingService _officeTimingService)
        {
            this.dailyReportService = _dailyReportService;
            this.officeTimingService = _officeTimingService;
        }

        // GET: Reports/DailyAttendance
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllProductionFloor()
        {
            try
            {
                List<ProductionFloorLineViewModel> productionFList = dailyReportService.GetProductionFloorList();
                return Json(productionFList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
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

        [HttpPost]
        public JsonResult DailyReportByDateFloor(DailyReportFilteringViewModel dReport)
        {
            try
            {
                var id = UserHelper.Instance.Get().BranchId;
                List<DailyReportViewModel> dailyReportList = dailyReportService.GetDailyReportByDateFloor(dReport, id);
                var jsonResult = Json(dailyReportList, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }
    }
}