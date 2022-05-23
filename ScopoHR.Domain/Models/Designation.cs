using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class Designation : BaseEntity
    {
        public int DesignationID { get; set; }
        public string DesignationName { get; set; }
        public string DesignationNameBangla { get; set; }
        public int DepartmentID { get; set; }
        public bool IsOverTimeApplicable { get; set; }
    }
}
