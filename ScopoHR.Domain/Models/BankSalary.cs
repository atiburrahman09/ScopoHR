using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
   public class BankSalary : BaseEntity
    {
        [Key]
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public string CardNo { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string AccountNo { get; set; }
        public string BankName { get; set; }
        public string Company { get; set; }
    }
}
