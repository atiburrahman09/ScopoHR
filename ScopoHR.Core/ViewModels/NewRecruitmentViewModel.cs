using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class NewRecruitmentViewModel
    {
        public string CardNo { get; set; }
        public string EmployeeName { get; set; }
        public string DesignationName { get; set; }
        public decimal? BasicSalary { get; set; }
        public decimal? Conveyance { get; set; }
        public decimal? FoodAllowance { get; set; }
        public decimal? MedicalAllowance { get; set; }
        public decimal? HouseRent { get; set; }
        public Nullable<DateTime> JoinDate { get; set; }
        public Int16? SalaryGrade { get; set; }
    }
}
