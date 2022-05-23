using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class SalaryMappingViewModel
    {
        public int EmployeeID { get; set; }
        public List<SalaryTypeAmountViewModel> SalaryTypeAmountList { get; set; }
    }

    public class SalaryTypeAmountViewModel
    {
        public int SalaryTypeID { get; set; }
        public string SalaryTypeName { get; set; }
        public decimal? Amount { get; set; }
    }
}
