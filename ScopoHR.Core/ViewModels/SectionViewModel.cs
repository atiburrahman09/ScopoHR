using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class SectionViewModel
    {
        public int SectionID { get; set; }
        public string SectionName { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}
