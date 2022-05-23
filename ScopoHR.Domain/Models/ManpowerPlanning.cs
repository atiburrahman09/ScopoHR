using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class ManpowerPlanning : BaseEntity
    {
        [Key]
        public int PlanID { get; set; }        
        public int DepartmentID { get; set; }
        public int DesignationID { get; set; }
        public int SectionID { get; set; }
        public int ProductionFloodID { get; set; }
        public Int16 SalaryGrade { get; set; }
        public int Budget { get; set; }
    }
}
