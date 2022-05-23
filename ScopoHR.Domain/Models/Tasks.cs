using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ScopoHR.Domain.Models
{
    public class Tasks : BaseEntity
    {
        [Key]
        public int TaskID { get; set; }
        public string TaskTitle { get; set; }
        public string Description { get; set; }
        public int? AssignedTo { get; set; }

        public DateTime PlannedStartDate { get; set; }
        public DateTime PlannedEndDate { get; set; }
        public int PlannedManHour { get; set; }

        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public int? ActualManHour { get; set; }

        public int Status { get; set; }
        public int Priority { get; set; }
        public string AttachmentPath { get; set; }

        public int Owner { get; set; }
        public DateTime EntryDate { get; set; }

        public int ProjectID { get; set; }
    }
}
