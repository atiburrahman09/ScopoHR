using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class EmployeeDetailsViewModel
    {
        public string CardNo { get; set; }
        public string EmployeeName { get; set; }
        public string MobileNo { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string Floor { get; set; }
        public string Line { get; set; }
        public decimal? Amount { get; set; }
        public int? LeaveDays { get; set; }
        public decimal? BasicSalary { get; set; }
        public decimal? HouseRent { get; set; }
        public decimal? MedicalAllowance { get; set; }
        public decimal? FoodAllowance { get; set; }
        public decimal? Conveyance { get; set; }
        public int? MedicalLeave { get; set; }
        public int? CasualLeave { get; set; }
        public int? MaternityLeave { get; set; }
        public int? compensatoryleave { get; set; }
        public int? earnedleave { get; set; }
        public string SearchFloor { get; set; }
        public Nullable<DateTime> Date { get; set; }
        public Nullable<DateTime> JoinDate { get; set; }
        public int? TicketNo { get; set; }
        public Nullable<DateTime> LastModified { get; set; }
        public string ModifiedBy { get; set; }
        public Int16? Grade { get; set; }
        public int? SL { get; set; }
        public int? SH { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string PresentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string Gender { get; set; }
        public string BloodGroup { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}

