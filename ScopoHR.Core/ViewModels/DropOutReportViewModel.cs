using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class DropOutReportViewModel
    {
        public string CardNo { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public Nullable<DateTime> JoinDate { get; set; }
        public Nullable<DateTime> AbsentFrom { get; set; }
        public Nullable<DateTime> FromDate { get; set; }
        public int AbsentDays { get; set; }
        public string Remarks { get; set; }
        public int NoticeID { get; set; }
        public string MobileNo { get; set; }
        public Nullable<DateTime> DropOutDate { get; set; }
        public string DepartmentName { get; set; }
        public string FatherName { get; set; }
        public string SpouseName { get; set; }
        public string PermanentAddress { get; set; }
        public string PresentAddress { get; set; }
        public decimal? Salary { get; set; }
        public Int16 SalaryGrade { get; set; }
    }
}
