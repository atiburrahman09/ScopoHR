using Org.BouncyCastle.Asn1.Ocsp;
using ScopoHR.Core.Helpers;
using ScopoHR.Core.ViewModels;
using ScopoHR.Domain.Models;
using ScopoHR.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using QRCoder;
using System.Drawing;

namespace ScopoHR.Core.Services
{
    public class ReportingService
    {
        private UnitOfWork _unitOfWork;
        private PublishNotice publishNotice;
        public ReportingService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<WeeklyReportViewModel> GetAuditReportDetail(ReportFilteringViewModel reportFilterVM, int branchId)
        {
            var query = $"EXEC AuditReportDetail '{reportFilterVM.FromDate}', '{reportFilterVM.ToDate}', '{branchId}','{reportFilterVM.ShiftId}','{reportFilterVM.Floor}'";
            var result = _unitOfWork.DailyAttendanceRepository.SelectQuery<WeeklyReportViewModel>(query).ToList();
            return result;
        }

        public List<WeeklyReportViewModel> GetAuditReportTotalHoursAndNoOfWorkers(ReportFilteringViewModel reportFilterVM, int branchId)
        {
            var query = $"EXEC AuditReportTotalHourAndNoOfWorker '{reportFilterVM.FromDate}', '{reportFilterVM.ToDate}', '{branchId}','{reportFilterVM.ShiftId}','{reportFilterVM.Floor}'";
            var result = _unitOfWork.DailyAttendanceRepository.SelectQuery<WeeklyReportViewModel>(query).ToList();
            return result;
        }

        public List<AttendanceReportViewModel> GetAbsentReport(ReportFilteringViewModel dReport)
        {
            var query = $"EXEC GetAbsentReport '{dReport.FromDate.ToString()}','{dReport.FromDate.ToString()}', '{dReport.Floor}','{1}','{dReport.ShiftId}'";
            var result = _unitOfWork.attendanceRepository.SelectQuery<AttendanceReportViewModel>(query).ToList();
            return result;
        }

        public List<OneTimeEntryReportViewModel> GetOneTimeEntryReport(ReportFilteringViewModel dReport)
        {
            var query = $"EXEC GetOneTimePunchReport '" + dReport.FromDate.ToString() + "','" + dReport.ShiftId + "','" + dReport.Floor + "','" + dReport.ToDate + "'";
            //var query = $"EXEC GetOneTimeEntryReportByDate '{dReport.FromDate.ToString()}'";
            var result = _unitOfWork.attendanceRepository.SelectQuery<OneTimeEntryReportViewModel>(query).ToList();
            return result;
        }

        public List<EmployeeAttendaceDetailsReportViewmodel> GetEmployeeAttendaceDetailsReport(ReportFilteringViewModel dReport, int branchId)
        {
            var query = $"EXEC GetAttendanceReport '{dReport.FromDate}', '{dReport.ToDate}','', '{branchId}','','{dReport.CardNo}'";
            var result = _unitOfWork.DailyAttendanceRepository.SelectQuery<EmployeeAttendaceDetailsReportViewmodel>(query).ToList();
            return result;
        }

        public List<DropOutReportViewModel> GetDropOutReport(ReportFilteringViewModel dReport, int branchId)
        {
            var query = $"EXEC GetDropOutList '" + dReport.FromDate.ToString() + "','" + dReport.Floor + "','" + dReport.ShiftId + "','" + dReport.Days + "'";

            var result = _unitOfWork.attendanceRepository.SelectQuery<DropOutReportViewModel>(query).ToList();
            return result;
        }



        public List<EmployeeDetailsViewModel> GetAllEmployeeDetailsReport(ReportFilteringViewModel dReport)
        {
            var query = $"EXEC GetALLEmployeeDetails '{dReport.FromDate.ToString()}','{dReport.ToDate.ToString()}','{dReport.Floor}', '{dReport.ShiftId}','" + dReport.DepartmentID + "','" + dReport.DesignationID + "','" + dReport.SalaryGrade + "','" + dReport.GenderID + "'";

            var result = _unitOfWork.attendanceRepository.SelectQuery<EmployeeDetailsViewModel>(query).ToList();
            return result;
        }

        public List<EmployeeDetailsViewModel> GetAllEmployeeDetailsReport(int take, int skip)
        {
            var query = $"EXEC GetALLEmployeeDetails '','','', '','','','',''";

            var result = _unitOfWork.attendanceRepository.SelectQuery<EmployeeDetailsViewModel>(query).Take(500).ToList();
            return result;
        }

        public List<EmployeeLeaveHistoryViewModel> GetEmployeeLeaveHistoryReport(ReportFilteringViewModel dReport)
        {
            var query = $"EXEC GetLeaveDetailsByEmployee '{dReport.CardNo}'";
            var result = _unitOfWork.attendanceRepository.SelectQuery<EmployeeLeaveHistoryViewModel>(query).ToList();
            return result;
        }

        public List<DailyReportViewModel> GetEmployeeMinimumWorkingHour(ReportFilteringViewModel reportFilteringVM)
        {
            var query = $"EXEC GetMinimumWorkTime '" + reportFilteringVM.FromDate.ToString() + "','" + reportFilteringVM.Floor + "','" + reportFilteringVM.ShiftId + "'";
            var result = _unitOfWork.attendanceRepository.SelectQuery<DailyReportViewModel>(query).ToList();
            return result;
        }

        public List<DailyReportViewModel> GetExtraDelay(ReportFilteringViewModel reportFilteringVM)
        {
            var query = $"EXEC GetExtraDelay '" + reportFilteringVM.FromDate.ToString() + "','" + reportFilteringVM.Floor + "','" + reportFilteringVM.ShiftId + "'";
            var result = _unitOfWork.attendanceRepository.SelectQuery<DailyReportViewModel>(query).ToList();
            return result;
        }

        public List<SalarySheetViewModel> GetSalarySheetReport(ReportFilteringViewModel dReport, int id)
        {
            var query = $"EXEC GetSalarySheet '" + dReport.Month + "','" + dReport.Floor + "','" + id + "','" + dReport.ShiftId + "','" + dReport.CardNo + "','" + dReport.DepartmentID + "','" + 1 + "','" + dReport.IsActive + "','" + 0 + "','" + dReport.Year + "'";
            var result = _unitOfWork.attendanceRepository.SelectQuery<SalarySheetViewModel>(query).ToList();
            return result;
        }

        public List<WeeklyReportViewModel> GetProgresiveReport(ReportFilteringViewModel reportFilterVM, int branchId)
        {
            var query = $"EXEC GetProgresiveReport '{reportFilterVM.FromDate}', '{reportFilterVM.ToDate}', '{branchId}','{reportFilterVM.ShiftId}','{reportFilterVM.Floor}'";
            var result = _unitOfWork.DailyAttendanceRepository.SelectQuery<WeeklyReportViewModel>(query).ToList();
            return result;
        }

        public List<SalarySheetViewModel> GetSalaryPaySheetReport(ReportFilteringViewModel dReport, int id)
        {
            var query = $"EXEC GetSalarySheet '" + dReport.Month + "','" + dReport.Floor + "','" + id + "','" + dReport.ShiftId + "','" + dReport.CardNo + "','" + dReport.DepartmentID + "','" + dReport.EmployeeType + "','" + dReport.IsActive + "','" + dReport.BankSalary + "','" + dReport.Year + "'";
            var result = _unitOfWork.attendanceRepository.SelectQuery<SalarySheetViewModel>(query).ToList();
            return result;
        }

        public List<WeeklyReportViewModel> GetAuditReportSummaryPerDay(ReportFilteringViewModel reportFilterVM, int id)
        {
            var query = $"EXEC AuditReportSummaryOfWorker '{reportFilterVM.FromDate}', '{reportFilterVM.ToDate}', '{id}','{reportFilterVM.ShiftId}','{reportFilterVM.Floor}'";
            var result = _unitOfWork.DailyAttendanceRepository.SelectQuery<WeeklyReportViewModel>(query).ToList();
            return result;
        }

        public List<EmployeeBioDataViewModel> GetEmployeeBioDataReport(ReportFilteringViewModel reportFilteringVM)
        {
            var query = $"EXEC GetEmployeeBioData '" + reportFilteringVM.CardNo + "'";
            var result = _unitOfWork.attendanceRepository.SelectQuery<EmployeeBioDataViewModel>(query).ToList();
            return result;
        }

        public List<TiffinBillViewModel> GetTiffinBillReport(ReportFilteringViewModel dReport, int id)
        {
            var query = $"EXEC GetTiffinBillData '" + dReport.FromDate + "','" + dReport.ToDate + "','" + dReport.Floor + "','" + id + "','" + dReport.ShiftId + "','" + dReport.CardNo + "','" + dReport.DepartmentID + "'";
            var result = _unitOfWork.attendanceRepository.SelectQuery<TiffinBillViewModel>(query).ToList();
            return result;
        }

        public List<NewRecruitmentViewModel> GetNewRecruitmentReport(ReportFilteringViewModel dReport, int id)
        {
            var query = $"EXEC GetNewRecruitmentList '" + dReport.FromDate + "','" + dReport.ToDate + "','" + dReport.Floor + "','" + id + "','" + dReport.ShiftId + "','" + dReport.CardNo + "','" + dReport.DepartmentID + "'";
            var result = _unitOfWork.attendanceRepository.SelectQuery<NewRecruitmentViewModel>(query).ToList();
            return result;
        }

        public List<IncrementReportViewModel> GetIncrementReport(ReportFilteringViewModel dReport, int id)
        {
            var query = $"EXEC GetIncrementData '" + dReport.FromDate + "','" + dReport.ToDate + "','" + dReport.Floor + "','" + id + "','" + dReport.ShiftId + "','" + dReport.CardNo + "','" + dReport.DepartmentID + "','" + dReport.EmployeeType + "'";
            var result = _unitOfWork.attendanceRepository.SelectQuery<IncrementReportViewModel>(query).ToList();
            return result;
        }

        public List<SalarySheetViewModel> GetStaffSalarySheetReport(ReportFilteringViewModel dReport, int id)
        {
            var query = $"EXEC GetSalarySheet '" + dReport.Month + "','" + dReport.Floor + "','" + id + "','" + dReport.ShiftId + "','" + dReport.CardNo + "','" + dReport.DepartmentID + "','" + 2 + "','" + dReport.IsActive + "','" + dReport.BankSalary + "','" + dReport.Year + "'";
            var result = _unitOfWork.attendanceRepository.SelectQuery<SalarySheetViewModel>(query).ToList();
            return result;
        }

        public List<TiffinBillViewModel> GetNightBillReport(ReportFilteringViewModel dReport, int id)
        {
            var query = $"EXEC GetNightBillData '" + dReport.FromDate + "','" + dReport.ToDate + "','" + dReport.Floor + "','" + id + "','" + dReport.ShiftId + "','" + dReport.CardNo + "','" + dReport.DepartmentID + "'";
            var result = _unitOfWork.attendanceRepository.SelectQuery<TiffinBillViewModel>(query).ToList();
            return result;
        }

        public List<EmployeeIdCardViewModel> GetEmployeeIdCardReport(ReportFilteringViewModel dReport, int id)
        {
            var query = $"EXEC GetEmployeeIdCardInfo '" + dReport.CardNo + "'";
            var result = _unitOfWork.attendanceRepository.SelectQuery<EmployeeIdCardViewModel>(query).ToList();

            string baseURL = @"file://" + HostingEnvironment.MapPath(ConfigurationManager.AppSettings.Get(DocumentCategory.ProfilePhoto.ToString()));
            result[0].UniqueIdentifier = baseURL + result[0].UniqueIdentifier;

            result[0].QRCode = GetQrCode(result);
            return result;
        }

        private byte[] GetQrCode(List<EmployeeIdCardViewModel> result)
        {

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(result[0].CardNo + System.Environment.NewLine + result[0].EmployeeName + System.Environment.NewLine + result[0].NID + System.Environment.NewLine + result[0].Telephone, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            byte[] bytes = new byte[0];
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                qrCodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                bytes = stream.ToArray();
            }

            return bytes;
        }

        public List<AttendanceSummaryViewModel> GetAttendanceSummaryReport(ReportFilteringViewModel dReport, int id)
        {
            var query = $"EXEC GetAttendanceSummary '" + dReport.FromDate + "','" + dReport.Floor + "','" + id + "','" + dReport.ShiftId + "'";
            var result = _unitOfWork.attendanceRepository.SelectQuery<AttendanceSummaryViewModel>(query).ToList();
            return result;
        }

        public List<ComperativeSummaryReportViewModel> GetComperativeSummaryReport(ReportFilteringViewModel dReport, int id)
        {
            var query = $"EXEC GetComparetiveStatement '" + dReport.FirstMonth + "','" + dReport.SecondMonth + "','" + dReport.Floor + "','" + id + "','" + dReport.ShiftId + "','" + dReport.FirstYear + "','" + dReport.SecondYear + "'";
            var result = _unitOfWork.attendanceRepository.SelectQuery<ComperativeSummaryReportViewModel>(query).ToList();
            return result;
        }

        public List<BankSalaryPadReportViewModel> GetBankSalaryPadReport(ReportFilteringViewModel dReport, int id)
        {
            var query = $"EXEC GetBankSalaryPad '" + dReport.Month + "','" + dReport.BankName + "','" + dReport.Company + "','" + dReport.Year + "'";
            var result = _unitOfWork.attendanceRepository.SelectQuery<BankSalaryPadReportViewModel>(query).ToList();
            return result;
        }

        public List<DropOutReportViewModel> GetDropedOutEmployeesReport(ReportFilteringViewModel dReport, int branchId)
        {
            var query = $"EXEC GetDropedOutEmployeeList '" + dReport.FromDate.ToString() + "','" + dReport.ToDate.ToString() + "','" + dReport.Floor + "','" + dReport.ShiftId + "'";

            var result = _unitOfWork.attendanceRepository.SelectQuery<DropOutReportViewModel>(query).ToList();
            return result;
        }

        public List<DropedNewEmployeeInfoViewModel> GetDropedNewJoinEmployeesInfoReport(ReportFilteringViewModel dReport, int branchId)
        {
            var query = $"EXEC GetDropedNewJoinEmployeeInfoList '" + dReport.FromDate.ToString() + "','" + dReport.ToDate.ToString() + "','" + dReport.Floor + "'";

            var result = _unitOfWork.attendanceRepository.SelectQuery<DropedNewEmployeeInfoViewModel>(query).ToList();
            return result;
        }

        public List<EarnedLeaveCalculationReportViewModel> GetEarnedLeaveCalculationReport(ReportFilteringViewModel dReport, int branchId)
        {
            var query = $"EXEC GetEarnedLeaveCalculationReport '" + dReport.Floor + "','" + dReport.EmployeeType + "','"+dReport.Month+"','"+dReport.Year+"'";

            var result = _unitOfWork.attendanceRepository.SelectQuery<EarnedLeaveCalculationReportViewModel>(query).ToList();
            return result;
        }

        public List<EmployeeLeaveHistoryViewModel> GetEmployeeLeavePostingReport(ReportFilteringViewModel dReport)
        {
            var query = $"EXEC GetLeavePostingDetails '" + dReport.FromDate + "','" + dReport.ToDate + "','" + dReport.Floor + "'";
            var result = _unitOfWork.attendanceRepository.SelectQuery<EmployeeLeaveHistoryViewModel>(query).ToList();
            return result;
        }

        public List<HolidayBillViewModel> GetHolidayBillReport(ReportFilteringViewModel dReport, int id)
        {
            var query = $"EXEC GetHolidayBillData '" + dReport.FromDate + "','" + dReport.ToDate + "','" + dReport.Floor + "','" + id + "','" + dReport.ShiftId + "','" + dReport.CardNo + "','" + dReport.DepartmentID + "'";
            var result = _unitOfWork.attendanceRepository.SelectQuery<HolidayBillViewModel>(query).ToList();
            return result;
        }

        public List<PromotionViewModel> GetPromotionReport(ReportFilteringViewModel dReport)
        {
            var query = $"EXEC GetPromotionData '" + dReport.FromDate + "','" + dReport.ToDate + "','" + dReport.Floor + "'";
            var result = _unitOfWork.attendanceRepository.SelectQuery<PromotionViewModel>(query).ToList();
            return result;
        }

        public List<MaternityViewModel> GetMaternityReport(ReportFilteringViewModel dReport)
        {
            var query = $"EXEC GetMaternityData '" + dReport.FromDate + "','" + dReport.ToDate + "','" + dReport.Floor + "'";
            var result = _unitOfWork.attendanceRepository.SelectQuery<MaternityViewModel>(query).ToList();
            return result;
        }

        public List<EidBonusVIewModel> GetEidBonusReport(ReportFilteringViewModel dReport, int branchId)
        {
            var query = $"EXEC GetEidBonusData '" + dReport.BankSalary + "','" + dReport.Floor + "','" + dReport.Company + "'";
            var result = _unitOfWork.attendanceRepository.SelectQuery<EidBonusVIewModel>(query).ToList();
            return result;
        }

        public List<RegisterOfWorkerViewModel> GetRegisterOfWorkerReport(ReportFilteringViewModel dReport)
        {
            var query = $"EXEC GetRegisterOfWorkerData '" + dReport.CardNo + "'";
            var result = _unitOfWork.EmployeeRepository.SelectQuery<RegisterOfWorkerViewModel>(query).ToList();
            return result;
        }

        public List<LeaveApprovalReportViewModel> GetLeaveApprovalReport(ReportFilteringViewModel dReport)
        {
            var query = $"EXEC GetLeaveApprovalData '" + dReport.LeaveApplicationID + "'";
            var result = _unitOfWork.EmployeeRepository.SelectQuery<LeaveApprovalReportViewModel>(query).ToList();
            return result;
            //var leaveDetails = (from l in _unitOfWork.LeaveAppRepository.Get()
            //                                             join e in _unitOfWork.EmployeeRepository.Get() on l.EmployeeID equals e.EmployeeID
            //                                             join d in _unitOfWork.DesignationRepository.Get() on e.DesignationID equals d.DesignationID
            //                                             join dept in _unitOfWork.DepartmentRepository.Get() on e.DepartmentID equals dept.DepartmentID
            //                                             where l.LeaveApplicationID == dReport.LeaveApplicationID
            //                                             select new LeaveApprovalReportViewModel
            //                                             {
            //                                                 EmployeeName = e.EmployeeName,
            //                                                 CardNo = e.CardNo,
            //                                                 Department = dept.DepartmentName,
            //                                                 Designation = d.DesignationName,
            //                                                 ApprovalDate = l.ApprovalDate,
            //                                                 JoinDate = Convert.ToDateTime(l.ToDate).AddDays(1),
            //                                                 LeaveDays= DbFunctions.DiffDays(l.FromDate, l.ToDate).Value + 1
            //                                             }).ToList();

            //return result;
        }

        public List<EmployeeDetailsViewModel> GetEmployeeBasicData(ReportFilteringViewModel dReport, int id)
        {
            var query = $"EXEC GetEmployeeBasicData '" + dReport.CardNo + "'";
            var result = _unitOfWork.EmployeeRepository.SelectQuery<EmployeeDetailsViewModel>(query).ToList();
            return result;
        }

        public List<SalaryIncrementViewModel> GetIncrementReportByCardNo(string cardNo)
        {
            List<SalaryIncrementViewModel> res = (from s in _unitOfWork.SalaryIncrementRepository.Get()
                                                  join e in _unitOfWork.EmployeeRepository.Get() on s.EmployeeID equals e.EmployeeID
                                                  where e.CardNo == cardNo
                                                  select new SalaryIncrementViewModel
                                                  {
                                                      SalaryIncrementID = s.SalaryIncrementID,
                                                      EmployeeID = s.EmployeeID,
                                                      IncrementAmount = s.IncrementAmount,
                                                      IncrementDate = s.IncrementDate,
                                                      CardNo = e.CardNo
                                                  }).ToList();
            return res;
        }
        public List<PromotionViewModel> GetEmployeeePromotionDetailsByCardNo(string cardNo)
        {
            List<PromotionViewModel> res = (from p in _unitOfWork.PromotionRepository.Get()
                                            join e in _unitOfWork.EmployeeRepository.Get() on p.EmployeeID equals e.EmployeeID
                                            where e.CardNo == cardNo
                                            select new PromotionViewModel
                                            {
                                                CardNo = p.CardNo,
                                                PromotionDate = p.PromotionDate,
                                                Remarks = p.Remarks,
                                                EmployeeName = e.EmployeeName,
                                                NewDesignationID = p.NewDesignationID,
                                                PreviousDesignationID = p.PreviousDesignationID
                                            }).AsEnumerable().ToList()
                                            .Select(x => new PromotionViewModel()
                                            {
                                                CardNo = x.CardNo,
                                                PromotionDate = x.PromotionDate,
                                                Remarks = x.Remarks,
                                                EmployeeName = x.EmployeeName,
                                                NewDesignation = GetDesignationName(x.NewDesignationID),
                                                PreviousDesignation = GetDesignationName(x.PreviousDesignationID)
                                            }).ToList();



            return res;
        }

        private string GetDesignationName(int designationID)
        {
            return (from d in _unitOfWork.DesignationRepository.Get()
                    where d.DesignationID == designationID
                    select d.DesignationName).SingleOrDefault();
        }

        public List<NoticeViewModel> GetNoticeHistoryByCardNo(string cardNo)
        {
            var res = (from p in _unitOfWork.publishNoticeRepository.Get()
                       join e in _unitOfWork.EmployeeRepository.Get() on p.EmployeeID equals e.EmployeeID
                       where e.CardNo == cardNo
                       select new NoticeViewModel
                       {
                           NoticeTitle = p.NoticeTitle,
                           NoticeDetail = p.NoticeDetails
                       }).ToList();
            return res;
        }

        public List<WorkerBusReportViewModel> GetWorkerBusReport(ReportFilteringViewModel dReport)
        {
            var query = $"EXEC GetWorkerBusData '" + dReport.Floor + "','" + dReport.FromDate + "'";
            var result = _unitOfWork.attendanceRepository.SelectQuery<WorkerBusReportViewModel>(query).ToList();
            return result;
        }

        public List<SalarySummaryViewModel> GetSalarySummaryReport(ReportFilteringViewModel dReport)
        {
            var query = $"EXEC GetSalarySummaryData '" + dReport.Month + "','" + dReport.Year + "'";
            var result = _unitOfWork.attendanceRepository.SelectQuery<SalarySummaryViewModel>(query).ToList();
            return result;
        }

        public List<OneTimeEntryReportViewModel> LeaveCorrectionReport(ReportFilteringViewModel dReport)
        {
            var query = $"EXEC GetLeaveCorrectionData '" + dReport.FromDate + "','" + dReport.ToDate + "','" + dReport.InTime + "','" + dReport.OutTime + "','" + dReport.LeaveTypeReport + "'";
            var result = _unitOfWork.attendanceRepository.SelectQuery<OneTimeEntryReportViewModel>(query).ToList();
            return result;
        }

        public List<MaternityReportViewModel> GetMaternityBillPayReport(ReportFilteringViewModel dReport)
        {
            var query = $"EXEC GetMaternityBillPayReportData '" + dReport.MaternityID + "'";
            var result = _unitOfWork.attendanceRepository.SelectQuery<MaternityReportViewModel>(query).ToList();
            return result;
        }

        public List<SubstituteHolidayReportViewModel> GetSubstituteHolidayReport(ReportFilteringViewModel dReport)
        {
            var res = (from l in _unitOfWork.LeaveAppRepository.Get()
                       join emp in _unitOfWork.EmployeeRepository.Get() on l.EmployeeID equals emp.EmployeeID
                       join d in _unitOfWork.DesignationRepository.Get() on emp.DesignationID equals d.DesignationID
                       where l.FromDate >= dReport.FromDate && l.ToDate <= dReport.ToDate && l.LeaveTypeID == 13
                       select new SubstituteHolidayReportViewModel
                       {
                           CardNo = emp.CardNo,
                           EmployeeName = emp.EmployeeName,
                           Designation = d.DesignationName,
                           TicketNo = emp.TicketNo,
                           WorkingDate = l.SubstituteDate,
                           SubstituteHolidayDate = l.FromDate,
                           Remarks = l.ReasonOfLeave
                       }).ToList();

            return res;
        }

        public List<EmployeeFinalBillReportViewModel> GetEmployeeFinalBillPaymentReport(ReportFilteringViewModel dReport)
        {
            var query = $"EXEC GetEmployeeFinalBillData '" + dReport.CardNo + "'";
            var result = _unitOfWork.attendanceRepository.SelectQuery<EmployeeFinalBillReportViewModel>(query).ToList();
            return result;
        }

        public List<EarnLeaveSummaryReportViewModel> GetEarnLeaveSummaryReport(ReportFilteringViewModel dReport)
        {
            var query = $"EXEC GetEarnLeaveSummaryReport '" + dReport.FromDate + "','" + dReport.ToDate + "','" + dReport.Floor + "'";
            var result = _unitOfWork.attendanceRepository.SelectQuery<EarnLeaveSummaryReportViewModel>(query).ToList();
            return result;
        }

        public List<EmployeeLeaveHistoryViewModel> GetEmployeeLeavePostingSummaryReport(ReportFilteringViewModel dReport)
        {
            var query = $"EXEC GetLeavePostingSummaryData '" + dReport.FromDate + "','" + dReport.ToDate + "','" + dReport.Floor + "'";
            var result = _unitOfWork.attendanceRepository.SelectQuery<EmployeeLeaveHistoryViewModel>(query).ToList();
            return result;
        }

        public List<SecurityGuardRosterViewModel> GetSecurityGuardRosterReport(ReportFilteringViewModel dReport)
        {
            var query = $"EXEC GetSecurityGuardRosterData '" + dReport.FromDate + "','" + dReport.ToDate + "'";
            var result = _unitOfWork.attendanceRepository.SelectQuery<SecurityGuardRosterViewModel>(query).ToList();
            return result;
        }

        public List<PrescriptionViewModel> GetPrescriptionReport(int prescriptionID)
        {
            List<PrescriptionViewModel> data = (from p in _unitOfWork.PrescriptionRepository.Get()
                                                join e in _unitOfWork.EmployeeRepository.Get() on p.EmployeeID equals e.EmployeeID
                                                where p.PrescriptionID == prescriptionID
                                                select new PrescriptionViewModel
                                                {
                                                    CardNo = e.CardNo,
                                                    EmployeeName = e.EmployeeName,
                                                    Description = p.Description,
                                                    LastModified = p.LastModified,
                                                    ModifiedBy=p.ModifiedBy,
                                                    PrescribedDate = p.PrescribedDate,
                                                    Pulse = p.Pulse,
                                                    Anaemia = p.Anaemia,
                                                    BP = p.BP,
                                                    Dehydretion = p.Dehydretion,
                                                    Eyes = p.Eyes,
                                                    Heart = p.Heart,
                                                    Jaundice = p.Jaundice,
                                                    Lungs = p.Lungs,
                                                    Oeadema = p.Oeadema,
                                                    Others = p.Others,
                                                    PAbdomen = p.PAbdomen,
                                                    PatientStatement = p.PatientStatement,
                                                    Respiration = p.Respiration,
                                                    Skin = p.Skin,
                                                    Temperature = p.Skin,
                                                    PrescriptionID=p.PrescriptionID
                                                }).ToList();

            return data;
        }
    }
}
