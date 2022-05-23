using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class Sys_GraceViewModel
    {
        public int GraceID { get; set; }
        public int BeforeInTimeGrace { get; set; }
        public int AfterInTimeGrace { get; set; }
        public int BeforeOutTimeGrace { get; set; }
        public int AfterOutTimeGrace { get; set; }
        public bool IsApplicable { get; set; }
    }
}
