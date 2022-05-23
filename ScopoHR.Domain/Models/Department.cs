using ScopoHR.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    
    public class Department : BaseEntity
    {        
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentNameBangla { get; set; }
        public int BranchID { get; set; }
    }
}
