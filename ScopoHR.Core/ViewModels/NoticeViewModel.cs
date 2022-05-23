using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class NoticeViewModel
    {
        public int NoticeID { get; set; }
        [Required]
        public string NoticeTitle { get; set; }
        [Required]
        public string NoticeDetail { get; set; }
        public string ModifiedBy { get; set; }

    }
}
