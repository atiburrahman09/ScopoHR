using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class ProjectViewModel
    {
        public int ProjectID { get; set; }

        [Required]
        public string ProjectName { get; set; }

        public string ProjectDesc { get; set; }
        public int Status { get; set; }
        public decimal Budget { get; set; }
        public Nullable<DateTime> DelivaryDate { get; set; }
        public int ClientID { get; set; }
    }
}
