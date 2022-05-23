using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class JobApplicationViewModel
    {
        public int JobApplicationId { get; set; }
        public int JobCircularId { get; set; }
        public string CandidateName { get; set; }
        public string CandidateEmail { get; set; }
        public DateTime CreatedDate { get; set; }
        public float ExpectedSalary { get; set; }
        public string CandidateResume { get; set; }
    }
}
