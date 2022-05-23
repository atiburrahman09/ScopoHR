using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class CardNoMappingViewModel
    {
        public int Id { get; set; }
        public string OriginalCardNo { get; set; }
        public string GeneratedCardNo { get; set; }
        public string Gender { get; set; }
    }
}
