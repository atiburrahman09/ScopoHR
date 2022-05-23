using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class MonthlySalary
    {
        public int MonthlySalaryId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int EmployeeId { get; set; }
        public bool IsTaken { get; set; }
        public int BranchID { get; set; }
        public string CardNo { get; set; }
        public string Name { get; set; }
        public string Grade { get; set; }
        public decimal? Basic { get; set; }
        public decimal? HouseRent { get; set; }
        public decimal? MedicalAllowance { get; set; }
        public decimal? Conveyance { get; set; }
        public decimal? FoodAllowance { get; set; }
        public decimal? GrossWage { get; set; }
        public string PD { get; set; }
        public string HD { get; set; }
        public string CasualLeave { get; set; }
        public string MedicalLeave { get; set; }
        public string MaternityLeave { get; set; }
        public string FLeave { get; set; }
        public string TWD { get; set; }
        public decimal? AttendanceBonus { get; set; }
        public string AbsentDays { get; set; }
        public decimal? Advance { get; set; }
        public decimal? Food { get; set; }
        public decimal? PF { get; set; }
        public decimal? PayableWages { get; set; }
        public decimal? OTH { get; set; }
        public decimal? OTRate { get; set; }
        public decimal? OTTaka { get; set; }
        public decimal? TotalPay { get; set; }
        public string JoiningDate { get; set; }
        public int? ShortLeave { get; set; }
        public int? SHLeave { get; set; }
        public decimal? Tax { get; set; }
        public decimal? Fine { get; set; }
        public decimal? Loan { get; set; }
        public string Floor { get; set; }
        public int? EmployeeType { get; set; }
        public int? DepartmentID { get; set; }
        public int? DesignationID { get; set; }
    }
}
