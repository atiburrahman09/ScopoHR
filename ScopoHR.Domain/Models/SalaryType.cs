using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class SalaryType : BaseEntity
    {
        public int SalaryTypeID { get; set; }
       
        public string SalaryTypeName { get; set; }
    }
}
