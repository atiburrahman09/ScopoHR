using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class EmployeeBioDataViewModel
    {
        public string CardNo { get; set; }
        public string EmployeeName { get; set; }
        public string MobileNo { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public decimal? BasicSalary { get; set; }
        public decimal? Conveyance { get; set; }
        public decimal? FoodAllowance { get; set; }
        public decimal? MedicalAllowance { get; set; }
        public decimal? HouseRent { get; set; }
        public Nullable<DateTime> DateOfBirth { get; set; }
        public string FatherName { get; set; }
        public int? GenderID { get; set; }
        public Nullable<DateTime> JoinDate { get; set; }
        public string GrandFatherName { get; set; }
        public short MaritalStatus { get; set; }
        public string NID { get; set; }
        public string NomineeName { get; set; }
        public string PermanentAddress { get; set; }
        public string PresentAddress { get; set; }
        public string SpouseName { get; set; }
        public string BloodGroup { get; set; }
        public string MotherName { get; set; }
        public Int16? SalaryGrade { get; set; }

    }
}
