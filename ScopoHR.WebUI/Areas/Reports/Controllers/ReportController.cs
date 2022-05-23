using ScopeHR.WebUI.Reports;
using ScopoHR.Core.Common;
using ScopoHR.Core.Helpers;
using ScopoHR.Core.Services;
using ScopoHR.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ScopoHR.WebUI.Areas.Reports.Controllers
{
    [Authorize(Roles = AppRoles.SuperUser + ", " + AppRoles.Director + "," + AppRoles.Admin + "," + AppRoles.Reports)]
    public class ReportController : Controller
    {
        private OverTimeReportService overTimeReportService;
        private ReportingService reportingService;
        private EmployeeService employeeService;
        private ProductionService productionService;
        private OfficeTimingService officeTimingService;
        private NoticeService noticeService;
        private SalaryMappingService salaryMappingService;
        private DepartmentService departmentService;
        private LeaveApplicationService leaveLogic;
        private DesignationService designationService;
        private MaternityService maternityService;

        public ReportController(
                OverTimeReportService _overTimeReportService,
                ReportingService _reportingService,
                EmployeeService _employeeService,
                ProductionService _productionService,
                OfficeTimingService _officeTimingService,
                NoticeService noticeService,
                SalaryMappingService _salaryMappingService,
                DepartmentService departmentService,
                LeaveApplicationService leaveLogic,
                DesignationService designationService,
                MaternityService maternityService
            )
        {
            this.overTimeReportService = _overTimeReportService;
            this.reportingService = _reportingService;
            this.employeeService = _employeeService;
            this.productionService = _productionService;
            this.officeTimingService = _officeTimingService;
            this.noticeService = noticeService;
            this.salaryMappingService = _salaryMappingService;
            this.departmentService = departmentService;
            this.leaveLogic = leaveLogic;
            this.designationService = designationService;
            this.maternityService = maternityService;
        }

        // GET: Reports/Report
        public ActionResult Index()
        {
            return View();
        }

        public List<ProductionFloorLineViewModel> GetAllProductionFloor()
        {
            List<ProductionFloorLineViewModel> productionFList = overTimeReportService.GetProductionFloorList();
            return productionFList;
        }

        #region Over Time

        public ActionResult OverTimeReport()
        {
            SelectList shiftList = new SelectList(officeTimingService.GetWorkingShiftsDropDown(), "Value", "Text");
            ViewBag.shiftList = shiftList;
            List<ProductionFloorLineViewModel> floorList = GetAllProductionFloor();
            ViewBag.floorList = new SelectList(floorList, "Floor", "Floor");
            return View();
        }

        [HttpPost]
        public ActionResult GetOverTimeReport(ReportFilteringViewModel dReport)
        {
            try
            {
                var id = UserHelper.Instance.Get().BranchId;
                List<DailyReportViewModel> overTimeReportList = overTimeReportService.GetOverTimeReport(dReport, id);
                DataSet ds = new DataSet();
                ds.Tables.Add(overTimeReportList.ConvertToDataTable());
                ReportHelper.SetData("OverTimeReport", ds, dReport);
                //return Redirect("~/Reports/ReportViewer.aspx");
                return Redirect("~/Reports/ReportViewer.aspx");
                //return Json(overTimeReportList);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }
        #endregion

        #region Audit
        [HttpGet]
        public ActionResult AuditReports()
        {
            SelectList shiftList = new SelectList(officeTimingService.GetWorkingShiftsDropDown(), "Value", "Text");
            ViewBag.shiftList = shiftList;
            List<ProductionFloorLineViewModel> floorList = GetAllProductionFloor();
            ViewBag.floorList = new SelectList(floorList, "Floor", "Floor");
            return View();
        }

        [HttpPost]
        public ActionResult AuditReports(ReportFilteringViewModel reportFilterVM)
        {
            try
            {
                var id = UserHelper.Instance.Get().BranchId;
                switch (reportFilterVM.Decision)
                {
                    case 1:
                        var data = reportingService.GetAuditReportDetail(reportFilterVM, id);
                        DataSet ds = new DataSet();
                        ds.Tables.Add(data.ConvertToDataTable());
                        ReportHelper.SetData("AuditReportDetails", ds, reportFilterVM);
                        return Redirect("~/Reports/ReportViewer.aspx");
                    case 2:
                        data = reportingService.GetAuditReportTotalHoursAndNoOfWorkers(reportFilterVM, id);
                        ds = new DataSet();
                        ds.Tables.Add(data.ConvertToDataTable());
                        ReportHelper.SetData("AuditReportTotalHours", ds, reportFilterVM);
                        return Redirect("~/Reports/ReportViewer.aspx");
                    case 3:
                        data = reportingService.GetAuditReportTotalHoursAndNoOfWorkers(reportFilterVM, id);
                        ds = new DataSet();
                        ds.Tables.Add(data.ConvertToDataTable());
                        ReportHelper.SetData("AuditReportNoOfWorkers", ds, reportFilterVM);
                        return Redirect("~/Reports/ReportViewer.aspx");
                    case 4:
                        data = reportingService.GetProgresiveReport(reportFilterVM, id);
                        ds = new DataSet();
                        ds.Tables.Add(data.ConvertToDataTable());
                        ReportHelper.SetData("ProgresiveReport", ds, reportFilterVM);
                        return Redirect("~/Reports/ReportViewer.aspx");
                    case 5:
                        data = reportingService.GetAuditReportSummaryPerDay(reportFilterVM, id);
                        ds = new DataSet();
                        ds.Tables.Add(data.ConvertToDataTable());
                        ReportHelper.SetData("AuditReportSummaryPerDayReport", ds, reportFilterVM);
                        return Redirect("~/Reports/ReportViewer.aspx");
                    default:
                        return View();
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }
        #endregion

        #region Absent Report
        public ActionResult AbsentReport()
        {
            SelectList floorList = new SelectList(productionService.GetProductionFloorList(), "Floor", "Floor");
            ViewBag.floorList = floorList;
            SelectList shiftList = new SelectList(officeTimingService.GetWorkingShiftsDropDown(), "Value", "Text");
            ViewBag.shiftList = shiftList;
            return View();
        }

        [HttpPost]
        public ActionResult GetAbsentReport(ReportFilteringViewModel dReport)
        {
            try
            {
                List<AttendanceReportViewModel> absentReportList = reportingService.GetAbsentReport(dReport);
                DataSet ds = new DataSet();
                ds.Tables.Add(absentReportList.ConvertToDataTable());
                ReportHelper.SetData("AbsentReport", ds, dReport);
                //return Redirect("~/Reports/ReportViewer.aspx");
                return Redirect("~/Reports/ReportViewer.aspx");
                //return Json(overTimeReportList);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }
        #endregion

        #region One Time Punch
        public ActionResult OneTimeEntryReport()
        {
            SelectList floorList = new SelectList(productionService.GetProductionFloorList(), "Floor", "Floor");
            ViewBag.floorList = floorList;
            SelectList shiftList = new SelectList(officeTimingService.GetWorkingShiftsDropDown(), "Value", "Text");
            ViewBag.shiftList = shiftList;
            return View();
        }

        [HttpPost]
        public ActionResult GetOneTimeEntryReport(ReportFilteringViewModel dReport)
        {
            try
            {
                List<OneTimeEntryReportViewModel> oneTimeEntryReportList = reportingService.GetOneTimeEntryReport(dReport);
                DataSet ds = new DataSet();
                ds.Tables.Add(oneTimeEntryReportList.ConvertToDataTable());
                ReportHelper.SetData("OneTimeEntryReport", ds, dReport);
                //return Redirect("~/Reports/ReportViewer.aspx");
                return Redirect("~/Reports/ReportViewer.aspx");
                //return Json(overTimeReportList);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }
        #endregion

        #region Employee Attendance
        public ActionResult EmployeeAttendaceDetailsReport()
        {
            List<EmployeeViewModel> employeeList = employeeService.GetDropDownList(UserHelper.Instance.Get().BranchId);

            ViewBag.employeeList = new SelectList(employeeList, "EmployeeID", "EmployeeName");
            return View();
        }

        [HttpPost]
        public ActionResult GetEmployeeAttendaceDetailsReport(ReportFilteringViewModel dReport)
        {
            try
            {
                var id = UserHelper.Instance.Get().BranchId;
                List<EmployeeAttendaceDetailsReportViewmodel> data = reportingService.GetEmployeeAttendaceDetailsReport(dReport, id);

                //int totalPresentDay = data.Where(x => x.Status != "A").Count();
                //int? totalOTMin = data.Sum(x => x.OTMinutes);
                //int totalDayAtMonth = DateTime.DaysInMonth(dReport.FromDate.Value.Year, dReport.FromDate.Value.Month);
                //bool isAttendanceBonusApplicable = data.Where(x => x.Status == "A").Count() == 0 ? true : false;

                //var salary = salaryMappingService.GetSalaryByCardNo(dReport.CardNo);

                //decimal? basicSalary = salary.Where(x => x.SalaryTypeID == 1).FirstOrDefault().Amount;
                //decimal? medicalAllowance = salary.Where(x => x.SalaryTypeID == 4).FirstOrDefault().Amount;
                //decimal? foodAllowance = salary.Where(x => x.SalaryTypeID == 5).FirstOrDefault().Amount;
                //decimal? houseRent = salary.Where(x => x.SalaryTypeID == 6).FirstOrDefault().Amount;
                //decimal? conveyance = salary.Where(x => x.SalaryTypeID == 7).FirstOrDefault().Amount;

                //decimal? basicAndHouseRent = basicSalary + houseRent;
                //decimal? otRate = (basicAndHouseRent / Convert.ToDecimal(1.4)) / 104;
                //decimal? attendanceBonus = isAttendanceBonusApplicable == true ? 500 : 0;

                //decimal? payableWages = (basicAndHouseRent / totalDayAtMonth) * totalPresentDay + medicalAllowance + foodAllowance + conveyance + attendanceBonus;
                //decimal? otWages = (otRate / 60) * totalOTMin;

                dReport.BasicSalary = 0;
                dReport.OtRate = 0;
                dReport.TotalSalary = 0;

                DataSet ds = new DataSet();
                ds.Tables.Add(data.ConvertToDataTable());
                ReportHelper.SetData("EmployeeAttendaceDetailsReport", ds, dReport);

                return Redirect("~/Reports/ReportViewer.aspx");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }
        #endregion

        #region DropOut
        public ActionResult DropOutReport()
        {
            SelectList shiftList = new SelectList(officeTimingService.GetWorkingShiftsDropDown(), "Value", "Text");
            ViewBag.shiftList = shiftList;
            List<ProductionFloorLineViewModel> floorList = GetAllProductionFloor();
            ViewBag.floorList = new SelectList(floorList, "Floor", "Floor");

            List<DropDownViewModel> list = new List<DropDownViewModel>();
            list.Add(new DropDownViewModel() { Text = "3", Value = 3 });
            list.Add(new DropDownViewModel() { Text = "7", Value = 7 });
            list.Add(new DropDownViewModel() { Text = "10", Value = 10 });

            ViewBag.dayList = new SelectList(list, "Value", "Text");
            return View();
        }

        [HttpPost]
        public ActionResult GetDropOutReport(ReportFilteringViewModel dReport)
        {
            try
            {
                List<DropOutReportViewModel> dropOutReportList = reportingService.GetDropOutReport(dReport, UserHelper.Instance.Get().BranchId);
                DataSet ds = new DataSet();
                ds.Tables.Add(dropOutReportList.ConvertToDataTable());
                ReportHelper.SetData("DropOutReport", ds, dReport);
                //return Redirect("~/Reports/ReportViewer.aspx");
                return Redirect("~/Reports/ReportViewer.aspx");
                //return Json(overTimeReportList);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }
        #endregion

        #region All Employee Details Report

        public ActionResult AllEmployeeDetails()
        {
            SelectList shiftList = new SelectList(officeTimingService.GetWorkingShiftsDropDown(), "Value", "Text");
            ViewBag.shiftList = shiftList;

            SelectList DepartmentList = new SelectList(departmentService.GetDropDownList(), "DepartmentID", "DepartmentName");
            ViewBag.DepartmentList = DepartmentList;

            List<ProductionFloorLineViewModel> floorList = GetAllProductionFloor();
            ViewBag.floorList = new SelectList(floorList, "Floor", "Floor");

            List<DropDownViewModel> list = new List<DropDownViewModel>();
            list.Add(new DropDownViewModel() { Text = "Male", Value = 1 });
            list.Add(new DropDownViewModel() { Text = "Female", Value = 2 });
            ViewBag.GenderList = new SelectList(list, "Value", "Text");
            return View();
        }



        public ActionResult GetAllEmployeeDetailsReport(ReportFilteringViewModel reportFilteringVM)
        {
            try
            {
                List<EmployeeDetailsViewModel> dropOutReportList = reportingService.GetAllEmployeeDetailsReport(reportFilteringVM);
                DataSet ds = new DataSet();
                ds.Tables.Add(dropOutReportList.ConvertToDataTable());
                ReportHelper.SetData("EmployeeDetailsReport", ds, reportFilteringVM);
                //return Redirect("~/Reports/ReportViewer.aspx");
                return Redirect("~/Reports/ReportViewer.aspx");
                //return Json(overTimeReportList);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDesignationList(int departmentID)
        {
            var data = designationService.GetAllDesignationByDepartment(departmentID);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Employee Leave History Report

        public ActionResult EmployeeLeaveHistory()
        {
            return View();
        }

        public ActionResult GetEmployeeLeaveHistoryReport(ReportFilteringViewModel reportFilteringVM)
        {
            try
            {
                List<EmployeeLeaveHistoryViewModel> dropOutReportList = reportingService.GetEmployeeLeaveHistoryReport(reportFilteringVM);
                DataSet ds = new DataSet();
                ds.Tables.Add(dropOutReportList.ConvertToDataTable());
                ReportHelper.SetData("EmployeeLeaveHistoryReport", ds, reportFilteringVM);
                //return Redirect("~/Reports/ReportViewer.aspx");
                return Redirect("~/Reports/ReportViewer.aspx");
                //return Json(overTimeReportList);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Salary Sheet
        public ActionResult SalarySheetReport()
        {
            SelectList shiftList = new SelectList(officeTimingService.GetWorkingShiftsDropDown(), "Value", "Text");
            ViewBag.shiftList = shiftList;

            List<DropDownViewModel> list = new List<DropDownViewModel>();
            list.Add(new DropDownViewModel() { Text = "January", Value = 1 });
            list.Add(new DropDownViewModel() { Text = "February", Value = 2 });
            list.Add(new DropDownViewModel() { Text = "March", Value = 3 });
            list.Add(new DropDownViewModel() { Text = "April", Value = 4 });
            list.Add(new DropDownViewModel() { Text = "May", Value = 5 });
            list.Add(new DropDownViewModel() { Text = "June", Value = 6 });
            list.Add(new DropDownViewModel() { Text = "July", Value = 7 });
            list.Add(new DropDownViewModel() { Text = "August", Value = 8 });
            list.Add(new DropDownViewModel() { Text = "September", Value = 9 });
            list.Add(new DropDownViewModel() { Text = "October", Value = 10 });
            list.Add(new DropDownViewModel() { Text = "November", Value = 11 });
            list.Add(new DropDownViewModel() { Text = "December", Value = 12 });
            ViewBag.MonthList = new SelectList(list, "Value", "Text");

            List<DropDownViewModel> yearList = new List<DropDownViewModel>();
            yearList.Add(new DropDownViewModel() { Text = "2017", Value = 2017 });
            yearList.Add(new DropDownViewModel() { Text = "2018", Value = 2018 });
            yearList.Add(new DropDownViewModel() { Text = "2019", Value = 2019 });
            yearList.Add(new DropDownViewModel() { Text = "2020", Value = 2020 });
            yearList.Add(new DropDownViewModel() { Text = "2021", Value = 2021 });
            yearList.Add(new DropDownViewModel() { Text = "2022", Value = 2022 });
            yearList.Add(new DropDownViewModel() { Text = "2023", Value = 2023 });
            yearList.Add(new DropDownViewModel() { Text = "2024", Value = 2024 });
            yearList.Add(new DropDownViewModel() { Text = "2025", Value = 2025 });
            ViewBag.yearList = new SelectList(yearList, "Value", "Text");

            List<DepartmentViewModel> DepartmentList = departmentService.GetDropDownList();
            ViewBag.DepartmentList = new SelectList(DepartmentList, "DepartmentID", "DepartmentName");



            List<ProductionFloorLineViewModel> floorList = GetAllProductionFloor();
            ViewBag.floorList = new SelectList(floorList, "Floor", "Floor");

            List<DropDownViewModel> isActive = new List<DropDownViewModel>();
            isActive.Add(new DropDownViewModel() { Text = "Yes", Value = 1 });
            isActive.Add(new DropDownViewModel() { Text = "No", Value = 0 });

            ViewBag.IsActive = new SelectList(isActive, "Value", "Text");

            return View();
        }



        [HttpPost]
        public ActionResult GetSalarySheetReport(ReportFilteringViewModel dReport)
        {
            try
            {
                var id = UserHelper.Instance.Get().BranchId;
                List<SalarySheetViewModel> salarySheetReportList = reportingService.GetSalarySheetReport(dReport, id); //reportingService.GE(dReport, id);
                DataSet ds = new DataSet();
                ds.Tables.Add(salarySheetReportList.ConvertToDataTable());
                ReportHelper.SetData("SalarySheetReport2", ds, dReport);
                return Redirect("~/Reports/ReportViewer.aspx");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        #endregion


        #region Employee Minimum Working Hour

        public ActionResult EmployeeMinimumWorkingHour()
        {
            SelectList shiftList = new SelectList(officeTimingService.GetWorkingShiftsDropDown(), "Value", "Text");
            ViewBag.shiftList = shiftList;
            List<ProductionFloorLineViewModel> floorList = GetAllProductionFloor();
            ViewBag.floorList = new SelectList(floorList, "Floor", "Floor");
            return View();
        }

        public ActionResult EmployeeMinimumWorkingHourReport(ReportFilteringViewModel reportFilteringVM)
        {
            try
            {
                List<DailyReportViewModel> empMinWorkHourList = reportingService.GetEmployeeMinimumWorkingHour(reportFilteringVM);
                DataSet ds = new DataSet();
                ds.Tables.Add(empMinWorkHourList.ConvertToDataTable());
                ReportHelper.SetData("EmployeeMinimumWorkHourReport", ds, reportFilteringVM);
                return Redirect("~/Reports/ReportViewer.aspx");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Extra Delay Report

        public ActionResult ExtraDelay()
        {
            SelectList shiftList = new SelectList(officeTimingService.GetWorkingShiftsDropDown(), "Value", "Text");
            ViewBag.shiftList = shiftList;
            List<ProductionFloorLineViewModel> floorList = GetAllProductionFloor();
            ViewBag.floorList = new SelectList(floorList, "Floor", "Floor");
            return View();
        }

        public ActionResult GetExtraDelay(ReportFilteringViewModel reportFilteringVM)
        {
            try
            {
                List<DailyReportViewModel> extraDelayList = reportingService.GetExtraDelay(reportFilteringVM);
                DataSet ds = new DataSet();
                ds.Tables.Add(extraDelayList.ConvertToDataTable());
                ReportHelper.SetData("ExtraDelayReport", ds, reportFilteringVM);
                return Redirect("~/Reports/ReportViewer.aspx");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion


        #region Employee Bio Data Report

        public ActionResult EmployeeBioData()
        {
            return View();
        }

        public ActionResult GetEmployeeBioDataReport(ReportFilteringViewModel reportFilteringVM)
        {
            try
            {
                List<EmployeeBioDataViewModel> employeeBioInfo = reportingService.GetEmployeeBioDataReport(reportFilteringVM);
                DataSet ds = new DataSet();
                ds.Tables.Add(employeeBioInfo.ConvertToDataTable());

                if (reportFilteringVM.ReportType == 1)
                {
                    ReportHelper.SetData("PersonalInformationVerificationReport", ds, reportFilteringVM);
                    return Redirect("~/Reports/ReportViewer.aspx");
                }
                else if (reportFilteringVM.ReportType == 2)
                {
                    ReportHelper.SetData("JobCircularReport", ds, reportFilteringVM);
                    return Redirect("~/Reports/ReportViewer.aspx");
                }
                else if (reportFilteringVM.ReportType == 3)
                {
                    ReportHelper.SetData("ConfessionalStatementReport", ds, reportFilteringVM);
                    return Redirect("~/Reports/ReportViewer.aspx");
                }
                else if (reportFilteringVM.ReportType == 4)
                {
                    ReportHelper.SetData("OathOfHonorReport", ds, reportFilteringVM);
                    return Redirect("~/Reports/ReportViewer.aspx");
                }
                else if (reportFilteringVM.ReportType == 5)
                {
                    ReportHelper.SetData("MedicalRecord", ds, reportFilteringVM);
                    return Redirect("~/Reports/ReportViewer.aspx");
                }
                else
                {
                    ReportHelper.SetData("ApplicationForJobReport", ds, reportFilteringVM);
                    return Redirect("~/Reports/ReportViewer.aspx");
                }


            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion


        #region Employye Pay Slip
        public ActionResult EmployeesPaySlip()
        {
            SelectList shiftList = new SelectList(officeTimingService.GetWorkingShiftsDropDown(), "Value", "Text");
            ViewBag.shiftList = shiftList;

            List<DropDownViewModel> list = new List<DropDownViewModel>();
            list.Add(new DropDownViewModel() { Text = "January", Value = 1 });
            list.Add(new DropDownViewModel() { Text = "February", Value = 2 });
            list.Add(new DropDownViewModel() { Text = "March", Value = 3 });
            list.Add(new DropDownViewModel() { Text = "April", Value = 4 });
            list.Add(new DropDownViewModel() { Text = "May", Value = 5 });
            list.Add(new DropDownViewModel() { Text = "June", Value = 6 });
            list.Add(new DropDownViewModel() { Text = "July", Value = 7 });
            list.Add(new DropDownViewModel() { Text = "August", Value = 8 });
            list.Add(new DropDownViewModel() { Text = "September", Value = 9 });
            list.Add(new DropDownViewModel() { Text = "October", Value = 10 });
            list.Add(new DropDownViewModel() { Text = "November", Value = 11 });
            list.Add(new DropDownViewModel() { Text = "December", Value = 12 });
            ViewBag.MonthList = new SelectList(list, "Value", "Text");

            List<DropDownViewModel> yearList = new List<DropDownViewModel>();
            yearList.Add(new DropDownViewModel() { Text = "2017", Value = 2017 });
            yearList.Add(new DropDownViewModel() { Text = "2018", Value = 2018 });
            yearList.Add(new DropDownViewModel() { Text = "2019", Value = 2019 });
            yearList.Add(new DropDownViewModel() { Text = "2020", Value = 2020 });
            yearList.Add(new DropDownViewModel() { Text = "2021", Value = 2021 });
            yearList.Add(new DropDownViewModel() { Text = "2022", Value = 2022 });
            yearList.Add(new DropDownViewModel() { Text = "2023", Value = 2023 });
            yearList.Add(new DropDownViewModel() { Text = "2024", Value = 2024 });
            yearList.Add(new DropDownViewModel() { Text = "2025", Value = 2025 });
            ViewBag.yearList = new SelectList(yearList, "Value", "Text");

            List<DepartmentViewModel> DepartmentList = departmentService.GetDropDownList();
            ViewBag.DepartmentList = new SelectList(DepartmentList, "DepartmentID", "DepartmentName");



            List<ProductionFloorLineViewModel> floorList = GetAllProductionFloor();
            ViewBag.floorList = new SelectList(floorList, "Floor", "Floor");

            List<DropDownViewModel> employeeTypeList = new List<DropDownViewModel>();
            employeeTypeList.Add(new DropDownViewModel() { Text = "Worker", Value = 1 });
            employeeTypeList.Add(new DropDownViewModel() { Text = "Staff", Value = 2 });
            //list.Add(new DropDownViewModel() { Text = "10", Value = 10 });

            ViewBag.employeeTypeList = new SelectList(employeeTypeList, "Value", "Text");

            List<DropDownViewModel> bankSalaryList = new List<DropDownViewModel>();
            bankSalaryList.Add(new DropDownViewModel() { Text = "Yes", Value = 1 });
            bankSalaryList.Add(new DropDownViewModel() { Text = "No", Value = 0 });
            ViewBag.IsBankSalaryList = new SelectList(bankSalaryList, "Value", "Text");

            List<DropDownViewModel> isActive = new List<DropDownViewModel>();
            isActive.Add(new DropDownViewModel() { Text = "Yes", Value = 1 });
            isActive.Add(new DropDownViewModel() { Text = "No", Value = 0 });
            ViewBag.IsActive = new SelectList(isActive, "Value", "Text");

            return View();
        }



        [HttpPost]
        public ActionResult GetEmployeesPaySlipReport(ReportFilteringViewModel dReport)
        {
            try
            {
                var id = UserHelper.Instance.Get().BranchId;
                List<SalarySheetViewModel> salarySheetReportList = reportingService.GetSalaryPaySheetReport(dReport, id); //reportingService.GE(dReport, id);
                DataSet ds = new DataSet();
                ds.Tables.Add(salarySheetReportList.ConvertToDataTable());
                ReportHelper.SetData("EmployeesSalarySummaryReport", ds, dReport);
                return Redirect("~/Reports/ReportViewer.aspx");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        #endregion


        #region TiffinBill
        public ActionResult TiffinBill()
        {
            SelectList shiftList = new SelectList(officeTimingService.GetWorkingShiftsDropDown(), "Value", "Text");
            ViewBag.shiftList = shiftList;

            List<DepartmentViewModel> DepartmentList = departmentService.GetDropDownList();
            ViewBag.DepartmentList = new SelectList(DepartmentList, "DepartmentID", "DepartmentName");

            List<ProductionFloorLineViewModel> floorList = GetAllProductionFloor();
            ViewBag.floorList = new SelectList(floorList, "Floor", "Floor");
            return View();
        }



        [HttpPost]
        public ActionResult GetTiffinBillReport(ReportFilteringViewModel dReport)
        {
            try
            {
                var id = UserHelper.Instance.Get().BranchId;
                List<TiffinBillViewModel> tiffinBillReportList = reportingService.GetTiffinBillReport(dReport, id); //reportingService.GE(dReport, id);
                DataSet ds = new DataSet();
                ds.Tables.Add(tiffinBillReportList.ConvertToDataTable());
                ReportHelper.SetData("TiffinReport", ds, dReport);
                return Redirect("~/Reports/ReportViewer.aspx");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        #endregion

        #region New Recruitment
        public ActionResult NewRecruitment()
        {
            SelectList shiftList = new SelectList(officeTimingService.GetWorkingShiftsDropDown(), "Value", "Text");
            ViewBag.shiftList = shiftList;

            List<DepartmentViewModel> DepartmentList = departmentService.GetDropDownList();
            ViewBag.DepartmentList = new SelectList(DepartmentList, "DepartmentID", "DepartmentName");

            List<ProductionFloorLineViewModel> floorList = GetAllProductionFloor();
            ViewBag.floorList = new SelectList(floorList, "Floor", "Floor");
            return View();
        }



        [HttpPost]
        public ActionResult GetNewRecruitmentReport(ReportFilteringViewModel dReport)
        {
            try
            {
                var id = UserHelper.Instance.Get().BranchId;
                List<NewRecruitmentViewModel> newRecruitmentList = reportingService.GetNewRecruitmentReport(dReport, id); //reportingService.GE(dReport, id);
                DataSet ds = new DataSet();
                ds.Tables.Add(newRecruitmentList.ConvertToDataTable());
                ReportHelper.SetData("NewRecruitmentReport", ds, dReport);
                return Redirect("~/Reports/ReportViewer.aspx");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        #endregion

        #region Increment Report
        public ActionResult IncrementReport()
        {
            SelectList shiftList = new SelectList(officeTimingService.GetWorkingShiftsDropDown(), "Value", "Text");
            ViewBag.shiftList = shiftList;

            List<DepartmentViewModel> DepartmentList = departmentService.GetDropDownList();
            ViewBag.DepartmentList = new SelectList(DepartmentList, "DepartmentID", "DepartmentName");

            List<ProductionFloorLineViewModel> floorList = GetAllProductionFloor();
            ViewBag.floorList = new SelectList(floorList, "Floor", "Floor");

            List<DropDownViewModel> employeeTypeList = new List<DropDownViewModel>();
            employeeTypeList.Add(new DropDownViewModel() { Text = "Worker", Value = 1 });
            employeeTypeList.Add(new DropDownViewModel() { Text = "Staff", Value = 2 });
            ViewBag.employeeTypeList = new SelectList(employeeTypeList, "Value", "Text");

            return View();
        }



        [HttpPost]
        public ActionResult GetIncrementReport(ReportFilteringViewModel dReport)
        {
            try
            {
                var id = UserHelper.Instance.Get().BranchId;
                List<IncrementReportViewModel> incrementList = reportingService.GetIncrementReport(dReport, id); //reportingService.GE(dReport, id);
                DataSet ds = new DataSet();
                ds.Tables.Add(incrementList.ConvertToDataTable());
                ReportHelper.SetData("IncrementReport", ds, dReport);
                return Redirect("~/Reports/ReportViewer.aspx");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        #endregion

        #region Staff Salary Sheet
        public ActionResult StaffSalarySheetReport()
        {
            SelectList shiftList = new SelectList(officeTimingService.GetWorkingShiftsDropDown(), "Value", "Text");
            ViewBag.shiftList = shiftList;

            List<DropDownViewModel> list = new List<DropDownViewModel>();
            list.Add(new DropDownViewModel() { Text = "January", Value = 1 });
            list.Add(new DropDownViewModel() { Text = "February", Value = 2 });
            list.Add(new DropDownViewModel() { Text = "March", Value = 3 });
            list.Add(new DropDownViewModel() { Text = "April", Value = 4 });
            list.Add(new DropDownViewModel() { Text = "May", Value = 5 });
            list.Add(new DropDownViewModel() { Text = "June", Value = 6 });
            list.Add(new DropDownViewModel() { Text = "July", Value = 7 });
            list.Add(new DropDownViewModel() { Text = "August", Value = 8 });
            list.Add(new DropDownViewModel() { Text = "September", Value = 9 });
            list.Add(new DropDownViewModel() { Text = "October", Value = 10 });
            list.Add(new DropDownViewModel() { Text = "November", Value = 11 });
            list.Add(new DropDownViewModel() { Text = "December", Value = 12 });
            ViewBag.MonthList = new SelectList(list, "Value", "Text");

            List<DropDownViewModel> yearList = new List<DropDownViewModel>();
            yearList.Add(new DropDownViewModel() { Text = "2017", Value = 2017 });
            yearList.Add(new DropDownViewModel() { Text = "2018", Value = 2018 });
            yearList.Add(new DropDownViewModel() { Text = "2019", Value = 2019 });
            yearList.Add(new DropDownViewModel() { Text = "2020", Value = 2020 });
            yearList.Add(new DropDownViewModel() { Text = "2021", Value = 2021 });
            yearList.Add(new DropDownViewModel() { Text = "2022", Value = 2022 });
            yearList.Add(new DropDownViewModel() { Text = "2023", Value = 2023 });
            yearList.Add(new DropDownViewModel() { Text = "2024", Value = 2024 });
            yearList.Add(new DropDownViewModel() { Text = "2025", Value = 2025 });
            ViewBag.yearList = new SelectList(yearList, "Value", "Text");

            List<DepartmentViewModel> DepartmentList = departmentService.GetDropDownList();
            ViewBag.DepartmentList = new SelectList(DepartmentList, "DepartmentID", "DepartmentName");



            List<ProductionFloorLineViewModel> floorList = GetAllProductionFloor();
            ViewBag.floorList = new SelectList(floorList, "Floor", "Floor");

            List<DropDownViewModel> isActive = new List<DropDownViewModel>();
            isActive.Add(new DropDownViewModel() { Text = "Yes", Value = 1 });
            isActive.Add(new DropDownViewModel() { Text = "No", Value = 0 });
            ViewBag.IsActive = new SelectList(isActive, "Value", "Text");

            List<DropDownViewModel> bankSalaryList = new List<DropDownViewModel>();
            bankSalaryList.Add(new DropDownViewModel() { Text = "Yes", Value = 1 });
            bankSalaryList.Add(new DropDownViewModel() { Text = "No", Value = 0 });

            ViewBag.IsBankSalaryList = new SelectList(bankSalaryList, "Value", "Text");
            return View();
        }



        [HttpPost]
        public ActionResult GetStaffSalarySheetReport(ReportFilteringViewModel dReport)
        {
            try
            {
                var id = UserHelper.Instance.Get().BranchId;
                List<SalarySheetViewModel> salarySheetReportList = reportingService.GetStaffSalarySheetReport(dReport, id); //reportingService.GE(dReport, id);
                DataSet ds = new DataSet();
                ds.Tables.Add(salarySheetReportList.ConvertToDataTable());
                ReportHelper.SetData("StaffSalarySheetReport", ds, dReport);
                return Redirect("~/Reports/ReportViewer.aspx");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        #endregion

        #region NightBill
        public ActionResult NightBill()
        {
            SelectList shiftList = new SelectList(officeTimingService.GetWorkingShiftsDropDown(), "Value", "Text");
            ViewBag.shiftList = shiftList;

            List<DepartmentViewModel> DepartmentList = departmentService.GetDropDownList();
            ViewBag.DepartmentList = new SelectList(DepartmentList, "DepartmentID", "DepartmentName");

            List<ProductionFloorLineViewModel> floorList = GetAllProductionFloor();
            ViewBag.floorList = new SelectList(floorList, "Floor", "Floor");
            return View();
        }



        [HttpPost]
        public ActionResult GetNightBillReport(ReportFilteringViewModel dReport)
        {
            try
            {
                var id = UserHelper.Instance.Get().BranchId;
                List<TiffinBillViewModel> nightBillReportList = reportingService.GetNightBillReport(dReport, id); //reportingService.GE(dReport, id);
                DataSet ds = new DataSet();
                ds.Tables.Add(nightBillReportList.ConvertToDataTable());
                ReportHelper.SetData("NightBillReport", ds, dReport);
                return Redirect("~/Reports/ReportViewer.aspx");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        #endregion

        #region Employee Id Card Report
        public ActionResult EmployeeIdCard()
        {
            return View();
        }



        [HttpPost]
        public ActionResult GetEmployeeIdCardReport(ReportFilteringViewModel dReport)
        {
            try
            {
                var id = UserHelper.Instance.Get().BranchId;

                dReport.FromDate = dReport.FromDate ?? DateTime.Now;

                if (!dReport.IsTempIDCard)
                {
                    List<EmployeeIdCardViewModel> employeeIdCardReport = reportingService.GetEmployeeIdCardReport(dReport, id); //reportingService.GE(dReport, id);
                    DataSet ds = new DataSet();
                    ds.Tables.Add(employeeIdCardReport.ConvertToDataTable());
                    ReportHelper.SetData("EmployeeIdCardReport", ds, dReport);
                    return Redirect("~/Reports/ReportViewer.aspx");
                }
                else
                {
                    DataSet ds = new DataSet();
                    ReportHelper.SetData("EmployeeTempIdCardReport", ds, dReport);
                    return Redirect("~/Reports/ReportViewer.aspx");
                }

            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        #endregion

        #region Attendance Summary Report
        public ActionResult AttendanceSummaryReport()
        {
            SelectList shiftList = new SelectList(officeTimingService.GetWorkingShiftsDropDown(), "Value", "Text");
            ViewBag.shiftList = shiftList;

            List<ProductionFloorLineViewModel> floorList = GetAllProductionFloor();
            ViewBag.floorList = new SelectList(floorList, "Floor", "Floor");
            return View();
        }



        [HttpPost]
        public ActionResult GetAttendanceSummaryReport(ReportFilteringViewModel dReport)
        {
            try
            {
                var id = UserHelper.Instance.Get().BranchId;
                List<AttendanceSummaryViewModel> attendancesummaryReportList = reportingService.GetAttendanceSummaryReport(dReport, id); //reportingService.GE(dReport, id);
                DataSet ds = new DataSet();
                ds.Tables.Add(attendancesummaryReportList.ConvertToDataTable());
                ReportHelper.SetData("AttendanceSummaryReport", ds, dReport);
                return Redirect("~/Reports/ReportViewer.aspx");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        #endregion

        #region Comperative Summary Report
        public ActionResult ComperativeSummaryReport()
        {
            List<DropDownViewModel> list = new List<DropDownViewModel>();
            list.Add(new DropDownViewModel() { Text = "January", Value = 1 });
            list.Add(new DropDownViewModel() { Text = "February", Value = 2 });
            list.Add(new DropDownViewModel() { Text = "March", Value = 3 });
            list.Add(new DropDownViewModel() { Text = "April", Value = 4 });
            list.Add(new DropDownViewModel() { Text = "May", Value = 5 });
            list.Add(new DropDownViewModel() { Text = "June", Value = 6 });
            list.Add(new DropDownViewModel() { Text = "July", Value = 7 });
            list.Add(new DropDownViewModel() { Text = "August", Value = 8 });
            list.Add(new DropDownViewModel() { Text = "September", Value = 9 });
            list.Add(new DropDownViewModel() { Text = "October", Value = 10 });
            list.Add(new DropDownViewModel() { Text = "November", Value = 11 });
            list.Add(new DropDownViewModel() { Text = "December", Value = 12 });
            ViewBag.MonthList = new SelectList(list, "Value", "Text");

            List<DropDownViewModel> yearList = new List<DropDownViewModel>();
            yearList.Add(new DropDownViewModel() { Text = "2017", Value = 2017 });
            yearList.Add(new DropDownViewModel() { Text = "2018", Value = 2018 });
            yearList.Add(new DropDownViewModel() { Text = "2019", Value = 2019 });
            yearList.Add(new DropDownViewModel() { Text = "2020", Value = 2020 });
            yearList.Add(new DropDownViewModel() { Text = "2021", Value = 2021 });
            yearList.Add(new DropDownViewModel() { Text = "2022", Value = 2022 });
            yearList.Add(new DropDownViewModel() { Text = "2023", Value = 2023 });
            yearList.Add(new DropDownViewModel() { Text = "2024", Value = 2024 });
            yearList.Add(new DropDownViewModel() { Text = "2025", Value = 2025 });
            ViewBag.yearList = new SelectList(yearList, "Value", "Text");

            SelectList shiftList = new SelectList(officeTimingService.GetWorkingShiftsDropDown(), "Value", "Text");
            ViewBag.shiftList = shiftList;

            List<ProductionFloorLineViewModel> floorList = GetAllProductionFloor();
            ViewBag.floorList = new SelectList(floorList, "Floor", "Floor");
            return View();
        }



        [HttpPost]
        public ActionResult GetComperativeSummaryReport(ReportFilteringViewModel dReport)
        {
            try
            {
                var id = UserHelper.Instance.Get().BranchId;
                List<ComperativeSummaryReportViewModel> comperativeReportList = reportingService.GetComperativeSummaryReport(dReport, id); //reportingService.GE(dReport, id);
                DataSet ds = new DataSet();
                ds.Tables.Add(comperativeReportList.ConvertToDataTable());
                ReportHelper.SetData("ComparetiveSummaryReport", ds, dReport);
                return Redirect("~/Reports/ReportViewer.aspx");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        #endregion

        #region Bank Salary Pad Report
        public ActionResult BankSalaryPadReport()
        {
            List<DropDownViewModel> list = new List<DropDownViewModel>();
            list.Add(new DropDownViewModel() { Text = "January", Value = 1 });
            list.Add(new DropDownViewModel() { Text = "February", Value = 2 });
            list.Add(new DropDownViewModel() { Text = "March", Value = 3 });
            list.Add(new DropDownViewModel() { Text = "April", Value = 4 });
            list.Add(new DropDownViewModel() { Text = "May", Value = 5 });
            list.Add(new DropDownViewModel() { Text = "June", Value = 6 });
            list.Add(new DropDownViewModel() { Text = "July", Value = 7 });
            list.Add(new DropDownViewModel() { Text = "August", Value = 8 });
            list.Add(new DropDownViewModel() { Text = "September", Value = 9 });
            list.Add(new DropDownViewModel() { Text = "October", Value = 10 });
            list.Add(new DropDownViewModel() { Text = "November", Value = 11 });
            list.Add(new DropDownViewModel() { Text = "December", Value = 12 });
            ViewBag.MonthList = new SelectList(list, "Value", "Text");

            List<DropDownViewModel> yearList = new List<DropDownViewModel>();
            yearList.Add(new DropDownViewModel() { Text = "2017", Value = 2017 });
            yearList.Add(new DropDownViewModel() { Text = "2018", Value = 2018 });
            yearList.Add(new DropDownViewModel() { Text = "2019", Value = 2019 });
            yearList.Add(new DropDownViewModel() { Text = "2020", Value = 2020 });
            yearList.Add(new DropDownViewModel() { Text = "2021", Value = 2021 });
            yearList.Add(new DropDownViewModel() { Text = "2022", Value = 2022 });
            yearList.Add(new DropDownViewModel() { Text = "2023", Value = 2023 });
            yearList.Add(new DropDownViewModel() { Text = "2024", Value = 2024 });
            yearList.Add(new DropDownViewModel() { Text = "2025", Value = 2025 });
            ViewBag.yearList = new SelectList(yearList, "Value", "Text");

            List<DropDownViewModel> bankList = new List<DropDownViewModel>();
            bankList.Add(new DropDownViewModel() { Text = "Brac", StringValue = "Brac" });
            bankList.Add(new DropDownViewModel() { Text = "Exim", StringValue = "Exim" });

            ViewBag.BankList = new SelectList(bankList, "StringValue", "Text");

            List<DropDownViewModel> companyList = new List<DropDownViewModel>();
            companyList.Add(new DropDownViewModel() { Text = "DMC", StringValue = "DMC" });
            companyList.Add(new DropDownViewModel() { Text = "Arunima", StringValue = "Arunima" });
            companyList.Add(new DropDownViewModel() { Text = "BDC", StringValue = "BDC" });

            ViewBag.CompanyList = new SelectList(companyList, "StringValue", "Text");

            //SelectList shiftList = new SelectList(officeTimingService.GetWorkingShiftsDropDown(), "Value", "Text");
            //ViewBag.shiftList = shiftList;

            //List<ProductionFloorLineViewModel> floorList = GetAllProductionFloor();
            //ViewBag.floorList = new SelectList(floorList, "Floor", "Floor");
            return View();
        }



        [HttpPost]
        public ActionResult GetBankSalaryPadReport(ReportFilteringViewModel dReport)
        {
            try
            {
                var id = UserHelper.Instance.Get().BranchId;
                List<BankSalaryPadReportViewModel> bankSalaryReportList = reportingService.GetBankSalaryPadReport(dReport, id); //reportingService.GE(dReport, id);
                DataSet ds = new DataSet();
                ds.Tables.Add(bankSalaryReportList.ConvertToDataTable());
                ReportHelper.SetData("BankSalaryPadReport", ds, dReport);
                return Redirect("~/Reports/ReportViewer.aspx");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        #endregion


        #region Droped Out Employees
        public ActionResult DropedOutEmployeesReport()
        {
            SelectList shiftList = new SelectList(officeTimingService.GetWorkingShiftsDropDown(), "Value", "Text");
            ViewBag.shiftList = shiftList;
            List<ProductionFloorLineViewModel> floorList = GetAllProductionFloor();
            ViewBag.floorList = new SelectList(floorList, "Floor", "Floor");


            return View();
        }

        [HttpPost]
        public ActionResult GetDropedOutEmployeesReport(ReportFilteringViewModel dReport)
        {
            try
            {
                List<DropOutReportViewModel> dropOutReportList = reportingService.GetDropedOutEmployeesReport(dReport, UserHelper.Instance.Get().BranchId);
                DataSet ds = new DataSet();
                ds.Tables.Add(dropOutReportList.ConvertToDataTable());
                ReportHelper.SetData("DropOutEmployeeReport", ds, dReport);
                //return Redirect("~/Reports/ReportViewer.aspx");
                return Redirect("~/Reports/ReportViewer.aspx");
                //return Json(overTimeReportList);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }
        #endregion

        #region Droped Out Employees And New Join Employees Summary
        public ActionResult DropedNewJoinEmployeesInfoReport()
        {
            List<ProductionFloorLineViewModel> floorList = GetAllProductionFloor();
            ViewBag.floorList = new SelectList(floorList, "Floor", "Floor");
            return View();
        }

        [HttpPost]
        public ActionResult GetDropedNewJoinEmployeesInfoReport(ReportFilteringViewModel dReport)
        {
            try
            {
                List<DropedNewEmployeeInfoViewModel> dropOutReportList = reportingService.GetDropedNewJoinEmployeesInfoReport(dReport, UserHelper.Instance.Get().BranchId);
                DataSet ds = new DataSet();
                ds.Tables.Add(dropOutReportList.ConvertToDataTable());
                ReportHelper.SetData("DropedNewEmployeeInfoReport", ds, dReport);
                //return Redirect("~/Reports/ReportViewer.aspx");
                return Redirect("~/Reports/ReportViewer.aspx");
                //return Json(overTimeReportList);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }
        #endregion

        #region Earned Leave Calculation Report
        public ActionResult EarnedLeaveCalculationReport()
        {
            List<ProductionFloorLineViewModel> floorList = GetAllProductionFloor();
            ViewBag.floorList = new SelectList(floorList, "Floor", "Floor");

            List<DropDownViewModel> employeeTypeList = new List<DropDownViewModel>();
            employeeTypeList.Add(new DropDownViewModel() { Text = "Worker", Value = 1 });
            employeeTypeList.Add(new DropDownViewModel() { Text = "Staff", Value = 2 });
            ViewBag.employeeTypeList = new SelectList(employeeTypeList, "Value", "Text");

            List<DropDownViewModel> list = new List<DropDownViewModel>();
            list.Add(new DropDownViewModel() { Text = "January", Value = 1 });
            list.Add(new DropDownViewModel() { Text = "February", Value = 2 });
            list.Add(new DropDownViewModel() { Text = "March", Value = 3 });
            list.Add(new DropDownViewModel() { Text = "April", Value = 4 });
            list.Add(new DropDownViewModel() { Text = "May", Value = 5 });
            list.Add(new DropDownViewModel() { Text = "June", Value = 6 });
            list.Add(new DropDownViewModel() { Text = "July", Value = 7 });
            list.Add(new DropDownViewModel() { Text = "August", Value = 8 });
            list.Add(new DropDownViewModel() { Text = "September", Value = 9 });
            list.Add(new DropDownViewModel() { Text = "October", Value = 10 });
            list.Add(new DropDownViewModel() { Text = "November", Value = 11 });
            list.Add(new DropDownViewModel() { Text = "December", Value = 12 });
            ViewBag.MonthList = new SelectList(list, "Value", "Text");

            List<DropDownViewModel> yearList = new List<DropDownViewModel>();
            yearList.Add(new DropDownViewModel() { Text = "2017", Value = 2017 });
            yearList.Add(new DropDownViewModel() { Text = "2018", Value = 2018 });
            yearList.Add(new DropDownViewModel() { Text = "2019", Value = 2019 });
            yearList.Add(new DropDownViewModel() { Text = "2020", Value = 2020 });
            yearList.Add(new DropDownViewModel() { Text = "2021", Value = 2021 });
            yearList.Add(new DropDownViewModel() { Text = "2022", Value = 2022 });
            yearList.Add(new DropDownViewModel() { Text = "2023", Value = 2023 });
            yearList.Add(new DropDownViewModel() { Text = "2024", Value = 2024 });
            yearList.Add(new DropDownViewModel() { Text = "2025", Value = 2025 });
            ViewBag.yearList = new SelectList(yearList, "Value", "Text");

            return View();
        }

        [HttpPost]
        public ActionResult GetEarnedLeaveCalculationReport(ReportFilteringViewModel dReport)
        {
            try
            {
                List<EarnedLeaveCalculationReportViewModel> dropOutReportList = reportingService.GetEarnedLeaveCalculationReport(dReport, UserHelper.Instance.Get().BranchId);
                DataSet ds = new DataSet();
                ds.Tables.Add(dropOutReportList.ConvertToDataTable());
                ReportHelper.SetData("EarnedLeaveCalculationReport", ds, dReport);
                //return Redirect("~/Reports/ReportViewer.aspx");
                return Redirect("~/Reports/ReportViewer.aspx");
                //return Json(overTimeReportList);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }
        #endregion

        #region Employee Leave Posting Report

        public ActionResult EmployeeLeavePostingReport()
        {
            List<ProductionFloorLineViewModel> floorList = GetAllProductionFloor();
            ViewBag.floorList = new SelectList(floorList, "Floor", "Floor");
            return View();
        }

        public ActionResult GetEmployeeLeavePostingReport(ReportFilteringViewModel reportFilteringVM)
        {
            try
            {
                List<EmployeeLeaveHistoryViewModel> dropOutReportList = reportingService.GetEmployeeLeavePostingReport(reportFilteringVM);
                DataSet ds = new DataSet();
                ds.Tables.Add(dropOutReportList.ConvertToDataTable());
                ReportHelper.SetData("EmployeeLeavePostingReport", ds, reportFilteringVM);
                //return Redirect("~/Reports/ReportViewer.aspx");
                return Redirect("~/Reports/ReportViewer.aspx");
                //return Json(overTimeReportList);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region HolidayBill
        public ActionResult HolidayBill()
        {
            SelectList shiftList = new SelectList(officeTimingService.GetWorkingShiftsDropDown(), "Value", "Text");
            ViewBag.shiftList = shiftList;

            List<DepartmentViewModel> DepartmentList = departmentService.GetDropDownList();
            ViewBag.DepartmentList = new SelectList(DepartmentList, "DepartmentID", "DepartmentName");

            List<ProductionFloorLineViewModel> floorList = GetAllProductionFloor();
            ViewBag.floorList = new SelectList(floorList, "Floor", "Floor");
            return View();
        }



        [HttpPost]
        public ActionResult GetHolidayBillReport(ReportFilteringViewModel dReport)
        {
            try
            {
                var id = UserHelper.Instance.Get().BranchId;
                List<HolidayBillViewModel> holidayBillReportList = reportingService.GetHolidayBillReport(dReport, id); //reportingService.GE(dReport, id);
                DataSet ds = new DataSet();
                ds.Tables.Add(holidayBillReportList.ConvertToDataTable());
                ReportHelper.SetData("HolidayBillReport", ds, dReport);
                return Redirect("~/Reports/ReportViewer.aspx");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        #endregion


        #region Promotion Report
        public ActionResult PromotionReport()
        {
            SelectList floorList = new SelectList(productionService.GetProductionFloorList(), "Floor", "Floor");
            ViewBag.floorList = floorList;
            SelectList shiftList = new SelectList(officeTimingService.GetWorkingShiftsDropDown(), "Value", "Text");
            ViewBag.shiftList = shiftList;
            return View();
        }

        [HttpPost]
        public ActionResult GetPromotionReport(ReportFilteringViewModel dReport)
        {
            try
            {
                List<PromotionViewModel> promotionReportList = reportingService.GetPromotionReport(dReport);
                DataSet ds = new DataSet();
                ds.Tables.Add(promotionReportList.ConvertToDataTable());
                ReportHelper.SetData("PromotionReport", ds, dReport);
                //return Redirect("~/Reports/ReportViewer.aspx");
                return Redirect("~/Reports/ReportViewer.aspx");
                //return Json(overTimeReportList);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }
        #endregion


        #region Maternity Report
        public ActionResult MaternityReport()
        {
            SelectList floorList = new SelectList(productionService.GetProductionFloorList(), "Floor", "Floor");
            ViewBag.floorList = floorList;
            SelectList shiftList = new SelectList(officeTimingService.GetWorkingShiftsDropDown(), "Value", "Text");
            ViewBag.shiftList = shiftList;
            return View();
        }

        [HttpPost]
        public ActionResult GetMaternityReport(ReportFilteringViewModel dReport)
        {
            try
            {
                List<MaternityViewModel> maternityReportList = reportingService.GetMaternityReport(dReport);
                DataSet ds = new DataSet();
                ds.Tables.Add(maternityReportList.ConvertToDataTable());
                ReportHelper.SetData("MaternityReport", ds, dReport);
                //return Redirect("~/Reports/ReportViewer.aspx");
                return Redirect("~/Reports/ReportViewer.aspx");
                //return Json(overTimeReportList);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }
        #endregion

        #region Bonus Report
        public ActionResult EidBonusReport()
        {
            List<ProductionFloorLineViewModel> floorList = GetAllProductionFloor();
            ViewBag.floorList = new SelectList(floorList, "Floor", "Floor");

            List<DropDownViewModel> bankSalaryList = new List<DropDownViewModel>();
            bankSalaryList.Add(new DropDownViewModel() { Text = "Yes", Value = 1 });
            bankSalaryList.Add(new DropDownViewModel() { Text = "No", Value = 0 });

            ViewBag.IsBankSalaryList = new SelectList(bankSalaryList, "Value", "Text");

            List<DropDownViewModel> companyList = new List<DropDownViewModel>();
            companyList.Add(new DropDownViewModel() { Text = "DMC", StringValue = "DMC" });
            companyList.Add(new DropDownViewModel() { Text = "Arunima", StringValue = "Arunima" });
            companyList.Add(new DropDownViewModel() { Text = "BDC", StringValue = "BDC" });

            ViewBag.CompanyList = new SelectList(companyList, "StringValue", "Text");

            return View();
        }

        [HttpPost]
        public ActionResult GetEidBonusReport(ReportFilteringViewModel dReport)
        {
            try
            {
                List<EidBonusVIewModel> bonusList = reportingService.GetEidBonusReport(dReport, UserHelper.Instance.Get().BranchId);
                DataSet ds = new DataSet();
                ds.Tables.Add(bonusList.ConvertToDataTable());
                ReportHelper.SetData("EidBonusReport", ds, dReport);
                //return Redirect("~/Reports/ReportViewer.aspx");
                return Redirect("~/Reports/ReportViewer.aspx");
                //return Json(overTimeReportList);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }
        #endregion

        #region Register Of Workers
        public ActionResult RegisterOfWorkerReport()
        {
            //SelectList floorList = new SelectList(productionService.GetProductionFloorList(), "Floor", "Floor");
            //ViewBag.floorList = floorList;
            //SelectList shiftList = new SelectList(officeTimingService.GetWorkingShiftsDropDown(), "Value", "Text");
            //ViewBag.shiftList = shiftList;
            return View();
        }

        [HttpPost]
        public ActionResult GetRegisterOfWorkerReport(ReportFilteringViewModel dReport)
        {
            try
            {
                List<RegisterOfWorkerViewModel> registerOfWorkerList = reportingService.GetRegisterOfWorkerReport(dReport);
                DataSet ds = new DataSet();
                ds.Tables.Add(registerOfWorkerList.ConvertToDataTable());
                ReportHelper.SetData("RegisterOfWorkerReport", ds, dReport);
                //return Redirect("~/Reports/ReportViewer.aspx");
                return Redirect("~/Reports/ReportViewer.aspx");
                //return Json(overTimeReportList);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }
        #endregion


        #region Leave Approval  Report
        public ActionResult LeaveApprovalReport()
        {
            //SelectList floorList = new SelectList(productionService.GetProductionFloorList(), "Floor", "Floor");
            //ViewBag.floorList = floorList;
            //SelectList shiftList = new SelectList(officeTimingService.GetWorkingShiftsDropDown(), "Value", "Text");
            //ViewBag.shiftList = shiftList;
            return View();
        }

        [HttpPost]
        public ActionResult GetLeaveApprovalReport(ReportFilteringViewModel dReport)
        {
            try
            {
                List<LeaveApprovalReportViewModel> leaveApprovalDetails = reportingService.GetLeaveApprovalReport(dReport);
                DataSet ds = new DataSet();
                ds.Tables.Add(leaveApprovalDetails.ConvertToDataTable());
                ReportHelper.SetData("LeaveApprovalReport", ds, dReport);
                //return Redirect("~/Reports/ReportViewer.aspx");
                return Redirect("~/Reports/ReportViewer.aspx");
                //return Json(overTimeReportList);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }


        public JsonResult GetLeaveDetailsByCardNo(string cardNo)
        {
            var data = leaveLogic.GetAllAppsByEmployeeCardNo(UserHelper.Instance.Get().BranchId, User.Identity.Name, cardNo);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Employee All History Data
        public ActionResult EmployeeAllHistoryData()
        {
            return View();
        }



        [HttpPost]
        public ActionResult GetEmployeeAllHistoryData(ReportFilteringViewModel dReport)
        {
            try
            {
                var id = UserHelper.Instance.Get().BranchId;

                List<EmployeeDetailsViewModel> basicData = reportingService.GetEmployeeBasicData(dReport, id); //reportingService.GE(dReport, id);
                List<EmployeeLeaveHistoryViewModel> leaveHistory = reportingService.GetEmployeeLeaveHistoryReport(dReport);
                List<SalaryIncrementViewModel> incrementHistory = reportingService.GetIncrementReportByCardNo(dReport.CardNo);
                List<PromotionViewModel> promotionHistory = reportingService.GetEmployeeePromotionDetailsByCardNo(dReport.CardNo);
                List<NoticeViewModel> noticeHistory = reportingService.GetNoticeHistoryByCardNo(dReport.CardNo);
                DataSet ds = new DataSet();
                ds.Tables.Add(basicData.ConvertToDataTable());
                ds.Tables.Add(leaveHistory.ConvertToDataTable());
                ds.Tables.Add(incrementHistory.ConvertToDataTable());
                ds.Tables.Add(promotionHistory.ConvertToDataTable());
                ds.Tables.Add(noticeHistory.ConvertToDataTable());
                ReportHelper.SetData("EmployeeAllHistoryDataReport", ds, dReport);
                return Redirect("~/Reports/ReportViewer.aspx");


            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        #endregion

        #region Worker Bus Report
        public ActionResult WorkerBusReport()
        {
            SelectList floorList = new SelectList(productionService.GetProductionFloorList(), "Floor", "Floor");
            ViewBag.floorList = floorList;
            return View();
        }



        [HttpPost]
        public ActionResult GetWorkerBusReport(ReportFilteringViewModel dReport)
        {
            try
            {
                List<WorkerBusReportViewModel> data = reportingService.GetWorkerBusReport(dReport);

                DataSet ds = new DataSet();

                ds.Tables.Add(data.ConvertToDataTable());
                ReportHelper.SetData("WorkerBusReport", ds, dReport);
                return Redirect("~/Reports/ReportViewer.aspx");


            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        #endregion

        #region Salary Summary Report
        public ActionResult SalarySummaryReport()
        {
            List<DropDownViewModel> list = new List<DropDownViewModel>();
            list.Add(new DropDownViewModel() { Text = "January", Value = 1 });
            list.Add(new DropDownViewModel() { Text = "February", Value = 2 });
            list.Add(new DropDownViewModel() { Text = "March", Value = 3 });
            list.Add(new DropDownViewModel() { Text = "April", Value = 4 });
            list.Add(new DropDownViewModel() { Text = "May", Value = 5 });
            list.Add(new DropDownViewModel() { Text = "June", Value = 6 });
            list.Add(new DropDownViewModel() { Text = "July", Value = 7 });
            list.Add(new DropDownViewModel() { Text = "August", Value = 8 });
            list.Add(new DropDownViewModel() { Text = "September", Value = 9 });
            list.Add(new DropDownViewModel() { Text = "October", Value = 10 });
            list.Add(new DropDownViewModel() { Text = "November", Value = 11 });
            list.Add(new DropDownViewModel() { Text = "December", Value = 12 });

            ViewBag.monthList = new SelectList(list, "Value", "Text");

            List<DropDownViewModel> yearList = new List<DropDownViewModel>();
            yearList.Add(new DropDownViewModel() { Text = "2017", Value = 2017 });
            yearList.Add(new DropDownViewModel() { Text = "2018", Value = 2018 });
            yearList.Add(new DropDownViewModel() { Text = "2019", Value = 2019 });
            yearList.Add(new DropDownViewModel() { Text = "2020", Value = 2020 });
            yearList.Add(new DropDownViewModel() { Text = "2021", Value = 2021 });
            yearList.Add(new DropDownViewModel() { Text = "2022", Value = 2022 });
            yearList.Add(new DropDownViewModel() { Text = "2023", Value = 2023 });
            yearList.Add(new DropDownViewModel() { Text = "2024", Value = 2024 });
            yearList.Add(new DropDownViewModel() { Text = "2025", Value = 2025 });

            ViewBag.yearList = new SelectList(yearList, "Value", "Text");

            return View();
        }



        [HttpPost]
        public ActionResult GetSalarySummaryReport(ReportFilteringViewModel dReport)
        {
            try
            {
                List<SalarySummaryViewModel> data = reportingService.GetSalarySummaryReport(dReport);

                DataSet ds = new DataSet();

                ds.Tables.Add(data.ConvertToDataTable());
                ReportHelper.SetData("SalarySummaryReport", ds, dReport);
                return Redirect("~/Reports/ReportViewer.aspx");


            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        #endregion


        #region Leave Correction Report
        public ActionResult LeaveCorrectionReport()
        {
            return View();
        }



        [HttpPost]
        public ActionResult GetLeaveCorrectionReport(ReportFilteringViewModel dReport)
        {
            try
            {
                List<OneTimeEntryReportViewModel> data = reportingService.LeaveCorrectionReport(dReport);

                DataSet ds = new DataSet();

                ds.Tables.Add(data.ConvertToDataTable());
                ReportHelper.SetData("LeaveCorrectionReport", ds, dReport);
                return Redirect("~/Reports/ReportViewer.aspx");


            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        #endregion


        #region Maternity Bill Pay Report
        public ActionResult MaternityBillPayReport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetMaternityBillPayReport(ReportFilteringViewModel dReport)
        {
            try
            {
                List<MaternityReportViewModel> maternityReportList = reportingService.GetMaternityBillPayReport(dReport);
                DataSet ds = new DataSet();
                ds.Tables.Add(maternityReportList.ConvertToDataTable());

                if (dReport.FirstInstallment)
                {
                    ReportHelper.SetData("FirstMaternityBillPayReport", ds, dReport);
                }
                else {
                    ReportHelper.SetData("SecondMaternityBillPayReport", ds, dReport);
                }
                
                //return Redirect("~/Reports/ReportViewer.aspx");
                return Redirect("~/Reports/ReportViewer.aspx");
                //return Json(overTimeReportList);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }
        public JsonResult GetMaternityLeaveDetailsByCardNo(string cardNo)
        {
            var data = maternityService.GetEmployeeeMaternityDetailsByCardNo(cardNo);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Substitute Holiday Report
        public ActionResult SubstituteHolidayReport()
        {
            return View();
        }



        [HttpPost]
        public ActionResult GetSubstituteHolidayReport(ReportFilteringViewModel dReport)
        {
            try
            {
                List<SubstituteHolidayReportViewModel> data = reportingService.GetSubstituteHolidayReport(dReport);

                DataSet ds = new DataSet();

                ds.Tables.Add(data.ConvertToDataTable());
                ReportHelper.SetData("SubstituteHolidayReport", ds, dReport);
                return Redirect("~/Reports/ReportViewer.aspx");


            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        #endregion

        #region Employee Final Bill Payment Report
        public ActionResult EmployeeFinalBillPaymentReport()
        {
            return View();
        }



        [HttpPost]
        public ActionResult GetEmployeeFinalBillPaymentReport(ReportFilteringViewModel dReport)
        {
            try
            {
                List<EmployeeFinalBillReportViewModel> data = reportingService.GetEmployeeFinalBillPaymentReport(dReport);

                DataSet ds = new DataSet();

                ds.Tables.Add(data.ConvertToDataTable());
                ReportHelper.SetData("EmployeeFinalBillPayReport", ds, dReport);
                return Redirect("~/Reports/ReportViewer.aspx");


            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        #endregion


        #region Earn Leave Summary Report
        public ActionResult EarnLeaveSummaryReport()
        {
            List<DropDownViewModel> list = new List<DropDownViewModel>();
            list.Add(new DropDownViewModel() { Text = "January", Value = 1 });
            list.Add(new DropDownViewModel() { Text = "February", Value = 2 });
            list.Add(new DropDownViewModel() { Text = "March", Value = 3 });
            list.Add(new DropDownViewModel() { Text = "April", Value = 4 });
            list.Add(new DropDownViewModel() { Text = "May", Value = 5 });
            list.Add(new DropDownViewModel() { Text = "June", Value = 6 });
            list.Add(new DropDownViewModel() { Text = "July", Value = 7 });
            list.Add(new DropDownViewModel() { Text = "August", Value = 8 });
            list.Add(new DropDownViewModel() { Text = "September", Value = 9 });
            list.Add(new DropDownViewModel() { Text = "October", Value = 10 });
            list.Add(new DropDownViewModel() { Text = "November", Value = 11 });
            list.Add(new DropDownViewModel() { Text = "December", Value = 12 });
            ViewBag.MonthList = new SelectList(list, "Value", "Text");

            List<DropDownViewModel> yearList = new List<DropDownViewModel>();
            yearList.Add(new DropDownViewModel() { Text = "2017", Value = 2017 });
            yearList.Add(new DropDownViewModel() { Text = "2018", Value = 2018 });
            yearList.Add(new DropDownViewModel() { Text = "2019", Value = 2019 });
            yearList.Add(new DropDownViewModel() { Text = "2020", Value = 2020 });
            yearList.Add(new DropDownViewModel() { Text = "2021", Value = 2021 });
            yearList.Add(new DropDownViewModel() { Text = "2022", Value = 2022 });
            yearList.Add(new DropDownViewModel() { Text = "2023", Value = 2023 });
            yearList.Add(new DropDownViewModel() { Text = "2024", Value = 2024 });
            yearList.Add(new DropDownViewModel() { Text = "2025", Value = 2025 });
            ViewBag.yearList = new SelectList(yearList, "Value", "Text");

            List<ProductionFloorLineViewModel> floorList = GetAllProductionFloor();
            ViewBag.floorList = new SelectList(floorList, "Floor", "Floor");

            return View();
        }

        [HttpPost]
        public ActionResult GetEarnLeaveSummaryReport(ReportFilteringViewModel dReport)
        {
            try
            {
                List<EarnLeaveSummaryReportViewModel> list = reportingService.GetEarnLeaveSummaryReport(dReport);
                DataSet ds = new DataSet();
                ds.Tables.Add(list.ConvertToDataTable());
                ReportHelper.SetData("EarnLeaveSummaryReport", ds, dReport);
                //return Redirect("~/Reports/ReportViewer.aspx");
                return Redirect("~/Reports/ReportViewer.aspx");
                //return Json(overTimeReportList);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }
        #endregion


        #region Employee Leave Posting Summary Report

        public ActionResult EmployeeLeavePostingSummaryReport()
        {
            List<ProductionFloorLineViewModel> floorList = GetAllProductionFloor();
            ViewBag.floorList = new SelectList(floorList, "Floor", "Floor");
            return View();
        }

        public ActionResult GetEmployeeLeavePostingSummaryReport(ReportFilteringViewModel reportFilteringVM)
        {
            try
            {
                List<EmployeeLeaveHistoryViewModel> dropOutReportList = reportingService.GetEmployeeLeavePostingSummaryReport(reportFilteringVM);
                int totalEmployee = employeeService.TotalEmployee(reportFilteringVM.Floor);
                reportFilteringVM.TotalEmployee = totalEmployee;
                DataSet ds = new DataSet();
                ds.Tables.Add(dropOutReportList.ConvertToDataTable());
                ReportHelper.SetData("LeavePostingSummaryReport", ds, reportFilteringVM);
                //return Redirect("~/Reports/ReportViewer.aspx");
                return Redirect("~/Reports/ReportViewer.aspx");
                //return Json(overTimeReportList);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion


        #region All Employee Dynamic report

    
        public ActionResult EmployeeDataDynamicReport()
        {
            return View();
        }
        
        [HttpGet]
        public JsonResult GetEmployeeDataDynamicReport(int skip = 0, int page = 1)
        {
            try
            {
                int pageSize = 30;
                List<EmployeeDetailsViewModel> data = reportingService.GetAllEmployeeDetailsReport(pageSize, skip * pageSize);
                return Json(data,JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        #endregion


        #region Security Guard Roster Report 

        public ActionResult SecurityGuardRosterReport()
        {
            return View();
        }

        public ActionResult GetSecurityGuardRosterReport(ReportFilteringViewModel reportFilteringVM)
        {
            try
            {
                List<SecurityGuardRosterViewModel> dropOutReportList = reportingService.GetSecurityGuardRosterReport(reportFilteringVM);
                DataSet ds = new DataSet();
                ds.Tables.Add(dropOutReportList.ConvertToDataTable());
                ReportHelper.SetData("SecurityGuardRosterReport", ds, reportFilteringVM);
                //return Redirect("~/Reports/ReportViewer.aspx");
                return Redirect("~/Reports/ReportViewer.aspx");
                //return Json(overTimeReportList);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Prescription Report
        [HttpPost]
        public ActionResult GetPrescriptionReport(ReportFilteringViewModel reportFilterVM)
        {
            try
            {


                List<PrescriptionViewModel> data = reportingService.GetPrescriptionReport(reportFilterVM.PrescriptionID);
                DataSet ds = new DataSet();
                ds.Tables.Add(data.ConvertToDataTable());
                ReportHelper.SetData("PrescriptionReport", ds, reportFilterVM);
                return Redirect("~/Reports/ReportViewer.aspx");
                //return Json(overTimeReportList);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }



}