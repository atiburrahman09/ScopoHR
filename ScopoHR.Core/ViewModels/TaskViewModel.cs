using ScopoHR.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class TaskViewModel
    {
        public int TaskID { get; set; }
        [Required]
        public string TaskTitle { get; set; }        
        public string Description { get; set; }
        public int? AssigneeID { get; set; }
        public string AssigneeName { get; set; }
        public string AssigneeCardNo { get; set; }
        [Required]
        public DateTime PlannedStartDate { get; set; }
        [Required]
        public DateTime PlannedEndDate { get; set; }
        [Required]
        public int PlannedManHour { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public int? ActualManHour { get; set; }
        public int Status { get; set; }
        public int Priority { get; set; }
        public string AttachmentPath { get; set; }
        public int Owner { get; set; }
        public string OwnerName { get; set; }
        public string OwnerCardNo { get; set; }
        public DateTime EntryDate { get; set; }
        public string ModifiedBy { get; set; }
        public int ProjectID { get; set; }
    }
}
