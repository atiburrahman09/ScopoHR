using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class Project
    {
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDesc { get; set; }
        public int Status { get; set; }
        public decimal Budget { get; set; }
        public Nullable<DateTime> DelivaryDate { get; set; }
        public int ClientID { get; set; }
    }
}
