using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class JobCircularViewModel
    {
        public int JobCircularId { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public DateTime DueDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
