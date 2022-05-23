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

namespace ScopoHR.WebUI.Areas.EmployeeManagement.Controllers
{
    //[Authorize(Roles = AppRoles.SuperUser + ", " + AppRoles.EmployeeAttendance + " ," + AppRoles.Admin)]
    public class AttendanceController : Controller
    {
        private AttendanceService attendanceService;
        private DepartmentService departmentService;
        private EmployeeService employeeService;
        private ProductionService productionService;
        
        public AttendanceController(AttendanceService _attendanceService, DepartmentService _departmentService, EmployeeService _employeeService, ProductionService _productionService)
        {
            this.attendanceService = _attendanceService;
            this.departmentService = _departmentService;
            this.employeeService = _employeeService;
            this.productionService = _productionService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Ntitas()
        {
            return View();
        }

        public ActionResult Attendance()
        {
            return View();
        }

        public JsonResult GetAllDepartments()
        {
            try
            {
                List<DepartmentViewModel> deparmentList = departmentService.GetAll();
                return Json(new { data = deparmentList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        
        public JsonResult GetAllAttendanceList()
        {
            try
            {
                List<AttendanceViewModel> attendanceList = attendanceService.GetAllAttendances();
                return Json(new { data = attendanceList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAttendanceByID(int AttendanceID)
        {
            try
            {
                AttendanceViewModel attendanceList = attendanceService.GetByID(AttendanceID);
                return Json(new { data = attendanceList }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        
        
        public JsonResult SaveAttendance(AttendanceViewModel attendance)
        {
            try
            {
                attendance.Remarks = "Missing Data Update";
                attendanceService.Save(attendance,User.Identity.Name);
                return Json("Time added!", JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        //[HttpPost]
        //public JsonResult UpdateAttendance(AttendanceViewModel attendance)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        Response.StatusCode = (int)HttpStatusCode.BadRequest;
        //        return Json("Model State is not valid!");
        //    }
        //    try
        //    {
        //        attendanceService.Update(attendance, User.Identity.Name);
        //        return Json("Updated Successfully!");
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
        //        return Json(ex.Message);
        //    }
        //}
        
        public JsonResult GetAllProductionFloor()
        {
            try
            {
                List<ProductionFloorLineViewModel> productionFList = productionService.GetProductionFloorList();
                return Json(productionFList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpPost]
        public JsonResult SearchAttendance(SearchAttendanceViewModel searchVM)
        {
            if(!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid data submitted");
            }

            try
            {
                var id = UserHelper.Instance.Get().BranchId;
                //var list = attendanceService.GetDailyAttendance(searchVM, id, Page.Size, Page.Size * pageNo, out count);
                var res= attendanceService.GetDailyAttendance(searchVM, id);
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDailyAttendance(DateTime date, string floor, bool absentOnly, int pageNo)
        {
            try
            {
                var id = UserHelper.Instance.Get().BranchId;
                List<AttendanceViewModel> list = new List<AttendanceViewModel>();               
                var skip = Page.Size * pageNo;
                list = attendanceService.GetDailyAttendance(date, floor, id, absentOnly, Page.Size, skip);
                return Json(new { data = list }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
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

        [HttpPost]
        public JsonResult UpdateDailyAttendance(List<AttendanceViewModel> attList)
        {
            //if (!ModelState.IsValid)
            //{
            //    Response.StatusCode = (int)HttpStatusCode.BadRequest;
            //    return Json("Invalid data submitted!");
            //}

            try
            {
                attList.ForEach(x => x.Remarks = "Missing Data Update");
                attendanceService.UpdateDailyAttendance(attList, User.Identity.Name);
                return Json("Update successful!");
            }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        public JsonResult GetEmployeeInfo(string cardNo)
        {
            try
            {
                EmployeeViewModel info = employeeService.GetEmployeeInfoByCardNo(cardNo);
                return Json(info, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteAttendance(AttendanceViewModel attendance)
        {
            try
            {
                attendanceService.DeleteAttendance(attendance,User.Identity.Name);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AttendanceLocationWise()
        {
            return View();
        }
        [HttpPost]
        public JsonResult SaveInformation(WorkerBusViewModel wbViewModel)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid data submitted!");
            }

            try
            {
                wbViewModel.LastModified = DateTime.Now;
                wbViewModel.ModifiedBy = User.Identity.Name;
                attendanceService.SaveInformation(wbViewModel);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }
        public JsonResult GetBusLocation()
        {
            try
            {
                List<DropDownViewModel> locationList = attendanceService.GetAllBusLocation();
                return Json(new { data = locationList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetRecentEmployees()
        {
            try
            {
                List<EmployeeDropDown> empList = employeeService.GetRecentEmployees(UserHelper.Instance.Get().BranchId);
                return Json(new { data = empList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetAllEmployees()
        {
            try
            {
                List<DropDownViewModel> empList = employeeService.GetAllEmployees();
                return Json(new { data = empList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetEmpList(int locationID)
        {
            try
            {
                List<DropDownViewModel> empList = employeeService.GetEmpByLocation(locationID);
                return Json(empList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SaveList(WorkerBusViewModel list)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid data submitted!");
            }

            try
            {
                attendanceService.SaveList(list);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }




    }
}