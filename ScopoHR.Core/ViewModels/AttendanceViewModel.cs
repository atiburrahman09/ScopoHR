using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class AttendanceViewModel
    {
        public int AttendanceID { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime InOutTime { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public int? DepartmentID { get; set; }
        public string CardNo { get; set; }
        public Nullable<DateTime> InTime { get; set; }
        public Nullable<DateTime> OutTime { get; set; }
        public Nullable<DateTime> InTimeDate { get; set; }
        public Nullable<DateTime> OutTimeDate { get; set; }
        public short? OverTimeHour { get; set; }
        public string Remarks { get; set; }
        public string ModifiedBy { get; set; }
        public string AttendanceType { get; set; }
        public int? HasAttendance { get; set; }

        public string Day { get; set; }
        public int? WorkingHours { get; set; }
        public int? OTMinutes { get; set; }
        public int LunchTime { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
    public class AttendanceDataViewModel
    {
        public int AttendanceID { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime InOutTime { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string EmployeeName { get; set; }
        public string CardNo { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string Remarks { get; set; }

        public string Day { get; set; }
        public int? WorkingHours { get; set; }
        public int? OTMinutes { get; set; }
        public int LunchTime { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string InTimeDate { get; set; }
        public string OutTimeDate { get; set; }
    }

    public class SearchAttendanceViewModel
    {
        [Required]
        public DateTime FromDate { get; set; }
        public Nullable<DateTime> ToDate { get; set; }
        public string CardNo { get; set; }
        public string FloorName { get; set; }
        public bool AbsentOnly { get; set; }
        public string ShiftId { get; set; }
    }

    public class BiometricDeviceViewModel
    {
        public int Id { get; set; }
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public DateTime InServiceSince { get; set; }
        public DateTime? LastSync { get; set; }
        public int MachineNumber { get; set; }
    }
}
