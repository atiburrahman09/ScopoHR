using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class ManpowerPlanningViewModel
    {
        public int PlanID { get; set; }
        public int DepartmentID { get; set; }
        public int DesignationID { get; set; }
        public int SectionID { get; set; }
        public int ProductionFloodID { get; set; }
        public Int16 SalaryGrade { get; set; }
        public int Budget { get; set; }
        public DateTime? LastModified { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }

    


}
