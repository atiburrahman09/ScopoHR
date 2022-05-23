using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class JobApplication : BaseEntity
    {
        public int JobApplicationId { get; set; }
        public int JobCircularId { get; set; }
        [ForeignKey("JobCircularId")]
        public virtual JobCircular JobCirculars { get; set; }
        public string CandidateName { get; set; }
        public string CandidateEmail { get; set; }
        public DateTime CreatedDate { get; set; }
        public float ExpectedSalary { get; set; }
        public string CandidateResume { get; set; }
        public int BranchID { get; set; }
    }
}
